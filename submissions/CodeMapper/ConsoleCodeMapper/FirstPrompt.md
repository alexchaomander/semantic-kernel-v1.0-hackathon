# Your goal
You are going to produce a mapping between the codes of two systems: (source) and (destination).
I will explain the data structures and other details below.
Once you are done absorbing this information, explain your plan of action to me.
Ask any questions that you might need.

## Information on codes
A code is a data structure that contains the following fields: 
- Id
- CategoryId
- CategoryDescription
- Code
- Description

Each code belongs to a category.
Each category can have multiple codes.

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
0001,4,2,Eye Color Codes,BLU,Blue,126,5,Color - Eyes,BLU,BLUE
0002,6,2,Eye Color Codes,GRN,Green,,,,,
0003,7,2,Eye Color Codes,GRY,Grey,129,5,Color - Eyes,GRY,GRAY
0004,42,5,Incident Type Codes,BURG,Burglary,21,1,Incident Types,Burglary,Burglary
0005,44,5,Incident Type Codes,DRUG1,Drug Possession,,,,,
0006,1,1,Sex Codes,F,Female,78,2,Gender,F,Subject's gender reported as female
0007,2,1,Sex Codes,M,Male,80,2,Gender,M,Subject's gender reported as male
0008,3,1,Sex Codes,01,,,,,,
```