using SharedExpensesApi.Models;
using Xunit;
using System;

namespace SharedExpenses.Tests
{
    //Como usuario, quiero acceder al grupo de gastos compartidos de mi grupo de amigos. Para cada gasto quiero visualizar la siguiente información:
    public class ExpensesTest : UnitTest
    {
        // TODO: ○ El listado debe estar ordenado por el último pago realizado.
        [Fact]
        public void ExpencesShouldExist()
        {
            // Given
            var sut = new Expences();
            // When
            var result = sut;
            // Then
            Assert.NotNull(result);
        }

        [Fact]
        // ○ Fecha del pago
        public void ExpencesShouldHaveAPaymentIdAndDate()
        {
            // Given
            var expected = new Payment() { Id = 1, Date = DateTime.UtcNow };
            var sut = new Expences();
            sut.Payment = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected.Id, result.Payment.Id);
            Assert.Equal(expected.Date, result.Payment.Date);
        }

        [Fact]
        // ○ Importe del pago
        public void ExpencesShouldHaveAPaymentAmount()
        {
            // Given
            var expected = new Payment() { Id = 1, Date = DateTime.UtcNow, Amount = 123.321 };
            var sut = new Expences();
            sut.Payment = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected.Amount, result.Payment.Amount);
        }

        [Fact]
        // ○ Descripción del pago
        public void ExpencesShouldHaveAPaymentDescription()
        {
            // Given
            var expected = new Payment() { Id = 1, Date = DateTime.UtcNow, Amount = 123.321, Description = "Test" };
            var sut = new Expences();
            sut.Payment = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected.Description, result.Payment.Description);
        }

        [Fact]
        // ○ Persona que realizó el pago
        public void ExpencesShouldHaveAUserId()
        {
            // Given
            var expected = new ApplicationUser() { Id = 1, FullName = "TestUser" };
            var sut = new Expences();
            sut.User = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected.Id, result.User.Id);
            Assert.Equal(expected.FullName, result.User.FullName);
        }
    }
}