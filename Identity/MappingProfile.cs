using AutoMapper;
using Identity.Models;
using Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<ApplicationUser, UserSignUpVM>(MemberList.Destination);
            CreateMap<UserSignUpVM, ApplicationUser>(MemberList.Source);
            CreateMap<ApplicationUser, UserSignInVM>(MemberList.Destination);
            CreateMap<UserSignInVM, ApplicationUser>(MemberList.Source);
            CreateMap<ApplicationUser, UserVM>(MemberList.Destination);
            CreateMap<UserVM, ApplicationUser>(MemberList.Source);
        }
    }
}
