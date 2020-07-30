using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API_Estate_management.Models.Model;
using API_Estate_management.Models.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API_Estate_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationAuthUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOptions<ApplicationSettings> _options;

        public ApplicationAuthUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> options)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options;
        }

        [HttpPost]
        [Route("Register")]
        // POST: api/ApplicationAuthUser/Register
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            else if (ModelState.IsValid)
            {
                if (EmailExistes(model.Email))
                {
                    return BadRequest("Email is not avaliable");
                }

                if (UserNameExistes(model.UserName))
                {
                    return BadRequest("UserName already exists in the database.");
                }

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.UserName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        // Checking existes username and email
        private bool UserNameExistes(string userName)
        {
            return _context.Users.Any(x => x.UserName == userName);
        }

        private bool EmailExistes(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }

        // Checking type Email xx@xx.com && xx@xx.net
        private bool IsEmailValid(string email)
        {
            Regex em = new Regex(@"\w+\@\w+.com|\w+\@\w+.net");
            if (em.IsMatch(email))
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        [Route("Login")]
        // POST: api/ApplicationAuthUser/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            else if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return BadRequest("You have incorrectly entered your Email or Password.");
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    // Authentication successful so generate Jwt Token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_options.Value.JWT_Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim(ClaimTypes.Role, "User"),
                        new Claim("UserId", user.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    // Return basic user info and authentication token
                    var value = new ApplicationAuthUser
                    {
                        Email = model.Email,
                        Token = tokenHandler.WriteToken(token)
                    };

                    return Ok(value);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpGet]
        [Route("GetUserProfile")]
        // GET: api/ApplicationAuthUser/GetUserProfile
        public async Task<Object> GetUserProfile()
        {
            string userId = User.Claims.First(x => x.Type == "UserId").Value;
            var user = await _userManager.FindByIdAsync(userId);

            return new
            {
                user.Email,
                user.UserName
            };
        }
    }
}
