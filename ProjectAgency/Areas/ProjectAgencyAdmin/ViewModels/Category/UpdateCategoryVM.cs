using System.ComponentModel.DataAnnotations;

namespace ProjectAgency.Areas.ProjectAgencyAdmin.ViewModels
{
    public class UpdateCategoryVM
    {
        [MaxLength(ErrorMessage ="Category name can contain maximum 30 characters")]
        public string? Name { get; set; }
    }
}
