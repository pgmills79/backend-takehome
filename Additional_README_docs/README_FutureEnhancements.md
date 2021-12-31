# Future Enhancements to the Quotes API

## Addition #1:  Error Handling
I would add error handling for if invalid input was entered from an external service talking to the API.  For example if a comma ',' is entered in the revenue value, the response back gives the generic '400 bad request' message.  I would send back a more accurate message (**side note**:  I can also design the API to accept commas for the revenue)

I would create a model called called 'ErrorResponse' or something along that line to be able to give a standard message back for whichever error is given.  For example, if the user enters an invalid state abbreviation we can return a message stating 'Invalid State'

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

With this project, I did NOT take advantage of dependency injection and running services in the structure because it would have been an overkill so it makes sense I would add this into the future enhancements


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