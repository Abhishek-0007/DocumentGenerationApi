using DocumentGenerationApi.DAL.Entity;
using DocumentGenerationApi.DAL.Repositories.Interfaces;
using DocumentGenerationApi.Models.RequestViewModels;
using DocumentGenerationApi.Services.Interfaces;
using PuppeteerSharp;
using System;
using System.Reflection;


namespace DocumentGenerationApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository; 
        public UserService(IServiceProvider serviceProvider)
        {
            _repository = serviceProvider.GetRequiredService<IUserRepository>();
        }

        public async Task Post(UserRequestModel userRequestModel)
        {
            var getUser = _repository.GetByPolicyNumber(userRequestModel).Result;
            Console.WriteLine(getUser.Name);
            var getDocument = _repository.GetContent("template-123").Result;

            if (getUser != null && getDocument.Content != null)
            {
                var propertyInfo = getUser.GetType().GetProperties();
                string newTemplate = FillInHtmlTemplate<User>(getDocument.Content, propertyInfo, getUser);

                var byteArray = await createPdf(newTemplate, getUser.Name);
                var docToSave = ConvertModelToSavedDoc(getUser, byteArray, getDocument);
                await _repository.MakeIsDeleteTrue(docToSave.ObjectCode);
                await SaveDocumentInDB(docToSave);
                if (docToSave != null)
                {
                    
                    //if(_repository.DocExistOrNot(docToSave.ObjectCode).Result)
                    //{
                    //    await createPdf(newTemplate, getUser.Name);
                    //}
                }
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
            await page.PdfAsync(Path.Combine(@"C:\Users\007bc\Desktop\pdf_generated", $"{templateName}gh.pdf"));

            return page.PdfDataAsync().Result;
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
