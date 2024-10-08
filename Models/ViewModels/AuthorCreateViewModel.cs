﻿using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models.ViewModels
{
    public class AuthorCreateViewModel // -> For create viewmodel
    {
        public Guid AuthorId { get; set; }
        [Required(ErrorMessage = "You should fill this field.")] // -> Asp Validation required and min length
        [MinLength(1)]
        public string FullName { get; set; }
        [Required(ErrorMessage = "You should fill this field.")]
        [MinLength(4)]
        public string Birthdate { get; set; }
        [Required(ErrorMessage = "You should fill this field.")]
        [MinLength(10)]
        public string About { get; set; }
        public string ImageUrl { get; set; }
    }
}
