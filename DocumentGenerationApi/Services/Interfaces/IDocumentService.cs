using DocumentGenerationApi.Models.RequestViewModels;
using DocumentGenerationApi.Models.ResponseViewModels;

namespace DocumentGenerationApi.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<List<DocumentResponseModel>> GetAllDocs();

        DocumentResponseModel GetDocumentById(int id);

    }
}
