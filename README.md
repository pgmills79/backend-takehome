# How to Run the Quote Engine API

## Step 1:  Get Remote Repository for Project
1. From your IDE of choice (Visual Studio, Jetbrains Rider, etc..) connect to remote repository which is located at https://github.com/pgmills79/backend-takehome
2. The solution to open all necessary projects is located at https://github.com/pgmills79/backend-takehome/tree/main/RatingEngine/RatingEngine.sln
   1. **Note:**  There are **5 total projects** so you will want to open the entire solution (RatingEngine.sln) in IDE

## Step 2A (Postman):  Testing Quote API from Postman
1. **Before Running The project in IDE**
2. Open Postman
    1. If you want to just import the request I included the json file that can be imported directly into Postman (File Name: RatingEngine.postman_collection.json)
    2. You can also create a new `POST` response and use this URL: http://localhost:5001/api/quotes  (5001 is the port I am using)

The response receives a Payload in the body. 
```
Example:        
{
    revenue: 6000000,
    state: 'TX',
    business: 'Plumber'
}
```     
3. Save the above request and body information (from example)
   1. Run The project in debug mode pointing at the **RatingEngine** or **IIS Express**
       1. When running IIS Express, you may receive 'HTTP Error 401.2 - Unauthorized' message when the browser appears. To fix this, navigate to \backend-takehome\RatingEngine\.vs\RatingEngine\config and open the applicationhost.config file and make sure anonymousAuthentication is set to true

       ``` 
        <authentication>
         <anonymousAuthentication enabled="true" />
         <windowsAuthentication enabled="false" />
        </authentication> 
       ``` 
       2. Save the file, rebuild the project and run again, and there should be access now
	   
4. Switch to Postman and go ahead and send the response (`POST`)

Response with a payload that is the correct premium amount (example above being the amount in following example):    
```
{
    premium: 11316
}
```   

## Step 2B (Swagger):  Testing Quote API from Swagger
1. Run The project in debug mode pointing at the RatingEngine Project configuration
   1. If a 'HTTP Error 401.2 - Unauthorized' message appears see 3.i.a above to resolve this
2. When the browser launches it goes directly to swagger
3. On the Swagger UI, click on the `POST` section just under Quotes and click 'Try it out'
4. Enter any values for revenue, State, and business and click 'Execute' to see the response
5. The response should send back a payload with the correct premium amount:
        
Example `POST` payload:
```
{
    revenue: 6000000,
    state: 'TX',
    business: 'Plumber'
}
```

Expected response:
```
{
    premium: 11316  
}
```

## Step 2C (Test Projects):  Testing Quote API from Integration and Unit Test Projects
In the solution are two projects for testing the API
    
1. RatingEngine.IntegrationTests
      1. These tests are to test the controllers down through the logic and receive the response to test all components
      2. Open up RatingEngine/RatingEngine.IntegrationTests/QuotesControllerTests.cs, right click at top of the class and choose "Run All"
      3. All tests should be successful   
2. RatingEngine.UnitTests
     1. This project was setup to test the logical units that calculate the different factors (state, business, base premium)
     2. Open up RatingEngine/RatingEngine.UnitTests/RatingEngineTests.cs, right click at top of the class and choose "Run All"
     3. All tests should be successful
	 
	 

# Future Enhancements to the Quotes API

## Addition #1:  Data Validation
Adding datavalidation using FluentValidation library (why reinvent the wheel??).  Instead of using multiple data annotations on a model we can use FluentValidation for all validation to seperate validation from the model.

Example data annotations on model (***before using FluentValidation***) 
```
public class Payload
{
	[Required]
	public decimal Revenue { get; set; }

	[Required]
	public string State { get; set; }

	[Required]
	public string Business { get; set; }
}
```

Using FluentValidation we can set it in Startup.cs

```
services.AddControllers().AddFluentValidation(s =>
{
	s.RegisterValidatorsFromAssemblyContaining<Startup>();
	s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
});
```

Then use a validation class at the API layer:
```
public class PayloadValidator : AbstractValidator<Payload>
{
	public PayloadValidator()
	{
		RuleFor(p => p.Revenue).NotEmpty().WithMessage("{PropertyName} should NEVER BE Empty!");
		
		RuleFor(p => p.Revenue).GreaterThanOrEqualTo(0);
	}
}
```

HTTP response when user forgets to input a revenue amount:
```
"errors": {
        "Revenue": [
            "Revenue should NEVER BE Empty!"
        ]
 }
```

HTTP response when user puts a negative revenue amount:
```
"errors": {
        "Revenue": [
            "'Revenue' must be greater than or equal to '0'."
        ]
}
```

## Addition #2:  Move the factoring ratios and calculations to database
We can move the data from the excel file to a database and be able to store the different factor values, states, and jobs to tables so that we can add/remove/update values.  I can see needing to add all states and many more job titles into the factoring and so having them stored in the database would be better for scalability

Some examples: 
```
Table Name: State (columns: state_fullname, state_abbreviation, factor)
Table Name: Business (columns: business_name, factor)
```

## Addition #3:  With Factoring in database create generic repository pattern
With the factoring data now stored in a database I would structure the API using a generic repository pattern and an action handler class to make scalability easier as endpoints are adding to the system.

The structure would look similar to this:

Controller:

```
[HttpGet("{id}")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProcessDefinitions))]
public async Task<ModelNameReturned> GetById([FromRoute] InfrastructureClassCommand aTypeofTheCommand)
{
   var results 
       = await _actionHandler.ModelName.GetByIdAsync(aTypeofTheCommand);
   
   return results.FirstOrDefault();
}
```

Action Handler ('_actionHandler') which resides back in an infrastructure project with the repositories allows for easy dependency injection of all services using the repositories

```
public interface IActionHandler
    {
        IModelNameRepository ModelName { get; }
        
        //Put any new services here
        //I[name here]Repository  { get; }
    }

    public class ActionHandler : IActionHandler
    {
        public ActionHandler(IModelNameRepository)
        {
            ModelName = modelNameRepository;
        }

        public IModelNameRepository ModelName { get; }
        
        //Put any new services here
        //I[name here]Repository  { get; }
    }
```

Generic repository class (IGenericRepository.cs) which resides back in an infrastructure project with the repositories allows for easy additions of repositories (scalability).  Below example shows simple CRUD operations the service is able to run

```
public interface IGenericRepository<T, G, I> 
        where T : class 
        where G : class
        where I : class
    {
        Task<IEnumerable<T>> GetAllAsync(G getAllCommand);
        Task<IEnumerable<T>> GetByIdAsync(I getByIdCommand);
    }
```

Specific repository class which resides back in an infrastructure project.  The example is using Dapper ORM for the database calls

```
public class ModelNameRepository : IModelNameRepository
    {
        
        private readonly SqlConnection _dbConnection;

        public ModelNameRepository(SqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        
        public async Task<IEnumerable<ModelName>> GetAllAsync(GetAllModelName modelName)
        {
            await using var con = new SqlConnection(_dbConnection.ConnectionString);
            con.Open();

            var sql = @"SELECT * from Table;";

            return (await con.QueryAsync<ModelName>(sql)).ToList();

        }

        public Task<IEnumerable<Processes>> GetByIdAsync(GetModelNameById getByIdCommand)
        {
            throw new System.NotImplementedException();
        }         

 }   
    
```

This type fo structure allows for scalability as more and more API endpoints are added and make boiler plate code which can be easily copied/pasted and I use Jetbrains Rider which allows me to have templates setup allowing me to wire up an API endpoint from controller to repository quickly.


## Addition #4:  API Versioning
The project would have API versionin on it.

in Program.CS to setup versioning:

```
services.AddApiVersioning(config =>
{
    // Specify the default API Version
    config.DefaultApiVersion = new ApiVersion(1, 0);
    // If the client hasn't specified the API version in the request, use the default API version number 
    config.AssumeDefaultVersionWhenUnspecified = true;
    // Advertise the API versions supported for the particular endpoint
    config.ReportApiVersions = true;

    // Supporting multiple versioning scheme
    config.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("X-version"), 
        new QueryStringApiVersionReader("api-version"));
});
```

The controller would then have the API version and route to reflect versioning:

```
 [ApiVersion("1.0")]
 [Route("api/v{version:apiVersion}/[controller]")]
```

## Addition #5:  IIS Express configurations and authentication
I was having to manually edit the applicationhost.config file to allow anonymous authentication each time the project was run which was time consuming so I would look at way to have that setting always persisted.  Of course, anonymous authentication would not be allowed in most cases in real world.  

I would add OAuth authentication to the application so that it is more secure