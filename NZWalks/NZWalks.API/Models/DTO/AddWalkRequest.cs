namespace NZWalks.API.Models.DTO
{
    public class AddWalkRequest
    {
        // Id get assigned by repository - db
        // We do not trust the client. They do not give us a unique ID
        //public Guid Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }


        // No Navigation Property --> They do not belong to walk. They are only connected to walk
        //public Region Region { get; set; }
        //public WalkDifficulty WalkDifficulty { get; set; }
    }
}
