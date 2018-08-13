# ITM Base Solution

The architecture for enterprise application should have at least the following levels:
1. Entity Layer: Contains entities (POCOs)
2. Data Layer: Contains all related code to database access
3. Business Layer: Contains definitions and validations related to business

| Layer | Content | Has Interface | Prefix | Remarks |
 ----------- | ----------- | ----------- | ----------- | ----------- |
| Entity Layer | DB Models | No | (none) | Properties are mapped to DB fields |
| Data Layer | DB Context and | No | (none) |  |
|  |            DB Configurations | No | Configuration | Configuration: Object property to DB field mapping |
|  | DB Repositories | Yes | Repository |  |
| Business Layer | DB Service | Yes | Service |  |

# How to add a new Table and access it to UI
| No. | Content | Target Project | Remarks |
 ----------- | ----------- | ----------- | ----------- |
| 1. | Add new class and with properties. | Itm.Database.Entities | Class name is mapped as table name and properties as field names. |
| 2. | No | Itm.Database.Context |  |
