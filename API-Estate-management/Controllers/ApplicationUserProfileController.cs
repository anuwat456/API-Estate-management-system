using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Estate_management.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API_Estate_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserProfileController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;

        public ApplicationUserProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        // GET: api/ApplicationUserProfile
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
