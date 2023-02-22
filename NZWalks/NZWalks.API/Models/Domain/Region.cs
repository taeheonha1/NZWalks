namespace NZWalks.API.Models.Domain
{
    public class Region
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Population { get; set; }

        //Naviation Property
        //[1] Every Region is related to Walks.-- One Region can have multiple Walks.
        public IEnumerable<Walk> Walks { get; set; }

    }
}
