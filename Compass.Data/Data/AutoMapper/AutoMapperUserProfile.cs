using AutoMapper;
using Compass.Data.Data.Models;
using Compass.Data.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compass.Data.Data.AutoMapper
{
    public class AutoMapperUserProfile :Profile
    {
        public AutoMapperUserProfile()
        {
            CreateMap<AppUser, RegisterUserVM>()
                .ForMember(dst => dst.Email, obj => obj.MapFrom(src => src.Email))
                .ForMember(dst => dst.ConfirmPassword, obj => obj.MapFrom(src => src.PasswordHash))
                .ForMember(dst => dst.Password, obj => obj.MapFrom(src=>src.PasswordHash));

            CreateMap<AppUser, LoginUserVM>()
                .ForMember(dst => dst.Email, obj => obj.MapFrom(src => src.Email));

            CreateMap<RegisterUserVM, AppUser>()
                .ForMember(dst => dst.UserName, obj => obj.MapFrom(src =>src.Username))
                .ForMember(dst => dst.Name, obj => obj.MapFrom(src =>src.Name))
                .ForMember(dst => dst.Surname, obj => obj.MapFrom(src =>src.Surname));

            CreateMap<ChangeUserVM, AppUser>()
               .ForMember(dst => dst.UserName, obj => obj.MapFrom(src => src.Username))
               .ForMember(dst => dst.Name, obj => obj.MapFrom(src => src.Name))
               .ForMember(dst => dst.Email, obj => obj.MapFrom(src => src.Email))
               .ForMember(dst => dst.Surname, obj => obj.MapFrom(src => src.Surname));

            CreateMap<AppUser, AllUsersVM>()
                .ForMember(user => user.Id, obj => obj.MapFrom(app => app.Id))
                .ForMember(user => user.Name, obj => obj.MapFrom(app => app.Name))
                .ForMember(user => user.Username, obj => obj.MapFrom(app => app.UserName))
                .ForMember(user => user.Email, obj => obj.MapFrom(app => app.Email))
                .ForMember(user => user.EmailConfirmed, obj => obj.MapFrom(app => app.EmailConfirmed))
                .ForMember(user => user.Surname, obj => obj.MapFrom(app => app.Surname));
        }
    }
}
