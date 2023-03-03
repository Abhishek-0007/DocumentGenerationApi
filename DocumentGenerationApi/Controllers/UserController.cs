using DocumentGenerationApi.Models.RequestViewModels;
using DocumentGenerationApi.Services.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace DocumentGenerationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IRecurringJobManager _recurringJobManager;
        public UserController(IServiceProvider serviceProperty) 
        { 
            _service = serviceProperty.GetRequiredService<IUserService>();
            _recurringJobManager = serviceProperty.GetRequiredService<IRecurringJobManager>();
        }

        [HttpPost]
        [AutomaticRetry(Attempts = 5)]
        public async Task<string> AddUser(UserRequestModel userRequestModel)
        {
             _recurringJobManager.AddOrUpdate(
                "Job To Be Executed every Monday at 8 AM",
                () =>  _service.Post(userRequestModel),
                "0 8 * * 1");

            return _recurringJobManager.ToString();

        }
    }
}
