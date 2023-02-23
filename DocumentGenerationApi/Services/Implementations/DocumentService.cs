using DocumentGenerationApi.DAL.Entity;
using DocumentGenerationApi.DAL.Repositories.Implementations;
using DocumentGenerationApi.DAL.Repositories.Interfaces;
using DocumentGenerationApi.Extensions;
using DocumentGenerationApi.Models.RequestViewModels;
using DocumentGenerationApi.Models.ResponseViewModels;
using DocumentGenerationApi.Services.Interfaces;

namespace DocumentGenerationApi.Services.Implementations
{
    public class DocumentService : IDocumentService
    {
        private IDocumentRepository _repository;
        public DocumentService(IServiceProvider serviceProvider)
        {
            _repository = serviceProvider.GetRequiredService<IDocumentRepository>();
        }
        public async Task<List<DocumentResponseModel>> GetAllDocs()
        {
           var list = await _repository.GetAllDocumentsAsync();
           List<DocumentResponseModel> responseList = new List<DocumentResponseModel>();

            foreach (var item in list)
            {
                var document = item.PropertyValueExtensionMethod<Document, DocumentResponseModel>() as DocumentResponseModel;
                responseList.Add(document);
            }

            return responseList;
        }

        public async DocumentResponseModel GetDocumentById(int id)
        {
            var list = await _repository.GetAllDocumentsAsync();
            var doc = list.Where(t => t.Id.Equals(id)).FirstOrDefault().
                PropertyValueExtensionMethod<Document, DocumentResponseModel>() as DocumentResponseModel;
            return doc;
        }
    }
}
