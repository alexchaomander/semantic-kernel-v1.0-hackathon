using GemBox.Document;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class WordWriter
    {
        public string Key { set; get; }
        public WordWriter()
        {
            var setting = AppSettings.LoadSettings();
            Key = setting.GemboxKey;
        }
        public byte[] WriteToDocx(List<ContentObject> PageContents)
        {
            try
            {
                // If using the Professional version, put your serial key below.
                ComponentInfo.SetLicense(Key);

                DocumentModel document = new DocumentModel();

                foreach (var content in PageContents)
                {
                    Section section = new Section(document);
                    document.Sections.Add(section);

                    Paragraph paragraph = new Paragraph(document);
                    section.Blocks.Add(paragraph);
                    var contentbreaks = content.Content.Split("\n");
                    foreach(var contentbreak in contentbreaks)
                    {
                        Run run = new Run(document, contentbreak);
                        paragraph.Inlines.Add(run);
                    }
                 
                }
                var ms = new MemoryStream();
                document.Save(ms, SaveOptions.DocxDefault);
                return ms.ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
