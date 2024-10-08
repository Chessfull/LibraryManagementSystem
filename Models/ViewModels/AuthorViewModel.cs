﻿namespace LibraryManagementSystem.Models.ViewModels
{
    public class AuthorViewModel
    {
        public Guid AuthorId { get; set; }
        public string FullName { get; set; }
        public string Birthdate { get; set; }
        public string About { get; set; }
        public string ImageUrl { get; set; }
        public byte IsDeleted { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
