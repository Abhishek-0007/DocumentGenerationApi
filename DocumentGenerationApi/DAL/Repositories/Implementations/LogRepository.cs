using DocumentGenerationApi.DAL.DbContexts;
using DocumentGenerationApi.DAL.Entity;
using DocumentGenerationApi.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocumentGenerationApi.DAL.Repositories.Implementations
{
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _context;
        public LogRepository(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        }

        public async Task AddLogAsync(CreateLogEntry createLog)
        {
            await _context.AddAsync<CreateLogEntry>(createLog);
            await _context.SaveChangesAsync();
        }

        public async Task<string> CheckIfEmailSent(string email)
        {
            var list = await _context.Logs.ToListAsync();
            var item = list.Where(t => t.Email.Equals(email)).FirstOrDefault();

            if (item != null && item.isSent == true)
            {
                return "Email aleady sent";
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
