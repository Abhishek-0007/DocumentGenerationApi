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
            await _context.SaveChangesAsync();
            return list;
        }

        public async Task<Document> GetDocumentById(int id)
        {
            var doc = _context.Documents.Where(t => t.Id.Equals(id)).FirstOrDefault();
            await _context.SaveChangesAsync();
            return doc;

        }

        public Task<Document> PostDocumentItem(Document document)
        {
            throw new NotImplementedException();
        }

        public async Task<Document> GetContent(string docCode)
        {
            var items = await _context.Documents.ToListAsync();

            var template = items.Where(t => t.DocumentCode.Equals(docCode)).FirstOrDefault();

            return template;
        }
    }
}
