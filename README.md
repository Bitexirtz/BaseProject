# ITM Base Solution

## Architecture
The architecture includes the following levels:
1. Entity Layer: Contains entities (POCOs)
2. Data Layer: Contains all related code to database access
3. Business Layer: Contains definitions and validations related to business
4. Presentation Layer: Contains User Interfaces

| Layer | Content | Has Interface | Prefix | Remarks |
 ----------- | ----------- | ----------- | ----------- | ----------- |
| Entity Layer | DB Models | No | (none) | Properties are mapped to DB fields |
| Data Layer | DB Context and | No | (none) |  |
|  |            DB Configurations | No | Configuration | Configuration: Object property to DB field mapping |
|  | DB Repositories | Yes | Repository |  |
| Business Layer | DB Service | Yes | Service | DB Models are converted to UI Models and vice-versa |
| Presentation Layer | UI Models | No | Model |  |
|  | View Models | No | ViewModel |  |
|  | View | No | (none) |  |

## HowTo: Add new Database Table
| No. | Content | Target Project | Remarks |
 ----------- | ----------- | ----------- | ----------- |
| 1. | Add new class for the table | [Itm.Database.Entities](Itm.Database.Entities) | Class name is mapped as table name and properties as field names. |
| 2. | Configure table mappings | [Itm.Database.Context](Itm.Database.Context) | Can specifify primary key, rename fields, etc. |
| 3. | Create a repository: class and interface | [Itm.Database.Repositories](Itm.Database.Repositories) |  |
| 4. | Create business logic | [Itm.Database.Services](Itm.Database.Services) | This is called by UI. Entities(DB) are mapped with Models(UI). |

## HowTo: Add new Module
(soon)

## Tips
DO NOT USE: Will set User properties to NULL if property not exists in UserModel. Use instead: Mapper.Map(updates, user);
