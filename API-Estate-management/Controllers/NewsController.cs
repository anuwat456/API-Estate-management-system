using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Estate_management.Models.Model;
using API_Estate_management.Models.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Estate_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        [Authorize(Policy = "RequireLoggedIn")]
        // GET: api/News/GetAllNews
        public IActionResult GetAllNews()
        {
            return Ok(_context.News.ToList());
        }

        [HttpPost("[action]")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // POST: api/News/CreateNews
        public async Task<IActionResult> CreateNews([FromBody] CreateNewsModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            else if (ModelState.IsValid)
            {
                var createNews = new ApplicationNews
                {
                    Subject = model.Subject,
                    Detail = model.Detail,
                    ImageNews = model.ImageNews,
                    DateNews = DateTime.Now.Date
                };

                _context.News.Add(createNews);
                await _context.SaveChangesAsync();

                // Finally return the result to client
                return Ok(new JsonResult("The News was add Successfully"));
            }
            return NotFound();
        }

        [HttpPut("[action]/{id}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        // PUT: api/News/UpdateNews/id
        public async Task<IActionResult> UpdateNews([FromRoute] string id, [FromBody] CreateNewsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findNews = _context.News.FirstOrDefault(n => n.Id == id);
            if (findNews == null)
            {
                return NotFound();
            }

            // The News was found
            findNews.Subject = model.Subject;
            findNews.Detail = model.Detail;
            findNews.ImageNews = model.ImageNews;
            findNews.DateNews = DateTime.UtcNow.Date;

            _context.Entry(findNews).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Finally return the result to client
            return Ok(new JsonResult("The News with id " + id + " is Update."));
        }

        [HttpDelete("[action]/{id}")]
        // DELETE: api/News/DeleteNews/id
        public async Task<IActionResult> DeleteNews([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // The News was found
            var findNews = await _context.News.FindAsync(id);
            if (findNews == null)
            {
                return NotFound();
            }

            _context.News.Remove(findNews);
            await _context.SaveChangesAsync();

            // Finally return the result to client
            return Ok(new JsonResult("The News with id : " + id + " is Delete."));
        }

        private bool NewsExists(string id)
        {
            return _context.News.Count(n => n.Id == id) > 0;
        }
    }
}
