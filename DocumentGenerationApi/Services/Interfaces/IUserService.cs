using DocumentGenerationApi.DAL.Entity;
using DocumentGenerationApi.Models.RequestViewModels;
using DocumentGenerationApi.Models.ResponseViewModels;

namespace DocumentGenerationApi.Services.Interfaces
{
    public interface IUserService
    {
        public Task Post(UserRequestModel userRequestModel);

    }
}
