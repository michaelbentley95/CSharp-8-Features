using AutoMapper;
using Identity.Data;
using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    [Route("/api/users")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ApplicationDbContext _context;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private IMapper _mapper;

        public AccountController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        private string GenerateAccessToken(ApplicationUser appUser)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, appUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Email", appUser.Email),
                new Claim("FirstName", appUser.FirstName),
                new Claim("LastName", appUser.LastName),
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")));
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = Environment.GetEnvironmentVariable("ISSUER"),
                Audience = Environment.GetEnvironmentVariable("AUDIENCE"),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(claims)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpVM user)
        {
            ApplicationUser appUser = _mapper.Map<UserSignUpVM, ApplicationUser>(user);
            appUser.UserName = user.Email;
            IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
            if(result == IdentityResult.Success)
            {
                await _signInManager.SignInAsync(appUser, false);
                string token = GenerateAccessToken(appUser);
                return Ok(token);
            }
            else
            {
                throw new Exception("Sign-Up Failed");
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInVM user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);
            if(result == Microsoft.AspNetCore.Identity.SignInResult.Success)
            {
                var appUser = await _context.Users.SingleAsync(u => u.Email == user.Email);
                string token = GenerateAccessToken(appUser);
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
