using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TheWall.Models
{
    public class TestUser : IdentityUser
    {
        
        // public int TestUserId { get; set; }
        // [Required]
        // [MinLength(2)]
        // [RegularExpression(@"^[a-zA-Z]+$")]
        // public string FirstName { get; set; }
        // [Required]
        // [MinLength(2)]
        // [RegularExpression(@"^[a-zA-Z]+$")]
        // public string LastName { get; set; }
        // [Required]
        // [EmailAddress]
        // public override string Email { get; set; }
        // [Required]
        // [MinLength(8)]
        // [DataType(DataType.Password)]
        // public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Message> Messages { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Shoppingcart> Shoppingcarts { get; set; }

        public TestUser()
        {
            Messages = new List<Message>();
            Comments = new List<Comment>();
            Shoppingcarts = new List<Shoppingcart>();
        }
    }
}