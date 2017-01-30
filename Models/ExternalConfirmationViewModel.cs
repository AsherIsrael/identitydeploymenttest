using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class ExternalConfirmationViewModel
    {
        [Required]
        public string UserName { get; set; }
    }
}