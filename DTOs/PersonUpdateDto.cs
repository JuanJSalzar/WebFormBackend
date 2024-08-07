﻿using System.ComponentModel.DataAnnotations;

namespace WebForm.DTOs;

public class PersonUpdateDto
{
    [Required (ErrorMessage = "The field {0} is required.")]
    [MaxLength(50, ErrorMessage = "The field {0} must have a maximum of 50 characters.")] 
    [MinLength(3, ErrorMessage = "The field {0} must have a minimum of 3 characters.")] 
    public string FirstName { get; set; }
    
    [Required (ErrorMessage = "The field {0} is required.")]
    [MaxLength(50, ErrorMessage = "The field {0} must have a maximum of 50 characters.")] 
    [MinLength(3, ErrorMessage = "The field {0} must have a minimum of 3 characters.")]
    public string LastName { get; set; }
    
    [Required (ErrorMessage = "The field {0} is required.")]
    [MaxLength(100)]
    [MinLength(3, ErrorMessage = "The field {0} must have a minimum of 3 characters.")]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required (ErrorMessage = "The field {0} is required.")]
    [MaxLength(20, ErrorMessage = "The field {0} must have a maximum of 20 characters.")]
    [MinLength(2, ErrorMessage = "The field {0} must have a minimum of 2 characters.")] 
    [RegularExpression("^[A-Za-z]+$", ErrorMessage = "The field {0} must have only letters.")]
    public string IdentificationType { get; set; }
}