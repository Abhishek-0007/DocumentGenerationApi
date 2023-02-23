using DocumentGenerationApi.Models.RequestViewModels;
using DocumentGenerationApi.Models.ResponseViewModels;
using DocumentGenerationApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DocumentGenerationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IServiceProvider serviceProperty) 
        { 
            _service = serviceProperty.GetRequiredService<IUserService>();
        }

        [HttpPost]
        public async Task AddUserAsync(UserRequestModel userRequestModel)
        {
           await _service.Post(userRequestModel);
        }
    }
}
