
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheWall.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        [Required]
        public string MessageText { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        public List<Comment> Comments { get; set; }

        // [Column("TestUserId")]
        public string TestUserId { get; set; }

        public TestUser TestUser { get; set; }

        public Message()
        {
            Comments = new List<Comment>();
        }
    }
}