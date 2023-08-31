using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ContactAppWeb.Validation
{
    public class EmailValidationAttribute : ValidationAttribute
    {
        // This method is responsible for validating the email address
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)  // Check if the value is not null
            {
                string email = value.ToString();  // Convert the value to a string (assumed to be an email address)
                string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

                // Use a regular expression pattern to validate the email address
                if (!Regex.IsMatch(email, pattern))
                {
                    // If the email does not match the pattern, return a validation error with the specified error message
                    return new ValidationResult(ErrorMessage);
                }
            }

            // If validation is successful, return ValidationResult.Success
            return ValidationResult.Success;
        }
    }
}