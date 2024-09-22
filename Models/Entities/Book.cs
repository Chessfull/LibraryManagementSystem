namespace LibraryManagementSystem.Models.Entities
{
    public class Book // BookDB entity
    {
        public Guid Id { get; set; }
        public string ISBN { get; set; }
        public string BookTitle { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string PublicationYear { get; set; }
        public string Publisher { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public DateTime UpdateDate { get; set; }
        public byte IsDeleted { get; set; }

    }
}
