namespace LazyLoadingInAspNetCoreWebAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }

        // Lazy-loaded navigation property
        public virtual Category Category { get; set; }
    }
}
