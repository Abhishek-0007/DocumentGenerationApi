using DocumentGenerationApi.Models.ResponseViewModels;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;

namespace DocumentGenerationApi.Services.Interfaces
{
    public interface IMailService
    {
        public Task<IActionResult> CreateMail(Byte[] pdf);
    }
}
