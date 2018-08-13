# ITEM Base Solution

The architecture for enterprise application should have at least the following levels:
1. Entity Layer: Contains entities (POCOs)
2. Data Layer: Contains all related code to database access
3. Business Layer: Contains definitions and validations related to business

|	Layer	|	Content	|	Has Interface	|	Prefix	|	Remarks	|
	-----------	|	-----------	|	-----------	|	-----------	|	-----------	|
|	Entity Layer	|	DB Models	|	No	|	(none)	|	Properties are mapped to DB fields	|
|	Data Layer	|	DB Context and	|	No	|	(none)	|		|
|		|	           DB Configurations	|	No	|	Configuration	|	Configuration: Object property to DB field mapping	|
|		|	DB Repositories	|	Yes	|	Repository	|		|
|	Business Layer	|	DB Service	|	Yes	|	Service	|		|

