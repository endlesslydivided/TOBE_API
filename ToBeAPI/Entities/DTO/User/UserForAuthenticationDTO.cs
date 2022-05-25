using System.ComponentModel.DataAnnotations;

namespace ToBeApi.Entities.DTO.User
{
    public class UserForAuthenticationDTO
    {
            [Required(ErrorMessage = "Username is required")]
            public string UserName { get; set; }
            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; }
    }
}
