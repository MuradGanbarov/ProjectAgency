using System.ComponentModel.DataAnnotations;

namespace ProjectAgency.Areas.ProjectAgencyAdmin.ViewModels
{
    public class CreateCategoryVM
    {
        [Required]
        [MinLength(3,ErrorMessage ="Category name can contain minimum 3 characters")]
        public string Name { get; set; }
    }
}
