using System;
using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        [Required]
        public string CommentText { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        public int MessageId { get; set; }
        public Message Message { get; set; }

        public string TestUserId { get; set; }

        public TestUser TestUser { get; set; }
    }
}