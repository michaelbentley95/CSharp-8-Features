using Common.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp.Services
{
    class AuthService : IAuthService
    {
        private string _signUpEndpoint = "https://localhost:43744/api/users/signup";
        private string _signInEndpoint = "https://localhost:43744/api/users/signin";
        private string _currentAccessToken = "";

        private static HttpClient _httpClient = new HttpClient();
        public async Task<string> SignIn(UserSignInVM user)
        {
            return await AuthAction(_signInEndpoint, user);
        }

        public async Task<string> SignUp(UserSignUpVM user)
        {
            return await AuthAction(_signUpEndpoint, user);
        }
        public string GetAccessToken()
        {
            throw new NotImplementedException();
        }

        public async Task<string> AuthAction<T>(string endpoint, T content)
        {
            var body = JsonConvert.SerializeObject(content);
            HttpResponseMessage response = await _httpClient.PostAsync(endpoint, new StringContent(body, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            else
            {
                string accessToken = await response.Content.ReadAsStringAsync();
                if (accessToken.Length == 0)
                {
                    _currentAccessToken = null;
                }
                else
                {
                    _currentAccessToken = accessToken;
                }
                return _currentAccessToken;
            }
        }
    }
}
