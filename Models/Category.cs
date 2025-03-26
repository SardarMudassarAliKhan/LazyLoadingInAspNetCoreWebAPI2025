namespace LazyLoadingInAspNetCoreWebAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Lazy-loaded navigation property
        public virtual ICollection<Product> Products { get; set; }
    }
}
