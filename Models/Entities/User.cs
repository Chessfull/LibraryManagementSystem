namespace LibraryManagementSystem.Models.Entities
{
    public class User // UserDB entity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime JoinDate { get; set; }

        // ▼ ctor when creating new instance auto join date and guid id ▼
        public User()
        {
            Id = Guid.NewGuid();
            JoinDate = DateTime.Now;
        }
    }
}
