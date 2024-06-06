namespace Dissertation.Models
{
    public class Features
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconPath { get; set; }

        public ICollection<ServiceListingFeature> ServiceListingFeatures { get; set; } = new List<ServiceListingFeature>();

    }
}
