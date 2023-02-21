using DocumentGenerationApi.DAL.DbContexts;
using DocumentGenerationApi.DAL.Entity;
using DocumentGenerationApi.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentGenerationApi.DAL.Repositories.Implementations
{
    public class DocumentRepository : IDocumentRepository
    {
        private ApplicationDbContext _context;
        public DocumentRepository(IServiceProvider serviceProvider) 
        {
            _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        }
        public async Task<List<Document>> GetAllDocumentsAsync()
        {
            var list = await _context.Documents.ToListAsync();
            return list;
        }

        public async Task<Document> GetDocumentById(int id)
        {
            var doc = await _context.Documents.Where(t => t.Id.Equals(id)).FirstOrDefaultAsync();
            return doc;

        }
        public async Task<Document> GetContent(string docCode)
        {
            var template = await _context.Documents.Where(t => t.DocumentCode.Equals(docCode)).FirstOrDefaultAsync();

            return template;
        }
    }
}
