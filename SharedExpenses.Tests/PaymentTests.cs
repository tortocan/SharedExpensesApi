using SharedExpensesApi.Models;
using Xunit;
using System;

namespace SharedExpenses.Tests
{
    public class PaymentTests : UnitTest
    {
        [Fact]
        [Trait("Category","Unit")]
        public void PaymentShouldHaveDate()
        {
            // Given
            var expected = DateTime.UtcNow;
            var sut = new PaymentResponse() { Date = expected };
            sut.Date = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected, result.Date);
        }

        [Fact]
        [Trait("Category","Unit")]
        public void PaymentShouldHaveAmount()
        {
            // Given
            var expected = 321.123;
            var sut = new PaymentResponse() { Amount = expected };
            sut.Amount = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected, result.Amount);
        }

        [Fact]
        [Trait("Category","Unit")]
        public void PaymentShouldHaveDescription()
        {
            // Given
            var expected = "Test";
            var sut = new PaymentResponse() { Description = expected };
            sut.Description = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected, result.Description);
        }
    }
}