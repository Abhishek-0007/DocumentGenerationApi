using DocumentGenerationApi.DAL.Repositories.Interfaces;
using DocumentGenerationApi.Models;
using DocumentGenerationApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PuppeteerSharp;
using System.Data;

namespace DocumentGenerationApi.Services.Implementations
{
    public class SpService : ISpService
    {
        private readonly IConfiguration _config;
        private readonly IRefundRepository _repo;
        private readonly IMailService _mailService;
        public SpService(IServiceProvider serviceProvider)
        {
            _config = serviceProvider.GetRequiredService<IConfiguration>(); 
            _repo = serviceProvider.GetRequiredService<IRefundRepository>();    
            _mailService = serviceProvider.GetRequiredService<IMailService>();
        }

        public async Task<IActionResult> ExecuteStoreProcedure(SpRequestModel requestModel)
        {
            try
            {
                await using (var conn = new SqlConnection(_config.GetConnectionString("ConnString")))
                {
                    await using (var comm = conn.CreateCommand())
                    {
                        conn.Open();
                        comm.CommandText = $"exec sp_generate_refund_policy '{requestModel.PolicyNumber}','{requestModel.ProductCode}', '{requestModel.TemplateCode}'";
                        comm.ExecuteNonQuery();
                    }
                }

                var str = _repo.GetAll().Result.FirstOrDefault().Body;
                var item = await createPdf(str);
                return await _mailService.CreateMail(item);

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Stored Procedure is not running");
            }
           

        }

        private async Task<Byte[]> createPdf(string template)
        {
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
            });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(template);
            //await page.PdfAsync(Path.Combine(@"C:\Users\007bc\Desktop\pdf_generated", $"{templateName}gh.pdf"));

            return await page.PdfDataAsync();
        }
    }
}
