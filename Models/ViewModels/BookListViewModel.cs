using LibraryManagementSystem.Models.Entities;

namespace LibraryManagementSystem.Models.ViewModels 
{
    public class BookListViewModel: BasePageBuild // -> For ViewAll viewmodel, Base class for pagination
    {
        public IEnumerable<BookViewModel> Books { get; set; }
        public BookUpdateViewModel BookUpdated { get; set; }
       
    }
}
