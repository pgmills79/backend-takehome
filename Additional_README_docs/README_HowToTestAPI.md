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
4. Run The project in debug mode pointing at the **RatingEngine** or **IIS Express**
	1. When running IIS Express, you may receive an authentication error when the browser appears. To fix this, navigate to \backend-takehome\RatingEngine\.vs\RatingEngine\config and open the applicationhost.config file and make sure anonymousAuthentication is set to true
	 
	 '''
	 <authentication>
	  <anonymousAuthentication enabled="true" />
	  <windowsAuthentication enabled="false" />
	 </authentication> 
	 '''
	2. Save the file, rebuild the project and run again, the access and should have access now
	   
5. switch to Postman and go ahead and send the response (`POST`)

Response with a payload that is the correct premium amount (example above being the amount in following example):    
```
{
    premium: 11316
}
```   

## Step 2B (Swagger):  Testing Quote API from Swagger
1. Run The project in debug mode pointing at the RatingEngine Project configuration
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
    

    