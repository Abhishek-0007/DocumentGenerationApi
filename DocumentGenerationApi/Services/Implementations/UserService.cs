using DocumentGenerationApi.DAL.Entity;
using DocumentGenerationApi.DAL.Repositories.Interfaces;
using DocumentGenerationApi.Models.RequestViewModels;
using DocumentGenerationApi.Models.ResponseViewModels;
using DocumentGenerationApi.Services.Interfaces;
using PuppeteerSharp;
using System.Reflection;


namespace DocumentGenerationApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMailService _mailService;
        private readonly ILogService _logService;
        public UserService(IServiceProvider serviceProvider)
        {
            _repository = serviceProvider.GetRequiredService<IUserRepository>();
            _mailService = serviceProvider.GetRequiredService<IMailService>();
            _logService = serviceProvider.GetRequiredService<ILogService>();
        }

        public async Task<LogModel> Post(UserRequestModel userRequestModel)
        {
            var getUser = await _repository.GetByPolicyNumber(userRequestModel);
            var getDocument = await _repository.GetContent("template");

            if (getUser != null && getDocument.Content != null)
            {
                var propertyInfo = getUser.GetType().GetProperties();
                string newTemplate = FillInHtmlTemplate<User>(getDocument.Content, propertyInfo, getUser);
                var byteArray = await createPdf(newTemplate, getUser.Name);
                var docToSave = ConvertModelToSavedDoc(getUser, byteArray, getDocument);
                await _repository.MakeIsDeleteTrue(docToSave.ObjectCode);
                await SaveDocumentInDB(docToSave);

                //send mail
                return await _mailService.CreateMail(byteArray);
            }
            else
            {
               return _logService.WriteLogException("Data Not Found!");
            }
            
        }
        private SaveDocument ConvertModelToSavedDoc(User getUser, Byte[] content, Document getDocument)
        {
            SaveDocument savedDoc = new SaveDocument();
            savedDoc.ObjectCode = $"{getUser.PolicyNumber} - {getUser.ProductCode}";
            savedDoc.ReferenceType = "Policy";
            savedDoc.ReferenceNumber = $"{getUser.PolicyNumber}";
            savedDoc.Content = content;
            savedDoc.FileName = $"{getDocument.Filename}";
            savedDoc.FileExtension = ".pdf";
            savedDoc.LanguageCode = "km-KH";
            savedDoc.CreatedUser = $"{getDocument.CreatedUser}";
            savedDoc.CreatedDateTime = DateTime.Now;
            savedDoc.isDeleted = false;

            return savedDoc;
        }

        private async Task<Byte[]> createPdf(string template, string templateName)
        {
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(template);
            //await page.PdfAsync(Path.Combine(@"C:\Users\007bc\Desktop\pdf_generated", $"{templateName}gh.pdf"));

            return await page.PdfDataAsync();
    }

        private string FillInHtmlTemplate<TEntity>(string template, PropertyInfo[] propertyArray, TEntity obj)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            foreach (var property in propertyArray)
            { 
                map.Add(property.Name, property.GetValue(obj as User).ToString());
            }
            for(int i=0; i<template.Length; i++)
            {
                char temp = template[i];
                string tempString = "";

                int currentNameLength = 0;
                if(temp == '{')
                {
                    for(int j=i+2; j<template.Length; j++)
                    {
                        if (template[j] == '}') { break; }
                        else
                        {
                            tempString += (template[j]);
                            currentNameLength++;
                        }
                    }
                    var value = map.GetValueOrDefault(tempString);
                    var replacedStr = "{{" + tempString + "}}";
                    Console.WriteLine($"Key: {tempString} - Value : {value}");
                    template = template.Replace(replacedStr, value);
                    i += currentNameLength;
                    tempString = "";
                }
            }
            return template;
        }

        private async Task SaveDocumentInDB(SaveDocument saveDocument)
        {
            await _repository.SaveDocInDBAsync(saveDocument);
        }
    }
}
