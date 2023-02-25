using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;


namespace NZWalks.API.Repositories
{
    public class WalkDifficultyDepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nzWalkDbContext;

        public WalkDifficultyDepository(NZWalksDbContext nzWalkDbContext)
        {
            this.nzWalkDbContext = nzWalkDbContext;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nzWalkDbContext.WalkDifficulties.ToListAsync();
        }

        public async Task<WalkDifficulty> GetSync(Guid id)
        {
            return await nzWalkDbContext.WalkDifficulties.FindAsync(id);
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            // Asssign a new Id because wa are not take from our ID from our client
            walkDifficulty.Id = Guid.NewGuid();
            await nzWalkDbContext.AddAsync(walkDifficulty);
            await nzWalkDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            // search Walk, using id
            var existingWalkDifficuty = await nzWalkDbContext.WalkDifficulties.FindAsync(id);

            if (existingWalkDifficuty == null)
            {
                return null;
            }

            existingWalkDifficuty.Code = walkDifficulty.Code;
            await nzWalkDbContext.SaveChangesAsync();

            return existingWalkDifficuty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            // search Region, using id
            //var existingWalk = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            var existingWalkDifficulty = await nzWalkDbContext.WalkDifficulties.FindAsync(id);
            if (existingWalkDifficulty == null)
            {
                return null;
            }
            // Delete the walk from DB
            nzWalkDbContext.WalkDifficulties.Remove(existingWalkDifficulty);
            await nzWalkDbContext.SaveChangesAsync();
            return existingWalkDifficulty;
        }
    }
}
