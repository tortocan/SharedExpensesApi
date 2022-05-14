using Xunit;
using SharedExpenses.Storage.Abstraction;
using NSubstitute;
using Bogus;
using SharedExpenses.Storage.Models;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedExpenses.Storage.Tests;

public class ExpensesServiceTests
{
    // TODO: ○ El listado debe estar ordenado por el último pago realizado.
    [Fact]
    [Trait("Category","Unit")]
    public async Task ExpensesShouldBeOrderedByPaymentDate()
    {
        // Given
        var faker = new Faker<Expense>();
        var childFaker = new Faker<Payment>();
        childFaker.RuleFor(x => x.Date, x => x.Date.Recent(x.UniqueIndex));
        var payments = childFaker.Generate(100);
        // faker.RuleFor(x => x.Id, f => f.UniqueIndex);
        faker.RuleFor(x => x.Payment, f => payments[f.Random.Int(1, 99)]).Generate();
        var expected = faker.Generate(10).ToList().OrderByDescending(x => x.Payment.Date);
        var sut = Substitute.For<IExpensesService>();
        sut.GetExpensesOrderedByPaymentDateAsync(Arg.Any<int>()).Returns(expected);
        // When
        var result = await sut.GetExpensesOrderedByPaymentDateAsync(1);
        // Then
        result.Should().BeAssignableTo<IEnumerable<Expense>>()
        .And.NotBeEmpty().And.HaveCount(10)
        .And.BeInDescendingOrder(x => x.Payment.Date);
    }

    //Como usuario, quiero añadir una persona a mi grupo de amigos.
    [Fact]
    [Trait("Category","Unit")]
    public async Task RegisterAnUserToExpense()
    {
        // Given
        var expected = true;
        var sut = Substitute.For<IExpensesService>();
        sut.AddUserToExpenseGroupAsync(Arg.Any<int>(),Arg.Any<int>()).Returns(expected);
        // When
        var result = await sut.AddUserToExpenseGroupAsync(1,1);
        // Then
        result.Should().BeTrue();
    }

}