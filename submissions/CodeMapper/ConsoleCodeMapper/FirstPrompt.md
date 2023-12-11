# Your goal
You are going to produce a mapping between the codes of two systems: (source) and (destination).
The remainder of this text will give you information and examples of the data you will be working with.
At the end I will provide you with an action plan, for you to execute once I tell you to 'proceed'.

Do not yet start to execute the action plan, but just absorb all the information and tell me what your plan is succintly.
Provide a short sample result as well so we can be sure you match the format outlined below.

## Information on codes
A code is a data structure that contains the following fields: 
- Id
- CategoryId
- CategoryDescription
- Code
- Description

Each code belongs to a category.
Each category can have multiple codes.

## How do we map codes?
Codes are mapped following this logic:

1. For each of the categories in SourceCodes.csv    
    1. Find the matching category in DestinationCodes.csv (refer to 'How do we map categories?' in this document)
    1. Get all the codes of the source category
    1. Get all the codes of the destination category
        1. Find the matching code in the destination category  (refer to 'How do we map codes?' in this document)

## How do we map categories?

1. We semantically match them by description. For instance:
    1. (source)'Eye Color Codes' is semantically similar to (destination) 'Eye Colors' or (destination) 'Codes for the eyes'
    1. (source)'Eye Color Codes' is not semantically similar to (destination) 'Incident Types'.

IMPORTANT: semantic meaning means that we can't just compare the strings, but we need to compare the meaning of the strings.

## How do we map codes?

1. We compare the field 'code' and see if there's a direct match.
1. We semantically match them by description. For instance:
    1. (source)'Blue' is semantically similar to (destination)'BLUE'
    1. (source)'BLU' is not semantically similar to 'BURG' and 'DRUG

IMPORTANT: 
1. When processing a category's codes, codes should always be filtered by category id.
2. We should never compare codes from different categories in (source) and (destination).

## Where to find the codes?
1. The codes for the (source) system are in the file `SourceCodes.csv`
1. The codes for the (destination) system are in the file `DestinationCodes.csv`

## Information on mapping
The end result is a list of records with these fields: 

- MappingId
- SRC_Id
- SRC_CategoryId
- SRC_CategoryDescription
- SRC_Code
- SRC_Description
- DEST_Id
- DEST_CategoryId
- DEST_CategoryDescription
- DEST_Code
- DEST_Description

1. **Make sure to output the result as outlined in the 'Example of a mapping ...' below**
2. The prefix SRC_ refers to the source system and DEST_ refers to the destination system.

### Examples of codes from (source)
```
Id,CategoryId,CategoryDescription,Code,Description
4,2,Eye Color Codes,BLU,Blue
6,2,Eye Color Codes,GRN,Green
7,2,Eye Color Codes,GRY,Grey
42,5,Incident Type Codes,BURG,Burglary
44,5,Incident Type Codes,DRUG1,Drug Possession
1,1,Sex Codes,F,Female
2,1,Sex Codes,M,Male
3,1,Sex Codes,01,
```

### Examples of codes from (destination)
```
Id,CategoryId,CategoryDescription,Code,Description
125,5,Color - Eyes,BLK,BLACK
126,5,Color - Eyes,BLU,BLUE
127,5,Color - Eyes,BRO,BROWN
129,5,Color - Eyes,GRY,GRAY
21,1,Incident Types,Burglary,Burglary
22,1,Incident Types,Carjacking,Carjacking
33,1,Incident Types,Escort,Escort
78,2,Gender,F,Subject's gender reported as female
79,2,Gender,G,"Occupation or charge indicated ""Male Impersonator"""
80,2,Gender,M,Subject's gender reported as male
81,2,Gender,N,"Occupation or charge indicated ""Female Impersonator"" or transvestite"
82,2,Gender,X,Unknown gender
83,2,Gender,Y,"Male name, no gender given"
84,2,Gender,Z,"Female name, no gender given"
```

### Example of a mapping referencing the source and destination code examples
```
MappingId,SRC_Id,SRC_CategoryId,SRC_CategoryDescription,SRC_Code,SRC_Description,DEST_Id,DEST_CategoryId,DEST_CategoryDescription,DEST_Code,DEST_Description
0001,42,5,Incident Type Codes,BURG,Burglary,21,1,Incident Types,Burglary,Burglary
0002,44,5,Incident Type Codes,DRUG1,Drug Possession,,,,,(UNMAPPED)
0003,1,1,Sex Codes,F,Female,78,2,Gender,F,Subject's gender reported as female
0004,2,1,Sex Codes,M,Male,80,2,Gender,M,Subject's gender reported as male
0005,3,1,Sex Codes,01,,,,,,(UNMAPPED)
0006,4,2,Eye Color Codes,BLU,Blue,126,5,Color - Eyes,BLU,BLUE
0007,6,2,Eye Color Codes,GRN,Green,,,,,(UNMAPPED)
0008,7,2,Eye Color Codes,GRY,Grey,129,5,Color - Eyes,GRY,GRAY
```

**IMPORTANT**: when a code match is not possible, DEST_Description should contain the text '(UNMAPPED)', just as in the example above.