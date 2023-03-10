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

        public async Task ExecuteStoreProcedure(SpRequestModel requestModel)
        {
            //await using (var conn = new SqlConnection(_config.GetConnectionString("ConnString")))
            //{
            //    await using (var comm = conn.CreateCommand())
            //    {
            //        sqlCon.Open();
            //        //comm.CommandText = $"exec sp_generate_refund_policy '{requestModel.PolicyNumber}','{requestModel.ProductCode}', '{requestModel.TemplateCode}'";
            //        //comm.ExecuteNonQuery();
            //        //var output = comm.ExecuteReader().ToString();

            //        //if(output is null) { throw new Exception(); }

            //        SqlCommand Cmnd = new SqlCommand("PROC_NAME", sqlCon);
            //        Cmnd.CommandType = CommandType.StoredProcedure;
            //        Cmnd.Parameters.AddWithValue("@ID", SqlDbType.Int).Value = Id;
            //        Cmnd.Parameters.AddWithValue("@NAME", SqlDbType.NVarChar).Value = Name;
            //        int result = Cmnd.ExecuteNonQuery();
            //        sqlCon.Close();
            //    }
            //}
            SqlConnection sqlCon = null;
            String SqlconString = _config.GetConnectionString("ConnString");
            using (sqlCon = new SqlConnection(SqlconString))
            {
                sqlCon.Open();
                SqlCommand Cmnd = new SqlCommand("sp_generate_refund_policy", sqlCon);
                Cmnd.CommandType = CommandType.StoredProcedure;
                Cmnd.Parameters.AddWithValue("@policy_number", SqlDbType.NVarChar).Value = requestModel.PolicyNumber;
                Cmnd.Parameters.AddWithValue("@product_code", SqlDbType.NVarChar).Value = requestModel.ProductCode;
                Cmnd.Parameters.AddWithValue("@template_code", SqlDbType.NVarChar).Value = requestModel.TemplateCode;
                var result = Cmnd.ExecuteReader();
                while (result.Read())
                {
                    string name = result[0].ToString();
                }
                sqlCon.Close();
            }



            var str = _repo.GetAll().Result.ToList().FirstOrDefault().Body;

            if (str is null) 
            {
                throw new Exception(); 
            }
            else
            {
                var item = await createPdf(str);
                await _mailService.CreateMail(item);
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
