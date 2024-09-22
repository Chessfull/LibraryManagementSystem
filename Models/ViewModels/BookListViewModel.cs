using LibraryManagementSystem.Models.Entities;

namespace LibraryManagementSystem.Models.ViewModels
{
    public class BookListViewModel:BasePageBuild
    {
        public IEnumerable<BookViewModel> Books { get; set; }
        public BookUpdateViewModel BookUpdated { get; set; }
       
    }
}
