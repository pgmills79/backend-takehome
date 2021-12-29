using System;
using RatingEngineCore.HelperClasses;
using Xunit;

namespace RatingEngine.UnitTests
{
    public class RatingEngineTests
    {
        [Theory]
        [InlineData("OH","1")]
        [InlineData("FL","1.2")]
        [InlineData("TX","0.943")]
        public void GetStateFactor_Should_Return_Correct_Value(string state,string expectedResult)
        {
            //Arrange
            
            //Act
            var factor = Factors.GetStateFactor(state).ToString("0.###");
            
            //assert
            Assert.Equal(expectedResult, factor);
        }
        
        [Theory]
        [InlineData("Architect","1")]
        [InlineData("Plumber","0.5")]
        [InlineData("Programmer","1.25")]
        public void GetBusinessFactor_Should_Return_Correct_Value(string business,string expectedResult)
        {
            //Arrange
            
            //Act
            var factor = Factors.GetBusinessFactor(business).ToString("0.###");
            
            //assert
            Assert.Equal(expectedResult, factor);
        }
        
        [Theory]
        [InlineData("6000000","6000")]
        [InlineData("250000","250")]
        [InlineData("357000","357")]
        public void GetBasePremium_Should_Return_Correct_Value(string premium,string expectedResult)
        {
            //Arrange
            var premiumAmount = Convert.ToDecimal(premium);
            
            //Act
            var factor = Factors.GetBasePremium(premiumAmount).ToString("0.###");
            
            //assert
            Assert.Equal(expectedResult, factor);
        }
    }
}