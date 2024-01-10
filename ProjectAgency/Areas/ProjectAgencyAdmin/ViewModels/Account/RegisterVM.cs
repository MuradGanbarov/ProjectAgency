using ProjectAgency.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectAgency.Areas.ProjectAgencyAdmin.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        [MinLength(3,ErrorMessage="Name can contain minimum 3 characters")]
        [MaxLength(25,ErrorMessage ="Name can contain maximum 25 characters")]
        public string Name { get; set; }
        [Required]
        [MinLength(3,ErrorMessage ="Surname can contain minumum 3 characters")]
        [MaxLength(25,ErrorMessage ="Surname can contain maximum 25 characters")]
        public string Surname { get; set; }
        [Required]
        [MaxLength(25,ErrorMessage ="Username can contain maximum 25 characters")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Gender is required")]
        public Gender Gender { get; set; }
        [Required(ErrorMessage ="Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage ="You need confirm password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Password should be match")]
        public string ConfirmPassword { get; set; }
    }
}
