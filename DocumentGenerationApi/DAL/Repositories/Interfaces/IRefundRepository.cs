using DocumentGenerationApi.DAL.Entity;

namespace DocumentGenerationApi.DAL.Repositories.Interfaces
{
    public interface IRefundRepository
    {
        public Task<IEnumerable<RefundPolicy>> GetAll();
    }
}
