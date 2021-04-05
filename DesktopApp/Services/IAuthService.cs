using Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp.Services
{
    public interface IAuthService
    {
        Task<string> SignUp(UserSignUpVM user);
        Task<string> SignIn(UserSignInVM user);
    }
}
