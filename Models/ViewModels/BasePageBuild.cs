namespace LibraryManagementSystem.Models.ViewModels
{
    public class BasePageBuild // -> For pagination base class
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }
}
