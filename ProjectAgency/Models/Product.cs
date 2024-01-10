namespace ProjectAgency.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        //relation props
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
