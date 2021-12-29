using System.Text.Json.Serialization;

namespace RatingEngineCore.Models
{
    public class Premium
    {
        [JsonPropertyName("premium")]
        public decimal PremiumAmount { get; set; }
    }
}