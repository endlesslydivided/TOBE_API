using AutoMapper;
using ToBeApi.Data.DTO;
using ToBeApi.Entities.DTO.User;
using ToBeApi.Models;

namespace ToBeApi.Data.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Post
            CreateMap<Post, PostDTO>();
            CreateMap<PostForCreateDTO, Post>();
            CreateMap<PostForUpdateDTO, Post>();
            CreateMap<PostForUpdateDTO, Post>().ReverseMap();
            #endregion

            #region User
            CreateMap<UserForRegistrationDTO, User>();

            #endregion
        }
    }
}
