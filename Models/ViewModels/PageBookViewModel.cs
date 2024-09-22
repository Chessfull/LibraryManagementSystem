using LibraryManagementSystem.Models.Entities;

namespace LibraryManagementSystem.Models.ViewModels
{
    public class PageBookViewModel
    {

        public IEnumerable<Book> Books { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
