using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RatingEngine.IntegrationTests
{
    public class QuotesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        
        public QuotesControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        
        [Theory]
        [InlineData("600000", "FL", "Architect","28800")]
        /*[InlineData("/Index")]
        [InlineData("/About")]
        [InlineData("/Privacy")]
        [InlineData("/Contact")]*/
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string revenue, string state, 
            string business, string expectedResult)
        {
            // Arrange
            var u = new Uri("https://localhost:5001/api/quotes");
            var payload = $@"{{""revenue"":{Convert.ToInt32(revenue)},""state"": ""{state}"",""business"": ""{business}"" }}";
           

            HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync(u, c);

            // Assert
            var data = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
            var premiumAmount = data.Descendants()
                                    .OfType<JProperty>()
                                    .FirstOrDefault(x => x.Name == "premium")
                                    ?.Value;
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            
            
            Assert.Equal("text/html; charset=utf-8", 
                response.Content.Headers.ContentType.ToString());
        }
    }
}