# How to Run the Quote Engine API

## Step 1:  Get Remote Repository for Project
-From your IDE of choice (Visual Studio, Jetbrains Rider, etc..) connect to remote repository which is located at -> https://github.com/pgmills79/backend-takehome
-The solution to open all necessary projects is located at backend-takehome/RatingEngine/RatingEngine.sln
  --Note:  There are 5 total projects so you will want to open the entire solution (RatingEngine.sln) in IDE

## Step 2A (Postman):  Testing Quote API from Postman
-Before Running The project in IDE
    -Open Postman
    -If you want to just import the request I included the json file that can be imported directly into Postman (File Name: RatingEngine.postman_collection.json)
    -You can also create a new `POST` response and use this URL: https://localhost:5001/api/quotes  (5001 is the port I am using)

The response receives a Payload in the body. 
```
Example:        
{
    revenue: 6000000,
    state: 'TX',
    business: 'Plumber'
}
```     
-Save the above request and body information (from example)
-Run The project in debug mode pointing at the RatingEngine Project configuration
-switch to Postman and go ahead and send the response (`POST`)

-Response with a payload that is the correct premium amount (example above being the amount in following example):    
```
{
    premium: 11316
}
```   

## Step 2B (Swagger):  Testing Quote API from Swagger
-Run The project in debug mode pointing at the RatingEngine Project configuration
    -When the browser launches it goes directly to swagger
-On the Swagger UI, click on the `POST` section just under Quotes and click 'Try it out'
-Enter any values for revenue, State, and business and click 'Execute' to see the response
-The response should send back a payload with the correct premium amount:
        
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
-In the solution I have created two projects for testing the code
    1. RatingEngine.IntegrationTests
      a. These tests are to test the controllers down through the logic and receive the response to test all components
      b.  Open up RatingEngine/RatingEngine.IntegrationTests/QuotesControllerTests.cs, right click at top of the class and choose "Run All"
      c.  All tests should be successful
    2. RatingEngine.UnitTests
      a. This project was setup to test the logical units that calculate the different factors (state, business, base premium)
      b.  Open up RatingEngine/RatingEngine.UnitTests/RatingEngineTests.cs, right click at top of the class and choose "Run All"
      c.  All tests should be successful
    

    