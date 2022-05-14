using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SharedExpenses.Storage.Abstraction;
using SharedExpensesApi.Models;

namespace SharedExpensesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly ILogger<ExpensesController> logger;
    private readonly IExpensesService expensesService;
    private readonly IMapper mapper;

    public ExpensesController(ILogger<ExpensesController> logger, IExpensesService expensesService, IMapper mapper)
    {
        this.logger = logger;
        this.expensesService = expensesService;
        this.mapper = mapper;
    }

    /// <summary>
    /// Gets Expenses from specific expnese group by id.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    /// Get /Expenses/1
    /// </remarks>
    /// <param name="expenseGroupId"></param>
    /// <response code="200">Returns expenses</response>
    /// <response code="400">Error message</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetExpenses([FromQuery,Required,Range(1,int.MaxValue)]int expenseGroupId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.Select(x => x.Value));
        }
        try
        {
            var result = await expensesService.GetExpensesOrderedByPaymentDateAsync(expenseGroupId);
            var model = mapper.Map<IEnumerable<Models.ExpenseResponse>>(result);
            if (model != null)
            {
                return Ok(model);
            }

        } catch (Exception ex ) {
            logger.LogError(ex.Message);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Gets users from specific expnese group by id.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    /// Get /ExpenseGroupUsers/1
    /// </remarks>
    /// <param name="expenseGroupId"></param>
    /// <response code="200">Returns expense group users</response>
    /// <response code="400">Error message</response>
    [HttpGet("GetExpenseGroupUsers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetExpenseGroupUsers([FromQuery,Required,Range(1,int.MaxValue)]int expenseGroupId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.Select(x => x.Value));
        }
        try
        {
            var result = await expensesService.GetExpenseGroupUsersAsync(expenseGroupId);
            var model = mapper.Map<IEnumerable<Models.ExpenseGroupUsersResponse>>(result);
            if (model != null)
            {
                return Ok(model);
            }

        } catch (Exception ex ) {
            logger.LogError(ex.Message);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
    /// <summary>
    /// Adds an Expense to an existing user and expense group.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Expenses
    ///     {
    ///      "userId": 1,
    ///      "expenseGroupId": 1,
    ///      "expense": {
    ///         "payment": {
    ///           "date": "2022-05-11T11:50:33.639Z",
    ///           "amount": 123.321,
    ///           "description": "Some Description"
    ///         }
    ///       }
    ///     }
    ///
    /// </remarks>
    /// <param name="request"></param>
    /// <response code="200">Returns bolean value</response>
    /// <response code="400">Error message</response>
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddExpense([FromBody] AddExpnenseRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.Select(x => x.Value));
        }
        try
        {
            var expenseEntity =mapper.Map<SharedExpenses.Storage.Models.Expense>(request.Expense);
            var result = await expensesService.AddExpenseAsync(request.UserId, request.ExpenseGroupId, expenseEntity);
            if (result)
            {
                return Ok(result);
            }

        } catch (Exception ex ) {
            logger.LogError(ex.Message);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Adds an User to an ExpenseGroup and creates the ExpenseGroup if it dose not exist.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Expenses/AddUserToExpenseGroup
    ///     {
    ///        "user:  {
    ///          "id"1,
    ///          "fullName": "John Doe"
    ///        },
    ///        "expenseGroupId": "1"
    ///     }
    ///
    /// </remarks>
    /// <param name="request"></param>
    /// <response code="200">Returns bolean value</response>
    /// <response code="400">Error message</response>
    [HttpPost("/AddUserToExpenseGroup")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUserToExpenseGroupAsync([FromBody] AddUserToExpnenseGroupRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.Select(x => x.Value));
        }
        try
        {
            var result = await expensesService.AddUserToExpenseGroupAsync(request.UserId, request.ExpenseGroupId);
            if (result)
            {
                return Ok(result);
            }

        } catch (Exception ex ) {
            logger.LogError(ex.Message);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
    /// <summary>
    /// Gets balance and calculates for a specifig expense group by id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    /// Get /GetExpenseGroupBalance/1
    /// </remarks>
    /// <param name="expenseGroupId"></param>
    /// <response code="200">Returns balance</response>
    /// <response code="400">Error message</response>
    [HttpGet("GetExpenseGroupBalance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetExpenseGroupBalance([FromQuery,Required,Range(1,int.MaxValue)]int expenseGroupId)
    {
         if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.Select(x => x.Value));
        }
        try
        {
            var result = await expensesService.GetExpenseGroupBalanceAsync(expenseGroupId);
            var model = mapper.Map<BalanceSummaryResponse>(result);
            if (model != null)
            {
                return Ok(model);
            }

        } catch (Exception ex ) {
            logger.LogError(ex.Message);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}
