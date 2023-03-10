using DocumentGenerationApi.Models.RequestViewModels;
using DocumentGenerationApi.Models.ResponseViewModels;
using DocumentGenerationApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DocumentGenerationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISpService _service;
        public UserController(IServiceProvider serviceProperty) 
        { 
            _service = serviceProperty.GetRequiredService<ISpService>();
        }
        [HttpGet]
        public async Task AddUser()
        {
            await _service.ExecuteStoreProcedure();
        }
    }
}
