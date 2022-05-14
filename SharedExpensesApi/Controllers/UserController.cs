using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SharedExpenses.Storage.Abstraction;

namespace SharedExpensesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper)
        {
            this.logger = logger;
            this.userService = userService;
            this.mapper = mapper;
        }
        /// <summary>
        /// Gets all application users.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// Get /User/
        /// </remarks>
        /// <param name="expenseGroupId"></param>
        /// <response code="200">Returns users</response>
        /// <response code="400">Error message</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Select(x => x.Value));
            }
            try
            {
                var result = await userService.GetUserAsync();
                var model = mapper.Map<IEnumerable<Models.ApplicationUserResponse>>(result);
                if (model != null)
                {
                    return Ok(model);
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}