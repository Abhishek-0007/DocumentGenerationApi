using DocumentGenerationApi.Models.ResponseViewModels;
using DocumentGenerationApi.Services.Implementations;
using DocumentGenerationApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DocumentGenerationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _service;
        public DocumentController(IServiceProvider service) 
        {
            _service = service.GetRequiredService<IDocumentService>();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentResponseModel>>> GetAllDocuments()
        {
            return _service.GetAllDocs().Result;
        }
        private bool DocumentItemExists(int id)
        {
            var listItems = _service.GetAllDocs().Result;
            return listItems.Any(e => e.Id == id);
        }
    }
}
