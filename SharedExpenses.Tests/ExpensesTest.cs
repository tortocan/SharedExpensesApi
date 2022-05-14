using SharedExpensesApi.Models;
using Xunit;
using System;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using SharedExpensesApi.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace SharedExpenses.Tests
{
    //Como usuario, quiero acceder al grupo de gastos compartidos de mi grupo de amigos. Para cada gasto quiero visualizar la siguiente información:
    public class ExpensesTest : UnitTest
    {
        private WebApplicationFactory<Program> application =>
        new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                // ... Configure test services
                builder.ConfigureServices(s =>
                {
                    //s.AddControllersAsServices();
                });
            });
        [Fact]
        [Trait("Category", "Unit")]
        public void ExpensesShouldExist()
        {
            // Given
            var sut = new ExpenseResponse();
            // When
            var result = sut;
            // Then
            Assert.NotNull(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        // ○ Fecha del pago
        public void ExpensesShouldHaveAPaymentIdAndDate()
        {
            // Given
            var expected = new PaymentResponse() { Id = 1, Date = DateTime.UtcNow };
            var sut = new ExpenseResponse();
            sut.Payment = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected.Id, result.Payment.Id);
            Assert.Equal(expected.Date, result.Payment.Date);
        }

        [Fact]
        [Trait("Category", "Unit")]
        // ○ Importe del pago
        public void ExpensesShouldHaveAPaymentAmount()
        {
            // Given
            var expected = new PaymentResponse() { Id = 1, Date = DateTime.UtcNow, Amount = 123.321 };
            var sut = new ExpenseResponse();
            sut.Payment = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected.Amount, result.Payment.Amount);
        }

        [Fact]
        [Trait("Category", "Unit")]
        // ○ Descripción del pago
        public void ExpensesShouldHaveAPaymentDescription()
        {
            // Given
            var expected = new PaymentResponse() { Id = 1, Date = DateTime.UtcNow, Amount = 123.321, Description = "Test" };
            var sut = new ExpenseResponse();
            sut.Payment = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected.Description, result.Payment.Description);
        }

        [Fact]
        [Trait("Category", "Unit")]
        // ○ Persona que realizó el pago
        public void ExpensesShouldHaveAUserId()
        {
            // Given
            var expected = new ApplicationUserResponse() { Id = 1, FullName = "TestUser" };
            var sut = new ExpenseResponse();
            sut.User = expected;
            // When
            var result = sut;
            // Then
            Assert.Equal(expected.Id, result.User.Id);
            Assert.Equal(expected.FullName, result.User.FullName);
        }
        // ○ El listado debe estar ordenado por el último pago realizado.
        [Fact]
        [Trait("Category", "Integration")]
        public async Task ExpensesShouldBeOrderedByPaymentDate()
        {
            // Given
            using (var serviceScope = application.Services.CreateScope())
            {
                var sut = serviceScope.ServiceProvider.GetService<ExpensesController>();
                // When
                var result = await sut.GetExpenses(1); ;
                // Then
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<ExpenseResponse>>(okResult.Value);
                returnValue.Should().BeAssignableTo<IEnumerable<ExpenseResponse>>()
                .And.NotBeEmpty().And.HaveCountGreaterThan(2);
                returnValue.Select(x => x.Payment.Date).Should().NotBeInAscendingOrder().And.
                NotBeInAscendingOrder();
            }

        }

        // Como usuario, quiero añadir una persona a mi grupo de amigos.
        [Fact]
        [Trait("Category", "Integration")]
        public async Task RegisterAnUserToExpense()
        {
            // Given
            var request = new AddUserToExpnenseGroupRequest
            {
                UserId = 1,
                ExpenseGroupId = 3
            };
            using (var serviceScope = application.Services.CreateScope())
            {
                var sut = serviceScope.ServiceProvider.GetService<ExpensesController>();
                // When
                var result = await sut.AddUserToExpenseGroupAsync(request); ;
                // Then
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<bool>(okResult.Value);
                returnValue.Should().BeTrue();
            }

        }

        // Como usuario, quiero añadir un pago
        [Fact]
        [Trait("Category", "Integration")]
        public async Task RegisterExpense()
        {
            // Given
            var request = new AddExpnenseRequest
            {
                UserId = 1,
                ExpenseGroupId = 3
            };
            request.Expense = new ExpenseRequest
            {
                Payment = new Faker<PaymentRequest>()
                .RuleFor(x => x.Amount, x => x.Random.Double(1, 100))
                .RuleFor(x => x.Description, x => x.Commerce.ProductDescription())
                .Generate()
            };
            using (var serviceScope = application.Services.CreateScope())
            {
                var sut = serviceScope.ServiceProvider.GetService<ExpensesController>();
                // When
                var result = await sut.AddExpense(request); ;
                // Then
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<bool>(okResult.Value);
                returnValue.Should().BeTrue();
            }

        }
        //Como usuario, quiero un balance para saber cuánto dinero tiene que pagar o recibir cada persona del grupo para que todos hubiéramos pagado lo mismo y no tener deudas entre el grupo de amigos.
        [Fact]
        [Trait("Category", "Integration")]
        public async Task GeExpenseGroupBalance()
        {
            // Given
            var expenseGroupId = 1;
            using (var serviceScope = application.Services.CreateScope())
            {
                var sut = serviceScope.ServiceProvider.GetService<ExpensesController>();
                // When
                var result = await sut.GetExpenseGroupBalance(expenseGroupId); ;
                // Then
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<BalanceSummaryResponse>(okResult.Value);
                returnValue.Balance.Should().NotBeEmpty();
                returnValue.BalanceDue.Should().NotBeEmpty();
                // returnValue[0].Balance.Amount.Should().Be(59.15);
                // returnValue[1].Balance.Amount.Should().Be(22.55);
                // returnValue[2].Balance.Amount.Should().Be(-40.85);
                // returnValue[3].Balance.Amount.Should().Be(-40.85);
            }

        }
    }
}