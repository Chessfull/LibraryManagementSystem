using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models.ViewModels
{
    public class BookCreateViewModel
    {
        [Required (ErrorMessage ="You should fill this field.")]
        [MinLength (1)]
        public string BookTitle { get; set; }
        
        public string ISBN { get; set; }
        
        [Required(ErrorMessage = "You should fill this field.")]
        [MinLength(1)]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "You should fill this field.")]
        [MinLength(1)]
        public string Publisher { get; set; }
        
        public string PublicationYear { get; set; }
        
        public string Genre { get; set; }

        [Required(ErrorMessage = "You should fill this field.")]
        [MinLength(10)]
        public string Description { get; set; }
        
        public string ImageURL { get; set; }

    }
}
