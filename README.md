# Templates
**Markdown Guide** - [Markdown Guide](https://www.markdownguide.org).\
**Solution Name**  - Templates.\
**Development Environment** - Visual Studio 2022 IDE using Windows 11.\
**Database**  - SQL Server or SQL Server.\
**Languages** - Razor HTML (HTML5, CSS3 & C#).\
**Status**    - Done.\
**Purpose**   - To Create 4 base projects which will form the foundation of future solutions.
**Description** - Create the new solution with the appropriate projects.  Ensure everything works before continuing including project references, etc.

## Motivation
**Purpose** - The ASP.Net Core 8 Frameworks can be used to create a project.  However, it will usually require reconfiguring to create, this Solution is used to set the main projects.       

## 1. Application Project
**Framework** - ASP.Net Core Web App (Models-View-Controller).\
**Template Name** - MH ASP.Net 8 App
**Name** - App
**Status**  - Done.\
**Purpose** - This is primarily a Graphical User Interface (GUI) it contains the views and controllers for those views. Must be included if a GUI is required.

## 2. Minimal API Project
**Framework** - ASP.Net Core Web API & Entity Framework Core.\
**Template Name** - MH ASP.Net 8 Minimal API
**Name** - MinAPI
**Status**   - Done.\
**Purpose**  - To Perform CRUD operations on the Data source as required, This Project contains the Endpoints. Must be included if access to the data source is required. 

## 3. Unit Testing Project
**Framework** - xUnit & Fluent Assertions.\
**Template Name**  - MH UnitTests.\
**Name** - UnitTests
**Status**   - Done.\
**Purpose**  - To encourage code is robustly developed.\
**Use** - Tests should be added to this project in the Tests folder as required so testing is done in parallel with writing the methods.  Also when an existing method is amended the tests should be re-run.

## Test Naming Conventions
1. **Class Name** - Will be located in the Tests folder, <Class Neme>Tests.cs so if the class is called SampleClass the name will be called "SampleClassTests.cs"
2. **Method Name** - <Method Name being tested>_<Return Type> so a method called SampleMethod that returns a string will be called "SampleMethod_String"
3. **Fact Test** - Will have no Parameters but will be set inside the test method.
4. **Theory Test** - Will use the same parameters of the method being tested and the result  

### Methods Tested
1. **Local Methods**    - Methods wrote in this solution.  
2. **Function Methods** - Methods that return a value. 
3. **Dynamic Methods**  -  Methods that are either changed regularly or Critical.
4. **Single Unit of Work** - Methods that only do one thing - these are known as testable methods.

### Methods Not Tested
1. **3rd Party Software** - Any Software that is included in the project but not wrote in this solution.  I.E. Frameworks, etc.
2. **Void Methods**   - Methods that do not return a value, As unit testing relies on the return value.
3. **Static Methods** - Methods that are not changed.
4. **Multiple Unit of Work** - Methods that are too complex to test - these can and should be broken down into workable methods.  
5. **Methods that require a database** - These will be tested in Integration Testing.

## 4. Domain
**Framework** - ASP.Net Core 8 Class Library.\
**Template Name** - MH Domain.\
**Name** - Domain
**Status**   - Done.\
**Purpose**  - Customised Solution Class Library. which contains all Classes for the Solution. Must always be included.

## License
See the [MIT](LICENSE) license for rights and limitations under the conditions of the license.
