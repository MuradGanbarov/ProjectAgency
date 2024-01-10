using ProjectAgency.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectAgency.Areas.ProjectAgencyAdmin.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password  { get; set; }
        public bool isRemembered { get; set; }
    }
}
