using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalkDbContext;

        public WalkRepository(NZWalksDbContext nZWalkDbContext)
        {
            this.nZWalkDbContext = nZWalkDbContext;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await nZWalkDbContext.Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await nZWalkDbContext.Walks
               .Include(x => x.Region)
               .Include(x => x.WalkDifficulty)
               .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            // Asssign a new Id because wa are not take from our ID from our client
            walk.Id = Guid.NewGuid();
            await nZWalkDbContext.Walks.AddAsync(walk);
            await nZWalkDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            // search Walk, using id
            var existingWalk = await nZWalkDbContext.Walks.FindAsync(id);

            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Length = walk.Length;
            existingWalk.Name = walk.Name;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await nZWalkDbContext.SaveChangesAsync();

            return existingWalk;

        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            // search Region, using id
            //var existingWalk = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            var existingWalk = await nZWalkDbContext.Walks.FindAsync(id);
            if (existingWalk == null)
            {
                return null;
            }
            // Delete the walk from DB
            nZWalkDbContext.Walks.Remove(existingWalk);
            await nZWalkDbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
