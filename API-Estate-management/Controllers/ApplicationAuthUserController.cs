using System;
using System.Collections.Generic;
using System.Globalization;
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

        public ApplicationAuthUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> options,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options;
            _roleManager = roleManager;
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
                    return BadRequest("Email already exists in the database.");
                }
                if (UserNameExistes(model.UserName))
                {
                    return BadRequest("UserName already exists in the database.");
                }

                if (IsEmailValid(model.Email))
                {
                    var user = new ApplicationUser
                    {
                        Email = model.Email,
                        UserName = model.UserName,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        RoleId = _options.Value.SetRoleDefault                // Set Role default is Manager
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
                return BadRequest(new JsonResult("Email is invalid"));
            }
            return BadRequest(new JsonResult(errorList));
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
                            new Claim(ClaimTypes.Email, model.Email),
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
                        authTokenKey = tokenHandler.WriteToken(token)
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
        [Authorize(Policy = "RequireLoggedIn")]
        // GET: api/ApplicationAuthUser/GetAllUsers
        public IActionResult GetAllUsers()
        {
            return Ok(_context.Users.ToList());
        }

        [HttpPost("[action]")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // POST: api/ApplicationAuthUser/CreateUser
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
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
                    return BadRequest("Email already exists in the database.");
                }
                if (UserNameExistes(model.UserName))
                {
                    return BadRequest("UserName already exists in the database.");
                }

                if (IsEmailValid(model.Email))
                {
                    // Date of Birth format Datetime
                    string formatBirthDate = model.BirthDate.ToString();

                    var newUser = new ApplicationUser
                    {
                        IdNumber = model.IdNumber,
                        FullName = model.FullName,
                        UserName = model.UserName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        AddressLine = model.AddressLine,
                        BirthDate = model.BirthDate,
                        Image = _options.Value.DefaultImageUrl,
                        RoleId = _options.Value.SetRoleDefault          // Set Role default is Manager
                    };

                    // The task was not Add() the password could not be encode.

                    var result = await _userManager.CreateAsync(newUser, model.Password);
                    if (result.Succeeded)
                    {
                        return Ok(new JsonResult("The User was add Successfully"));
                    }
                    else
                    {
                        return BadRequest(result.Errors);
                    }
                }
                return BadRequest(new JsonResult("Email is invalid"));
            }
            return BadRequest(new JsonResult(errorList));
        }

        [HttpPut("[action]/{id}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // PUT: api/ApplicationAuthUser/UpdateUser/id
        public async Task<IActionResult> UpdateUser([FromRoute] string id, [FromBody] UpdateUserModel model)
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

            // Date of Birth format Datetime
            string formatBirthDate = model.BirthDate.ToString();

            // The User was found
            findUser.IdNumber = model.IdNumber;
            findUser.FullName = model.FullName;
            findUser.UserName = model.UserName;
            findUser.NormalizedUserName = model.UserName.ToUpper();
            findUser.Email = model.Email;
            findUser.NormalizedEmail = model.Email.ToUpper();
            findUser.PhoneNumber = model.PhoneNumber;
            findUser.AddressLine = model.AddressLine;
            findUser.BirthDate = model.BirthDate;
            findUser.Image = model.Image;

            if (model.RoleId == null)
            {
                findUser.RoleId = model.RoleId;
            }
            findUser.RoleId = _options.Value.SetRoleDefault;          // Set Role default is Manager

            _context.Entry(findUser).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

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
            var findUser = _context.Users.FirstOrDefault(u => u.Id == id);
            if (findUser == null)
            {
                return NotFound();
            }

            // Get Name User
            var getName = await _userManager.GetUserNameAsync(findUser);

            _context.Users.Remove(findUser);
            await _context.SaveChangesAsync();

            // Finally return the result to client
            return Ok(new JsonResult("The User with Username : " + getName + " is Delete."));
        }


        // Checking existes User
        private bool UserNameExistes(string userName)
        {
            return _context.Users.Any(x => x.UserName == userName);
        }

        private bool EmailExistes(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }

        private bool UserExists(string id)
        {
            return _context.Users.Count(u => u.Id == id) > 0;
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
    }
}
