namespace LibraryManagementSystem.Models.ViewModels
{
    public class AuthorUpdateViewModel // -> For update pop up post form
    {
        public Guid AuthorId { get; set; }
        public string FullName { get; set; }
        public string Birthdate { get; set; }
        public string About { get; set; }
        public string ImageUrl { get; set; } 
    }
}
