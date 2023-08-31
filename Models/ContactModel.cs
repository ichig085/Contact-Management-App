using System;
using System.ComponentModel.DataAnnotations;
using ContactAppWeb.Validation;
using Microsoft.EntityFrameworkCore;

namespace ContactAppWeb.Models
{
    public class ContactModel
    {
        // Primary key for the Contact entity
        [Key]
        public int Id { get; set; }

        // First name of the contact
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        // Last name of the contact
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Phone number of the contact
        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^1?[0-9]{10}$", ErrorMessage = "Please enter a valid 10-digit telephone number.")]
        //[Phone]
        public string PhoneNumber { get; set; }

        // Email address of the contact
        [Display(Name = "Email")]
        [Required(ErrorMessage = "The Email field is required")]
        [EmailValidation(ErrorMessage = "Invalid email format")] // Custom validation attribute
        public string Email { get; set; }
    }

}

