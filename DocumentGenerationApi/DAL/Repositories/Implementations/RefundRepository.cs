using DocumentGenerationApi.DAL.DbContexts;
using DocumentGenerationApi.DAL.Entity;
using DocumentGenerationApi.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentGenerationApi.DAL.Repositories.Implementations
{
    public class RefundRepository : IRefundRepository
    {
        private readonly ApplicationDbContext _context;
        public RefundRepository(IServiceProvider serviceProvider) 
        {
            _context = serviceProvider.GetRequiredService<ApplicationDbContext>();  
        }

        public async Task<IEnumerable<RefundPolicy>> GetAll()
        {
            return await _context.Refunds.ToListAsync();
        }
    }
}
