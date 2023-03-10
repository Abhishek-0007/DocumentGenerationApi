using DocumentGenerationApi.Models.ResponseViewModels;
using PuppeteerSharp;

namespace DocumentGenerationApi.Services.Interfaces
{
    public interface IMailService
    {
        public Task CreateMail(Byte[] pdf);
    }
}
