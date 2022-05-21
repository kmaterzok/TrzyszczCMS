# Solution projects

## Frontend
* ```Core.Application``` - it contains services, models and other components deployed in the frontend, in separation from the targeted platform. This project serves the main business logic for the frontend. Used ONLY by the frontend.
* ```TrzyszczCMS.Client``` - Frontend. Its main purpose is presenting content obtained from backend by REST API. The architectural pattern deployed here is MVVM. Almost every view (page) has its own view model. For management of views (HTML and CSS manipulation, JavaScript invocation) the dedicated methods are used, implemented as static methods in static classes.

## Backend
* ```Core.Infrastructure``` - Entity Framework Core, ORM generated entities, migrations by FluentMigrator. Used ONLY by the backend.
* ```Core.Server``` - it contains services, models and other components deployed in the backend, in separation from the targeted platform. This project serves the main business logic for the backend. Used ONLY by the backend.
* ```TrzyszczCMS.Server``` - The startup project, backend. It contains controllers handling REST requests.

## Common parts
* ```Core.Infrastructure.Shared``` - tables' constraints and their values used in the whole application. Used by the frontend and by the backend.
* ```Core.Shared``` - the elements common for the frontend and for the backend such as classes, constants and enums.

## Additional projects
* ```TrzyszczCMS.Docs``` - The simplified documentation of the project.
* ```TrzyszczCMS.Tests``` - Unit test approving that the crucial classes and functions are well projected and working without flaws.

# Overview of ```Core.Shared```

## Models
Models used for data download and management are placed in ```Models``` directory. The models contain in their names the following prefixes:
* ```Simple``` - This model stores basic info data. It is used only for reading data by the frontend. It is not used for database data management.
* ```Detailed``` - This model stores data used for modifying and persisting in the database by sending it to the backend by REST API. The model is utilised for database data management.

## Components
* ```ModuleContent``` - stores a data object of specified type that is responsible for storing page module data. All supported objects have names with suffix _ModuleContent_.
* ```Result<T,E>``` - it stores a result of operation execution, whether it is a success result object or an error result object.
* ```ExceptionMaker``` - the following component is responsible for creating exception to be thrown in explicitly defined places in code. Used to mitigate code repetition.
* ```CommonConstants``` - the static class of all shared constant values for the solution.
* ```LocalConstants``` - the static class of locally used constant values. Not used outside the project.

# Overview of ```Core.Infrastructure```

## CQRS
There is a separation between read-ready database server handling SELECT (loading page data) queries and the second one handling all the others (CRUD operations, page management). The settings are placed in class ```ConnectionStrings```.

## Entity models
* Stored in ```/Models/Database```.
* Meaning of prefixes used in the naming convention:
	* ```Auth``` - authentication or authorisation
	* ```Cont``` - page content
* Convention for naming n:m relationship table entities: Prefix + table names without prefixes + ```Assign```, e.g. ```AuthRolePolicyAssign``` for ```AuthRole``` and ```AuthPolicy```.

## Migrations
* All migrations are stored in ```/Migrations``` directory of the project.
* Naming of migration classes is as follows: ```DatabaseVer``` + version with dots replaced with underscores, e.g. ```DatabaseVer_1_1_3``` for version 1.1.3.
* ```Migration``` attribute contains a version number as follows: 3 digits of major + 3 digits of minor + 3 digits of patch, e.g. ```[Migration(1001003)]``` for version 1.1.3.

## Foreign keys
* The class ```ForeignKeys``` contains all foreign keys' names for ones currently used (subclass ```Current```) and the obsolete ones (subclass ```Obsolete```).
* Naming of the keys: primary key table + foreign key table + field in the table with the foreign key, e.g. ```AuthToken_AuthUser_AssignedUserId```.

## Creating connection
The classes responsible for creating database connections are as follows:
* ```DatabaseStrategyFactory : IDatabaseStrategyFactory``` - Creating objects implementing the interface ```IDatabaseStrategy```.
* ```PgsqlDatabaseStrategy : IDatabaseStrategy``` - Object of access to the database. It allows getting database context for handling the database with Entity Framework Core.

# Overview of ```Core.Infrastructure.Shared```
The following project contains elements shared in the whole solution.

The constraints are placed in the static class ```Constraints```. It contains subclasses named for names of tables that use constraints. Their members are constants named by the name of the tables' columns and hold values specific for max allowed size of value that is stored in the table's row's cell.

The static class ```UserPolicies``` holds in constants all the policies' names that describe what every user in the system can or cannot do. The names are used in the frontend and in the backend.

# Overview of ```Core.Server```
The following project contains all the business logic for the backend.
# Classes of helpers
* ```CryptoHelper``` - cryptographic methods used for secure data processing or creating,
* ```FilterDataParser``` - parses filtering conditions for table data,
* ```FilterExtensions``` - contains static methods that filter data provided by ```Queryable``` instances of data delivered by Entity Framework Code,
* ```MappingExtensions``` - contains static methods that remap data between instances of different classes,
* ```RepetitiveTask``` - utilised for invoking actions that must be run repetitively until some conditions occur so invoking stops,
* ```SemaphoredValue``` - utilised for synchronous invocation of methods for a specific value that must be guarded against data races.

# Classes of models
* _Adapters_ - adapters for classes utilised in this project,
* _Crypto_ - cryptographically oriented models that store data for processing or usage in other classes,
* _Enums_ - enum types
* _Extensions_ - model-oriented extension methods that return them,
* _Settings_ - settings-oriented models for storing settings,
* ```Constants``` - static class holding constants for usage in the backend.

# Services
The services are split into 4 different categories:
* ```DbAccess/Modify``` - services that allow to modify data in the database (a part of CQRS), dedicated for usage with frontend during managing the system,
* ```DbAccess/Read``` - services that allow to only read data from the database (a part of CQRS), dedicated for usage in the frontend during presenting pages content,
* the rest of ```DbAccess```
	* ```AuthDatabaseService``` - authorisation-oriented data handling,
	* ```RepetitiveTaskService``` - all task that must be executed cyclically or repetitively when the backend runs.
* unclassified services:
	* ```ICryptoService``` - cryptographically-oriented data handling with settings applied from the settings instance,
	* ```IStorageService``` - reading and saving files used on all the pages created by authorised users.

# Overview of ```TrzyszczCMS.Server```
The startup project consisting of:
* _Controllers_,
* _Data_ - data oriented types,
* _Handlers_ - used for handling data in the whole backend, specifically this project,
* _Helpers_ - helper methods for usage in this project, e.g. ```HttpContext``` helpers for simplified data getting,
* _appsettings.json_ and _appsettings.development.json_ - settings of the backend,
* ```Startup``` - the class responsible for initialisation of the backend. It contains dedicated methods for registering of services and configuring. Services are registered as scoped ones.

## Controllers
Every controller class must:
* inherit ```ControllerBase``` class,
* have a name ending with ```Controller``` word,
* have applied attributes:
	* ```ApiController```,
	* ```Route```,
	* ```Authorize``` - only if all the methods must be available to users if authorised,
	* ```RequireHttps``` - applied if the requirement of HTTPS usage is sternly required for applying of higher security transport.

Additionally, controllers' methods must:
* have applied attributes:
	* defining a method, e.g. ```HttpPost```, ```HttpGet```, etc.
	* ```Produces``` - expresses a MIME type of the returned content for the client, except for deletion only if needed,
	* ```Route``` - defines a route to the controller's method. Routes of methods are created as follows: ```[action]/``` + sequence of parameters from the method, e.g. ```[Route("[action]/{arg1}/{arg2}")]``` for method ```MyMethod(string arg1, int arg2)```
	* ```Authorize``` - only if the controller does not apply this attribute and the authorisation is required.

## Authentication & authorisation
* To authorise and/or authenticate a user, the HTTP request is required to have the authorisation header - its name is stored in the constant ```HEADER_AUTHORIZATION_NAME```. The logic of header check is done in ```BasicAuthenticationHandler``` class instance.
* To authorise access to a controller or a controller's endpoint, there must be placed an attribute ```Authorize``` above the corresponding method or class. The attribute must have defined a policy that tells what kind of policy is required to pass the execution. NOTE: This way of authorisation is used when a class or endpoint required only one policy to be checked.
* To authorise access with requirement of 2 or more policies at once, another mechanism is utilised. There is the extension method ```HasUserPolicy``` which lets check whether a user of a specific request has assigned a policy to itself.

# Overview of ```Core.Application```
The following project is responsible for frontend's business logic.

## Classes of helpers
* ```PageFetcher``` - the utility for loading paginated data page by page from a specified data source. Used for paginated tables.
* ```DelayedInvoker``` - the utility for invoking methods after a specified time span. Used for saving changes.

## Classes of models
* deposits - the classes used to store data for exchange between 2 or more other classes - the first one puts the deposit class in the depository (store for deposits), the other ones get it during instantiating (in a constructor) or during a method execution.

## Services
All services are split into 2 types:
* database data management - these have name beginning with ```Manage```,
* the other ones, e.g. page data loading and authentication.

# Overview of ```TrzyszczCMS.Client```
The main frontend project consisting of:
* Data & model classes - contains classes typical for usage in this project:
	* ```GridItem``` - wrapper for a table row,
	* _JSInterop_ classes - used during JavaScript invocation,
	* _PolicyClearance_ - used to distinguish a set of required policies to display or access data,
	* _Enum extensions_ - processes enums or converts them into other objects,
	* Constants.
* Helpers,
* Services:
	* ```AuthService``` - authentication & authorisation,
	* ```DataDepository``` - store for exchanging data through deposits (mementos),
	* ```JSInteropService``` - invoking JavaScript,
	* ```TokenService``` - accessing the authorisation token.
* Views,
* View models,
* ```Program``` - the class responsible for preparing WebAssembly client in the browser. ```Main``` method of the class registers services and view models through dedicated methods usage. Services are registered as scoped ones, view models as transient ones.
* Other classes:
	*  ```MarkDownFormatter``` – used for formatting MarkDown code and making changes directly into a piece of code. It utilises some factory methods for various cases of similar formatting.

## Authorisation & authentication
* To authenticate and/or authorise a user in the backend the client must set an authorisation header in the HTTP request. It is done by usage of ```TokenHeaderHandler``` class. There are 2 HTTP clients in use – the unauthenticated one not setting any authorisation header – and the second one that sets header if available in the Local Storage with use of the aforementioned handler. Services use them and to get the desired one the service does it during the instantiation. The clients are distinguished by its names stored in the constants – ```HTTP_CLIENT_ANON_NAME``` and ```HTTP_CLIENT_AUTH_NAME```.
* To determine if a view has to display a certain data, the instance of ```ApplicationAuthenticationStateProvider``` is used. The view checks if a user is signed in.
* To check if the signed-in user has assigned the sufficient permissions to do something, it is checked with usage of clearances. If there is possibility to allow doing something, there must be done a check of clearance – it is done using ```IAuthService``` (method ```HasClearanceAsync```). If the clearance (a predefined condition of checks of permissions) exists – the operation might be done with usage of a view.

## Views & view models
* Every view model is always created as an adequate view is created and must access the view model.
* View models inherit class ```ViewModelBase``` that contains basic method for notifying changes.
* Any view model is always used for a certain instance of a view and cannot be reused for another instance of a view of the same class. e.g.: if we enter a page and the view model is created for it the application must not use the same instance of view model after entering another instance of the page (caused by refreshing or going back and forth). Hence, view models are registered as transient ones, not scoped ones or singletones.
* ```IDataDepository``` is utilised for exchange of data between different view models instances.
* View model processes data and if there is a need of managing things that do views, e.g. navigation, dedicated ```EventHandler```s should be created for clarity of code and achieving of segregation of purposes of every class. For the same reason, ```Popupper``` must be used in view instances only.
* To display a pop-up there should be used ```Popupper``` – a class that lets display a pop-up within a view and manage its content during execution. A sample use of this component is implemented in ```Settings``` behind code.

# Testing (```TrzyszczCMS.Tests```)
* All the unit tests are stored within this project. The settings for creating the class are default, except for this one - Test project: _TrzyszczCMS.Tests_
* Convention for naming: ```Test_``` + tested issue + additional info separated with underscore, e.g. ```Test_GetNext_ThrowException```
