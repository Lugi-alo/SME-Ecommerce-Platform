namespace Dissertation.Models
{
    public class ServiceListingFeature
    {
        public int ServiceListingsId { get; set; }
        public ServiceListings ServiceListings { get; set; }

        public int FeaturesId { get; set; }
        public Features Features { get; set; }
    }
}
