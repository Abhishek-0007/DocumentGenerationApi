using DocumentGenerationApi.Models;
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
        [HttpPost]
        public async Task<string> AddUser(SpRequestModel requestModel)
        {
            try
            {
                await _service.ExecuteStoreProcedure(requestModel);
                return "Success";
            }
            catch(Exception ex) 
            {
                return ex.Message;
            }
        }
    }
}
