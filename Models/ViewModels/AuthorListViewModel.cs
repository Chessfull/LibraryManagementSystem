namespace LibraryManagementSystem.Models.ViewModels
{
    public class AuthorListViewModel: BasePageBuild // -> For ViewAll viewmodel, Base class for pagination
    {
        public IEnumerable<AuthorViewModel> Authors { get; set; }
        public AuthorUpdateViewModel Author { get; set; }

    }
}
