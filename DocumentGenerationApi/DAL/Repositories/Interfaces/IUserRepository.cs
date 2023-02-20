using DocumentGenerationApi.DAL.Entity;
using DocumentGenerationApi.Models.RequestViewModels;

namespace DocumentGenerationApi.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task AddAsync (User user); 
        public Task<User> GetByPolicyNumber(UserRequestModel userRequestModel);
        Task<List<Document>> GetAllDocumentsAsync();
        public Task SaveDocInDBAsync(SaveDocument saveDocument);
        public Task MakeIsDeleteTrue(string ObjectCode);
        public Task<bool> DocExistOrNot(string ObjectCode);
        public Task<Document> GetContent(string docCode);

    }
}
