using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToBeApi.Models
{
    public class User : IdentityUser
    {
        //[Column("UserId")]
        //public Guid Id { get; set; }


        [Required(ErrorMessage = "FirstName is required field.")]
        [StringLength(maximumLength: 255, MinimumLength = 1, ErrorMessage = "FirstName lenght is 1-255 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required field.")]
        [StringLength(maximumLength: 255, MinimumLength = 1, ErrorMessage = "FirstName lenght is 1-255 characters")]
        public string LastName { get; set; }

        public ICollection<Post> Posts { get; set; }

        //public DateTime CreatedAt { get; set; } = DateTime.Now;
        //public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
}
