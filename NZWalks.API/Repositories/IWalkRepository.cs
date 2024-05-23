using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsyn(Walk walk);
        Task<List<Walk>> GetAsync(); 
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> UpdatebyIdAsync(Guid id,Walk walkDomainModel);
        Task<Walk?> DeleteAsync(Guid id);
    }
}
