using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();

        Task<Region> GetAsync(Guid id);

        Task<Region> AddSync(Region region);

        Task<Region> DeleteAsyn(Guid id);

        Task<Region> UpdateAsync(Guid id, Region region);
    }
}
