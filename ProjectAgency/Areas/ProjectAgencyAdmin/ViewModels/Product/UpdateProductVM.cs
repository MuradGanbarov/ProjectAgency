using ProjectAgency.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectAgency.Areas.ProjectAgencyAdmin.ViewModels
{
    public class UpdateProductVM
    {
        [MinLength(3,ErrorMessage="Product name can contain minimum 30 characters")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Photo { get; set; }
        public string? Url { get; set; }
        public int? CategoryId { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
