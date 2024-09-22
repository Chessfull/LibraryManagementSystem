using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models.ViewModels
{
    public class SignUpViewModel // -> For SignUp viewmodel
    { 

        [Required(ErrorMessage = "Min length 1.")] // -> Asp Validation required and min length
        [MinLength(1)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Min length 1.")]
        [MinLength(1)]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Min length 1.")]
        [MinLength(1)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Min length 4.")]
        [MinLength(4)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }
        public string PhoneNumber { get; set; }
       
    }
}
