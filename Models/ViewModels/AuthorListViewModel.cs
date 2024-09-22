namespace LibraryManagementSystem.Models.ViewModels
{
    public class AuthorListViewModel:BasePageBuild
    {
        public IEnumerable<AuthorViewModel> Authors { get; set; }
        public AuthorUpdateViewModel Author { get; set; }

    }
}
