using DocumentGenerationApi.DAL.Entity;

namespace DocumentGenerationApi.DAL.Repositories.Interfaces
{
    public interface ILogRepository
    {
        public Task<string> CheckIfEmailSent(string email);

        public Task AddLogAsync(CreateLogEntry createLog);
    }
}
