namespace LibraryManagementSystem.Models.ViewModels
{
    public class BookUpdateViewModel // -> For update viewmodel
    {
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public string AuthorName { get; set; }
        public string PublicationYear { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }
}
