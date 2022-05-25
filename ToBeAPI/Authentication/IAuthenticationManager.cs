using ToBeApi.Entities.DTO.User;

namespace ToBeApi.Authentication
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(UserForAuthenticationDTO userForAuth);
        Task<string> CreateToken();
    }

}
