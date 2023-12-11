# Code Mapper

Code Mapper is an application that uses artificial intelligence to map list of lookup codes from two separate systems.
Such mappings are often needed when integrating systems or migrating data from a system to another.

The mapping of codes is often a very laborious and error prone work, and lists of codes might have thousand of entries that needs human attention and require hours, if not days, to complete. 
Data exchange between systems often cannot commence until the mapping is complete, and this can cause delays in the implementation of new systems or the migration of existing ones.

---

Code Mapper was built for the second Semantic Kernel Hackathon with the following goals in mind:

1. To create a tool that shows how SK can be used to solve real-world IT problems.    
1. To create a Semantic Kernel plugin to helps with the mapping process.
1. To experiment with a large prompt and the new SK planners.
1. To have fun! :smiley:

*Note: CM was built using data inspired by the law enforcement domain, but it can be used in any domain (e.gh. healthcare, warehousing, etc.).**

## Why Code Mapper?

Imagine you need to integrate two systems (source and destination) that use different codes to identify the same things.

For instance, imagine categories of codes such as 'Eye Color Codes', 'Hair Color Codes', etc.
Everytime an insert or an update occurrs in the source system, the destination system needs to be notified and updated accordingly, but the values of the lookup codes will likely be different.

The source system might have categories such as'Eye Color Codes' (CategoryId=2) and 'Sex Codes' (CategoryId=1):

| Id  | CategoryId | CategoryDescription | Code | Description             |
| --- | ---------- | ------------------- | ---- | ----------------------- |
| 4   | 2          | Eye Color Codes     | BLU  | Blue                    |
| 5   | 2          | Eye Color Codes     | BRO  | Brown                   |
| 6   | 2          | Eye Color Codes     | GRN  | Green                   |
| 7   | 2          | Eye Color Codes     | GRY  | Grey                    |
| 8   | 2          | Eye Color Codes     | He has light red eyes  | He has light red eyes   |
| 9   | 2          | Eye Color Codes     | HAZ  | Hazel                   |
| 10  | 2          | Eye Color Codes     | H    | Haz                     |
| 11  | 2          | Eye Color Codes     | PNK  | Pink                    |
| 12  | 2          | Eye Color Codes     | P    | P                       |
| 13  | 2          | Eye Color Codes     | B    | B                       |
| 1   | 1          | Sex Codes           | F    | Female      |
| 2   | 1          | Sex Codes           | M    | Male        |

The destination system has instead a category 'Color - Eyes' (Id=5) with the following values:

| Id  | CategoryId | CategoryDescription | Code | Description    |
| --- | ---------- | ------------------- | ---- | --------------- |
| 125 | 5          | Color - Eyes        | BLK  | BLACK           |
| 126 | 5          | Color - Eyes        | BLU  | BLUE            |
| 127 | 5          | Color - Eyes        | BRO  | BROWN           |
| 128 | 5          | Color - Eyes        | GRN  | GREEN           |
| 129 | 5          | Color - Eyes        | GRY  | GRAY            |
| 130 | 5          | Color - Eyes        | HAZ  | HAZEL           |
| 131 | 5          | Color - Eyes        | MAR  | MAROON          |
| 132 | 5          | Color - Eyes        | MUL  | MULTICOLORED    |
| 133 | 5          | Color - Eyes        | PNK  | PINK            |
| 134 | 5          | Color - Eyes        | XXX  | UNKNOWN         |
| 78  | 2          | Gender              | F    | Subject's gender reported as female         |
| 79  | 2          | Gender              | G    | Occupation or charge indicated "Male Impersonator"  |
| 80  | 2          | Gender              | M    | Subject's gender reported as male           |
| 81  | 2          | Gender              | N    | Occupation or charge indicated "Female Impersonator" or transvestite |
| 82  | 2          | Gender              | X    | Unknown gender                              |
| 83  | 2          | Gender              | Y    | Male name, no gender given                  |
| 84  | 2          | Gender              | Z    | Female name, no gender given                |

In order to integrate the two systems, the codes in the source system need to be mapped to the codes in the destination system.

## Challenges
1. How do we automatically match the codes in the source system to the codes in the destination system?
    1. The match is fairly easy when the **codes** have the same code and/or descriptions 
        1. ```Eye Color Codes/BLU/Blue``` matches the destination code ```Color Eyes/BLU/BLUE```.    
    1. Other matches such as the **category** are not so obvious:
          1. How did we match ```Eye Color Codes``` to ```Color Eyes```?
1. What about more complicated cases like ```Sex Code/F/Female``` to ```Gender/F/Subject's gender reported as female```
    1. While the code 'F' is the same, the descriptions are wildly different.     
    1. What if the mapping were ```Sex Code/F/Female``` to ```Gender/2311/Subject's gender reported as female```? 
        1. How do we match codes that have nothing in common, except **semantically**?

Code Mapper uses a combination of machine learning and natural language processing to facilitate the mapping of codes. 

## How does it work?

1. The user prepares two CSV files containing the source codes and destination codes.
    1. These files need to be placed in the ```\Data``` subfolder, for the app to have access to them.
        1. Check [SourceCodes.csv](/ConsoleCodeMapper/Data/SourceCodes.csv) to see all the codes in the source system.
        1. Check [DestinationCodes.csv](/ConsoleCodeMapper/Data/DestinationCodes.csv) to see all the codes in the destination system.
Take a look at the 

1. The user starts Code Mapper and begins interacting with it via a command-line chat interface.

# Architecture



### Contact Info
**Name:** Alessandro Federici

**Email:** alef@freemindlabs.com