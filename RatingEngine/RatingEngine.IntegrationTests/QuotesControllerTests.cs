using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

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
        [InlineData("6000000", "FL", "Plumber","14400")]
        [InlineData("357200", "FL", "Programmer","2148")]
        [InlineData("6000000", "OH", "Plumber","12000")]
        [InlineData("300000", "OH", "Programmer","1500")]
        [InlineData("300000", "TX", "Architect","1131.60")]
        [InlineData("500000", "TX", "Programmer","2357.50")]
        public async Task POST_Payloads_Should_Return_Correct_Premium_Amounts(string revenue, string state, 
            string business, string expectedResult)
        {
            // Arrange
            var u = new Uri("https://localhost/api/quotes");
            var payload = $@"{{""revenue"":{Convert.ToInt32(revenue)},""state"": ""{state}"",""business"": ""{business}"" }}";

            HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync(u, c);
            var data = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
            var premiumAmount = data.Descendants()
                                    .OfType<JProperty>()
                                    .FirstOrDefault(x => x.Name == "premium")
                                    ?.Value.ToObject<decimal>();
           
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal($"{Convert.ToDecimal(expectedResult):.##}", $"{premiumAmount:.##}");
        }
    }
}