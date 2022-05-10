using SharedExpensesApi.Models;
using Xunit;
using System;

namespace SharedExpenses.Tests
{
    public class PaymentTests : UnitTest
    {
        [Fact]
        public void PaymentShouldHaveDate()
        {
            // Given
            var expected = DateTime.UtcNow;
            var sut = new Payment() { Date = expected };
            sut.Date = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected, result.Date);
        }

        [Fact]
        public void PaymentShouldHaveAmount()
        {
            // Given
            var expected = 321.123;
            var sut = new Payment() { Amount = expected };
            sut.Amount = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected, result.Amount);
        }

        [Fact]
        public void PaymentShouldHaveDescription()
        {
            // Given
            var expected = "Test";
            var sut = new Payment() { Description = expected };
            sut.Description = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected, result.Description);
        }
    }
}