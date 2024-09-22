namespace LibraryManagementSystem.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime JoinDate { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
            JoinDate = DateTime.Now;
        }
    }
}
