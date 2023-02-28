using DocumentGenerationApi.DAL.Repositories.Interfaces;
using DocumentGenerationApi.Services.Interfaces;
using DocumentGenerationApi.DAL.Entity;
using DocumentGenerationApi.Models.ResponseViewModels;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace DocumentGenerationApi.Services.Implementations
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _repository;
        public LogService(IServiceProvider serviceProvider)
        {
            _repository = serviceProvider.GetRequiredService<ILogRepository>();
        }
        public async Task<LogModel> WriteLogAsync(string email)
        {
           var response = await HandleLog(email);
            LogModel model = new LogModel();
            model.TimeStamp = DateTime.Now;
            

            if (response.IsNullOrEmpty())
            {
                model.Code = "200";
                model.Message = "Success";
            }
            else
            {
                model.Code = "500";
                model.Message = response;
            }
            var json = JsonConvert.SerializeObject(model);
            Console.Write(json);

            if (response.IsNullOrEmpty())
            {
                return model;
            }
            else
            {
                return model;
            }

        }

        public LogModel WriteLogException(string message)
        {
            var model = new LogModel()
            {
                TimeStamp = DateTime.Now,
                Message = message,
                Code = "500"
            };

            return model;
        }

        private async Task<string> HandleLog(string Email)
        {
            var check = await _repository.CheckIfEmailSent(Email);

            if(check.IsNullOrEmpty())
            {
                await _repository.AddLogAsync(new CreateLogEntry() { Email = Email, isSent = true });
                return check;
            }
            else
            {
                return check;
            }

        }
    }
}
