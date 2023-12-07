# Code Mapper
[ :hammer: by FML]
[ :hammer: FML image here]
[ :hammer: GPL license here]

## Description

Code Mapper is an application that uses artificial intelligence to map list of codes from two separate systems.

It was built for the second Semantic Kernel Hackathon with the following goals in mind:

1. Create a useful tool and proof-of-concept that shows how SK can be used to solve real-world problems
1. Create a plugin that wraps some of Elasticsearch's API
	1. NLP to create indices
	1. NLP to search indices	
	1. more?
1. Showcase the use of KM as a memory plugin for SK
	1. Test FML's Elasticsearch adapter
1. Learn new things and have fun!

our goal was to showcase the use of FML's ES adapter and the use of KM as a memory plugin for SK.

## Why Code Mapper?

The mapping of codes is often a very laborious and error prone work, and lists of codes might have thousand of entries that needs human attention and require hours, if not days, to complete. Data exchange between systems often cannot commence until the mapping is complete, and this can cause delays in the implementation of new systems or the migration of existing ones.

[ :hammer: image showing codes or something]

Code Mapper uses a combination of machine learning and natural language processing to facilitate the mapping of codes. 

## How does it work?

1. The user prepares a spreadsheet containing the codes to be mapped using a simple Excel spreadsheet.
[see CodeMatrixData.xlsx for an example  :hammer: ]
1. The user starts Code Mapper and a chat session starts

```
CM> Hi, I am Code Mapper, how can I help you?
User> I need you to map the codes in a spreadsheet.
CM> Please provide me with the name of the file.
User> The file is called CodeMatrixData.xlsx
CM> The document is valid and I found bla bla
Do you want me to start the mapping process?
User>yay!
```

:hammer: Magic happens then 
```
CM> I have found 1000 codes in the spreadsheet bla bla
CM> I have found 13 categories in the spreadsheet bla bla
much more...
CM> the results are in [possibly ES]
```


# Architecture


1. [ :hammer: architecture diagram here]
	1. [show the flow of the data and the components involved]

# Wishlist
- It should use KM as memory plugin for SK
- It should showcase the use of FML's ES adapter
	- It should use FML's ES adapter to store the mappings and the codes

- Store the codes and the mappings in KM/ES
	- This show cases how easy it is to create adapters for KM/ES
	- It also provides us access to statistical analysis of the mappings and the codes
	
- Maybe a UI to see the results in a nice format?
	- Just output text file? JSON? Excel?
	- Just add a new page in the original spreadsheet? (no time to code this)
	- Dashboard in Kibana would be nice



### Contact Info
**Name:** Alessandro Federici

**Email:** alef@freemindlabs.com