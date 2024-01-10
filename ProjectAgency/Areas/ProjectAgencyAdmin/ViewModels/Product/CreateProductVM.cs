using System.ComponentModel.DataAnnotations;
using ProjectAgency.Models;
namespace ProjectAgency.Areas.ProjectAgencyAdmin.ViewModels
{
    public class CreateProductVM
    {
        [Required]
        [MinLength(3,ErrorMessage = "Product name can contain maximum 30 characters")]
        public string Name { get; set; }
        public string? Desctiption { get; set; }
        public IFormFile? Photo { get; set; }
        public int? CategoryId { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
