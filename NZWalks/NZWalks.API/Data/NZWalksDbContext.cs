using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext: DbContext
    {
        // Inject our dependency into the services
        // Dependency injection is a software design pattern. It is a techniqe for achieving inversion of contorl
        // between classes and their dependencies.
        // In this pattern, the dependencies of a class are injected into them rather then the class directly
        //Inject NZWalkDB into the service
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options): base(options)
        {

        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulties { get; set; }

    }
}
