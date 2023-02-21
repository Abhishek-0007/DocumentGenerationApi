using DocumentGenerationApi.DAL.Entity;

namespace DocumentGenerationApi.DAL.Repositories.Interfaces
{
    public interface IDocumentRepository
    {
        Task<List<Document>> GetAllDocumentsAsync();
        Task<Document> GetDocumentById(int id);
        public Task<Document> GetContent(string docCode);
    }
}
