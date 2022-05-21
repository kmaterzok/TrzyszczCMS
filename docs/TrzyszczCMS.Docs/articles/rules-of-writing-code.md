
The project's architecture and implementation implies the specific manners that ease the code readability and maintenance of the solution for future.

# Architectural convention

* Methods of helper classes are always implemented as extension methods. This way the code invocation for objects is eased, code footprint is lower and the methods are able to be invoked in a chain (one by one). Lest there isn't any specific object that the method is invoked for, the method is required to be static (e.g. ```SpecificHelper.SpecificMethod(arg1, arg2)``` instead of ```arg1.SpecificMethod(arg2)```).
* Invoking methods for objects should be implemented as chain invocation, as it eases readability and the code of the business logic.
* Whilst defining ```switch```es' implementation, remaining cases must throw an exception that comes from ```ExceptionMaker```. This is a mechanism for explicit informing of unsupported, unexpected or unforeseen cases that a programmer might have not covered.
* All methods that handle data for further processing with database data with usage of Entity Framework must contain contracts. Their purpose is to detect invalid data or unprocessable cases that cannot let the further execution to be done.
* To avoid writing repetitive code a few classes exist, specifically for:
	* making exceptions to be thrown (```ExceptionMaker```),
	* validators of inputs' value data (```ValidationHelper```),
	* helper methods.
* All constants are required to be placed in static classes. This approach lets a programmer get to know what purpose of a constant it is in the code. Moreover, it's easier to determine all usage cases in the code by following their references.
* Crucial database data operations in the backend must be logged into a log using ```ILogger<T>``` instance as information entries.


# General naming convention

The convection for the following project is typical for _C#_ language but a few exceptions which trying to increase the code readability:

* Constant values are expressed by upper snake case naming, e.g. ```public const int MAX_OVERALL_TIME_MINUTES = 3```,
* Properties that express constant values in another way (e.g. by convertion) are expressed as above, e.g. ```public int MAX_OVERALL_TIME_HOURS => MAX_OVERALL_TIME_MINUTES * 60```,
* Naming classes, enums, methods, properties, values of enums: PascalCase,
* Naming properties and fields used in the _razor_ pages implemented directly in code-behind, local variables and local constants: camelCase,
* Naming private fields of classes: camelCase name starting with underscore, e.g. ```private readonly IMyService _myService```.

# Code indentation

The indentation between pieces of code should be taken with care. Operation of the same type which are repeated line by line should be aligned to each other, so readability of the code will be achieved.

The example of applying is presented below:

```
Create.Table(nameof(AuthUser))
 .WithColumn(nameof(AuthUser.Id))                .AsInt32().NotNullable().PrimaryKey().Identity()
 .WithColumn(nameof(AuthUser.Username))          .AsString(Constraints.AuthUser.USERNAME).NotNullable().Unique()
 .WithColumn(nameof(AuthUser.Description))       .AsString(Constraints.AuthUser.DESCRIPTION).Nullable().WithDefaultValue(null)
 .WithColumn(nameof(AuthUser.PasswordHash))      .AsBinary(128).NotNullable()
 .WithColumn(nameof(AuthUser.PasswordSalt))      .AsBinary(32).NotNullable()
 .WithColumn(nameof(AuthUser.Argon2Iterations))  .AsInt32().NotNullable()
 .WithColumn(nameof(AuthUser.Argon2MemoryCost))  .AsInt32().NotNullable()
 .WithColumn(nameof(AuthUser.Argon2Parallelism)) .AsInt32().NotNullable()
 .WithColumn(nameof(AuthUser.AuthRoleId))        .AsInt32().NotNullable();
```

The following example comes from the migration code. It applies aligning of lines and segregation of keywords by its true use, e.g. column names of ```AuthUser``` table are in one line. It also applies to chains of methods applied to all the table rows. A similar approach can be taken for assigning values for properties or fields of any instance.



# Git commits
The name of commit consists of 2 parts: the type of commit and the description.

Type of commits:
* ```feat``` - feature,
* ```fix``` - fix of a bug,
* ```refact``` - refactoring of the code,
* ```upgrade``` - upgrading or updating parts of libraries or versions,
* ```docs``` - writing documentation,
* ```misc``` - unclassified things.

Examples:
* refact: Optimised loading
* feat: Changing page's data

The following principle of writing commits lets searching them more easily.