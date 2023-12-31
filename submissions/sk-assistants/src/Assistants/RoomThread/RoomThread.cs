﻿// Copyright (c) Kevin BEAUGRAND. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HandlebarsDotNet;
using Microsoft.SemanticKernel.AI.ChatCompletion;

namespace SemanticKernel.Assistants.RoomThread;

internal class RoomThread : IRoomThread
{
    public IReadOnlyList<ChatMessageContent> ChatMessages => throw new NotImplementedException();

    private readonly Dictionary<IAssistant, IThread> _assistantThreads = new();

    public event EventHandler<ChatMessageContent>? OnMessageReceived;

    internal RoomThread(IEnumerable<IAssistant> agents)
    {
        this._assistantThreads = agents.ToDictionary(agent => agent, agent =>
        {
            var thread = agent.CreateThread();
            thread.AddSystemMessage(this.GetRoomInstructions()(new
            {
                agent,
                participants = agents
            }));

            return thread;
        });

        this.OnMessageReceived += (sender, message) =>
        {
            var agent = sender as IAssistant;

            this.DispatchMessageRecievedAsync(agent!.Name!, message); // TODO fix to run it synchronously
        };
    }

    public async Task AddUserMessageAsync(string message)
    {
        await this.DispatchMessageRecievedAsync("User", message).ConfigureAwait(false);
    }

    /// <summary>
    /// Dispatches the message to all recipients.
    /// </summary>
    /// <param name="sender">The sender of the message (can be the agent name or "User").</param>
    /// <param name="message">The message..</param>
    /// <returns></returns>
    private async Task DispatchMessageRecievedAsync(string sender, string message)
    {
        await Task.WhenAll(this._assistantThreads
                     .Where(c => c.Key.Name != sender)
                     .Select(async assistantThread =>
                     {
                         var response = await assistantThread.Value.InvokeAsync($"{sender} > {message}")
                                                                    .ConfigureAwait(false);

                         if (response.Content.Equals("[silence]", StringComparison.OrdinalIgnoreCase))
                         {
                             return;
                         }

                         this.OnMessageReceived!(assistantThread.Key, response);
                     })).ConfigureAwait(false);
    }

    private HandlebarsTemplate<object, object> GetRoomInstructions()
    {
        var roomInstructionTemplate = this.ReadManifestResource("RoomMeetingInstructions.handlebars");

        IHandlebars handlebarsInstance = Handlebars.Create(
           new HandlebarsConfiguration
           {
               NoEscape = true
           });

        var template = handlebarsInstance.Compile(roomInstructionTemplate);

        return template;
    }

    private string ReadManifestResource(string resourceName)
    {
        var promptStream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{typeof(RoomThread).Namespace}.{resourceName}")!;

        using var reader = new StreamReader(promptStream);

        return reader.ReadToEnd();
    }
}
