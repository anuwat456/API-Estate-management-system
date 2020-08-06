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
using Microsoft.EntityFrameworkCore;
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
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public ApplicationAuthUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> options,
            RoleManager<ApplicationRole> roleManager, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("[action]")]
        // POST: api/ApplicationAuthUser/Register
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // Will hold all the errors related to registation
            List<string> errorList = new List<string>();

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
                    UserName = model.UserName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    RoleId = "2"                // Set default role is Manager
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {    
                    if (await _roleManager.RoleExistsAsync(ApplicationUserRole.Manager))
                    {
                        await _userManager.AddToRoleAsync(user, ApplicationUserRole.Manager);
                    }
                                        
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest(new JsonResult(errorList));
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

        [HttpPost("[action]")]
        // POST: api/ApplicationAuthUser/Login
        public async Task<IActionResult> Login([FromBody] LoginModel model)
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

                double tokenExpireTime = Convert.ToDouble(_options.Value.ExpiryTime);

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    // Authentication successful so generate Jwt Token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_options.Value.JWT_Secret);

                    // Find Role of User for Auth
                    var userRole = await _roleManager.FindByIdAsync(user.RoleId);
                    var roles = await _roleManager.GetRoleNameAsync(userRole);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, roles),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim("UserId", user.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(tokenExpireTime),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    
                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    // Return basic user info and authentication token
                    var value = new AuthUser
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

        [HttpGet("[action]")]
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

        [HttpGet("[action]")]
        //[Authorize(Policy = "RequireLoggedIn")]
        // GET: api/ApplicationAuthUser/GetUsers
        public IActionResult GetUsers()
        {
            return Ok(_context.Users.ToList());
        }

        [HttpPost("[action]")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // POST: api/ApplicationAuthUser/AddUser
        public async Task<IActionResult> AddUser([FromBody] ApplicationUser user)
        {
            // Passwordhash encoder
            var passwordhash = _passwordHasher.HashPassword(user, user.PasswordHash);
            user.SecurityStamp = Guid.NewGuid().ToString();

            var newuser = new ApplicationUser
            {
                NumberId = user.NumberId,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = passwordhash,
                PhoneNumber = user.PhoneNumber,
                AddressLine = user.AddressLine,
                BirthDate = user.BirthDate,
                Image = _options.Value.DefaultImageUrl
            };
            
            await _context.Users.AddAsync(newuser);
            await _context.SaveChangesAsync();

            return Ok(new JsonResult("The User was add Successfully"));
        }

        [HttpPut("action/{id}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // PUT: api/ApplicationAuthUser/UpdateUser/id
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findUser = _context.Users.FirstOrDefault(u => u.Id == id);
            if (findUser == null)
            {
                return NotFound();
            }

            // The User was found
            findUser.NumberId = user.NumberId;
            findUser.FullName = user.FullName;
            findUser.UserName = user.UserName;
            findUser.Email = user.Email;
            findUser.PhoneNumber = user.PhoneNumber;
            findUser.AddressLine = user.AddressLine;
            findUser.BirthDate = user.BirthDate;
            findUser.Image = user.Image;

            _context.Entry(findUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Finally return the result to client
            return Ok(new JsonResult("The User with id " + id + " is Update."));
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // DELETE: api/ApplicationAuthUser/DeleteUser/id
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // The User was found
            var findUser = await _context.Users.FindAsync(id);
            if (findUser == null)
            {
                return NotFound();
            }

            _context.Users.Remove(findUser);
            await _context.SaveChangesAsync();

            // Finally return the result to client
            return Ok(new JsonResult("The User with id " + id + " is Delete."));
        }
    }
}
