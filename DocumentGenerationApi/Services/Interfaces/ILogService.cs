using DocumentGenerationApi.DAL.Entity;
using DocumentGenerationApi.Models.ResponseViewModels;

namespace DocumentGenerationApi.Services.Interfaces
{
    public interface ILogService
    {
        public Task<LogModel> WriteLogAsync(string email);

        public LogModel WriteLogException(string message);
    }

}
