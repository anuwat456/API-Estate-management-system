using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Estate_management.Models.Model;
using API_Estate_management.Models.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Estate_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonFeeTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommonFeeTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        // GET: api/CommonFeeType/GetAllCommonFeeType
        public IActionResult GetAllCommonFeeType()
        {
            return Ok(_context.CommonFeeTypes.ToList());
        }

        [HttpPost("[action]")]
        // POST: api/CommonFeeType/CreateCommonFeeType
        public async Task<IActionResult> CreateCommonFeeType([FromBody] CreateCommonFeeTypeModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            else if (ModelState.IsValid)
            {
                var createCommonFeeType = new ApplicationCommonFeeType
                {
                    Type = model.Type
                };

                _context.CommonFeeTypes.Add(createCommonFeeType);
                await _context.SaveChangesAsync();

                // Finally return the result to client
                return Ok(new JsonResult("The CommonFeeType was add Successfully"));
            }
            return NotFound();
        }

        [HttpPut("[action]/{id}")]
        // PUT: api/CommonFeeType/UpdateCommonFeeType
        public async Task<IActionResult> UpdateCommonFeeType([FromRoute] string id, [FromBody] CreateCommonFeeTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findCommonFeeType = _context.CommonFeeTypes.FirstOrDefault(cft => cft.Id == id);
            if (findCommonFeeType == null)
            {
                return NotFound();
            }

            // The News was found
            findCommonFeeType.Type = model.Type;

            _context.Entry(findCommonFeeType).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommonFeeTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Finally return the result to client
            return Ok(new JsonResult("The findCommonFeeType with id " + id + " is Update."));
        }

        [HttpDelete("[action]/{id}")]
        // DELETE: api/CommonFeeType/DeleteCommonFeeType
        public async Task<IActionResult> DeleteCommonFeeType([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // The News was found
            var findCommonFeeType = await _context.CommonFeeTypes.FindAsync(id);
            if (findCommonFeeType == null)
            {
                return NotFound();
            }

            _context.CommonFeeTypes.Remove(findCommonFeeType);
            await _context.SaveChangesAsync();

            // Finally return the result to client
            return Ok(new JsonResult("The CommonFeeType with id : " + id + " is Delete."));
        }

        private bool CommonFeeTypeExists(string id)
        {
            return _context.CommonFeeTypes.Count(cft => cft.Id == id) > 0;
        }
    }
}
