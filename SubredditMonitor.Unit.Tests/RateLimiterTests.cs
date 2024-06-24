using Moq;
using SubredditMonitor.Core.Services;

namespace SubredditMonitor.Unit.Tests
{
    [TestClass]
    public class RateLimiterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        public void SetRateLimitDelayThrowsArugmentNullExceptionForNullArgument()
        {
            // act
            var result = RateLimitDelayCalculator.CalculateRequestDelayBasedOnRateLimits(null);
        }
        #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetRateLimitDelayThrowsArugmentExceptionForEmptyArgument()
        {
            // arrange
            var fakeHeaders = new Mock<IReadOnlyCollection<RestSharp.HeaderParameter>>();

            // act
            var result = RateLimitDelayCalculator.CalculateRequestDelayBasedOnRateLimits(fakeHeaders.Object);
        }

        [TestMethod]
        public void RateLimitDelayCalculatorGetsCorrectResultBasedOnInputRateHeaders()
        {
            // act
            var result = RateLimitDelayCalculator.CalculateDelay("600.0", "300");

            // assert
            Assert.AreEqual(result, 500);
        }

        [TestMethod]
        public void RateLimitDelayCalculatorAlwaysReturnsResetMillisecondsIfRequestsExhausted()
        {
            var result = RateLimitDelayCalculator.CalculateDelay("0.0", "10");
            Assert.AreEqual(result, 10000);

            result = RateLimitDelayCalculator.CalculateDelay("0.0", "300");
            Assert.AreEqual(result, 300000);

        }

    }
}