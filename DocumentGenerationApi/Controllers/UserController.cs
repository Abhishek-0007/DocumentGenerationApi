using DocumentGenerationApi.Models;
using DocumentGenerationApi.Models.RequestViewModels;
using DocumentGenerationApi.Models.ResponseViewModels;
using DocumentGenerationApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;
using PuppeteerSharp;
using System.Diagnostics;

namespace DocumentGenerationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISpService _service;
        public UserController(IServiceProvider serviceProperty) 
        { 
            _service = serviceProperty.GetRequiredService<ISpService>();
        }
        [HttpPost]
        public async Task<string> AddUser(SpRequestModel requestModel)
        {
            try
            {
                await _service.ExecuteStoreProcedure(requestModel);
                return "Success";
            }
            catch(Exception ex) 
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public async Task<IActionResult> pdf()
        {
            string content = System.IO.File.ReadAllText("C:\\Users\\abhis\\Downloads\\htmlpage.html");
            //var bytes = await createPdf("\r\n    \r\n    \r\n\r\n\r\nHello {{Name}},\r\nYour policy detail is , Policy Number : {{PolicyNumber}}\r\n    \r\n  \r\n    Age\r\n    Salary\r\n    Occupation\r\n    ProductCode\r\n    PolicyExpiryDate \r\n  \r\n  \r\n    {{Age}}\r\n    {{Salary}}\r\n    {{Occupation}}\r\n    {{ProductCode}}\r\n    {{PolicyExpiryDate}}\r\n  \r\n    \r\n\r\n");
            //pdfsharp(bytes);
            var bytes = await createPdf(content);
            System.IO.File.WriteAllBytes(@"C:\Users\abhis\OneDrive\Desktop\projects\DocumentGenerationApi\PDFs\gh.pdf", bytes);
            return Ok();
        }

        private async Task<Byte[]> createPdf(string template)
        {
            var f = "test.pdf";

            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
            });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(template);
            return pdfsharp(await page.PdfDataAsync(), f);

            //return await page.PdfDataAsync();
        }

        private byte[] pdfsharp(Byte[] bytePDF, string filename)
        {

            //File.Copy(Path.Combine("C:\\Users\\abhis\\OneDrive\\Desktop\\projects\\DocumentGenerationApi\\PDFs", "gh.pdf"), Path.Combine(Directory.GetCurrentDirectory(), "HelloWorld_tempfile.pdf"), true);
            using MemoryStream stream = new MemoryStream(bytePDF);
            PdfDocument document = PdfReader.Open(stream, PdfDocumentOpenMode.Import); //You might not need  PdfDocumentOpenMode.Import
                                                                                       // do some modification on the document
            PdfSecuritySettings securitySettings = document.SecuritySettings;
            securitySettings.UserPassword = "user";
            securitySettings.OwnerPassword = "owner";

            securitySettings.PermitAccessibilityExtractContent = false;
            securitySettings.PermitAnnotations = false;
            securitySettings.PermitAssembleDocument = false;
            securitySettings.PermitExtractContent = false;
            securitySettings.PermitFormsFill = true;
            securitySettings.PermitFullQualityPrint = false;
            securitySettings.PermitModifyDocument = true;
            securitySettings.PermitPrint = false;

            Console.WriteLine(document.PageCount);
            var m = new MemoryStream();
            document.Save(m);
            return m.ToArray();
            //Process.Start(filename);
        }
    }
}
