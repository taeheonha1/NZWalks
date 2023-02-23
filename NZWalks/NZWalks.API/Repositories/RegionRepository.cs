using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Security.AccessControl;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            _nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
        }

        //Task<Region> AddSync(Region region);
        public async Task<Region> AddSync(Region region)
        {
            region.Id = Guid.NewGuid();
            await _nZWalksDbContext.AddAsync(region);
            await _nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsyn(Guid id)
        {
            // search Region, using id
            var region = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(region == null)
            {
                return null; 
            }
            // Delete the region from DB
            _nZWalksDbContext.Regions.Remove(region);
            await _nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> UpdateAsync(Guid id, Region requestedRegion)
        {
            // search Region, using id
            var existingRegion = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            {
                return null;
            }

            // region is a part of our Request
            existingRegion.Code = requestedRegion.Code;
            existingRegion.Name = requestedRegion.Name;
            existingRegion.Area = requestedRegion.Area;
            existingRegion.Latitude = requestedRegion.Latitude;
            existingRegion.Longitude = requestedRegion.Longitude;
            existingRegion.Population = requestedRegion.Population;

            await _nZWalksDbContext.SaveChangesAsync();

            return existingRegion;
                
        }
    }
}
