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
        private string _signUpEndpoint = "https://localhost:44365/api/users/signup";
        private string _signInEndpoint = "https://localhost:44365/api/users/signin";

        private static HttpClient _httpClient = new HttpClient();
        public Task<string> SignIn(UserSignInVM user)
        {
            throw new NotImplementedException();
        }

        public Task<string> SignUp(UserSignUpVM user)
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
                    return null;
                }
                else
                {
                    return accessToken;
                }
            }
        }
    }
}
