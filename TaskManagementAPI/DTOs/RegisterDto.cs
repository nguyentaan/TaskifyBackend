﻿using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs
{
    public class RegisterDto
    {
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        [Required(ErrorMessage = "Username is required.")]
        public required string Username { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string Password { get; set; }
    }
}
