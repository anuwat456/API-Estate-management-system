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
    public class CommonFeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommonFeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        // GET: api/CommonFee/GetAllCommonFee
        public IActionResult CommonFee()
        {
            return Ok(_context.CommonFees.ToList());
        }

        [HttpPost("[action]")]
        // POST: api/CommonFee/CreateCommonFee
        public async Task<IActionResult> CreateCommonFee([FromBody] CreateCommonFeeModel model)
        {
            if (model == null)
            {
                return NotFound();
            }
            else if (ModelState.IsValid)
            {
                var createCommonFees = new ApplicationCommonFee
                {
                    Amount = model.Amount,
                    Detail = model.Detail,
                    StatusPay = false,
                    KeepDate = DateTime.Now.Date,
                    GetDate = model.GetDate
                };

                _context.CommonFees.Add(createCommonFees);
                await _context.SaveChangesAsync();

                // Finally return the result to client
                return Ok(new JsonResult("The News was add Successfully"));
            }
            return NotFound();
        }

        [HttpPut("[action]/{id}")]
        // PUT: api/CommonFee/UpdateCommonFee/id
        public async Task<IActionResult> UpdateCommonFee([FromRoute] string id, [FromBody] CreateCommonFeeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findCommonFee = _context.CommonFees.FirstOrDefault(cf => cf.Id == id);
            if (findCommonFee == null)
            {
                return NotFound();
            }

            // The CommonFee was found
            findCommonFee.Amount = model.Amount;
            findCommonFee.Detail = model.Detail;
            findCommonFee.StatusPay = model.StatusPay;
            findCommonFee.GetDate = model.GetDate;

            _context.Entry(findCommonFee).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommonFeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Finally return the result to client
            return Ok(new JsonResult("The CommonFee with id " + id + " is Update."));
        }

        [HttpDelete("[action]/{id}")]
        // DELETE: api/CommonFee/DeleteCommonFee/id
        public async Task<IActionResult> DeleteCommonFee([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // The News was found
            var findCommonFee = await _context.CommonFees.FindAsync(id);
            if (findCommonFee == null)
            {
                return NotFound();
            }

            _context.CommonFees.Remove(findCommonFee);
            await _context.SaveChangesAsync();

            // Finally return the result to client
            return Ok(new JsonResult("The CommonFee with id : " + id + " is Delete."));
        }

        private bool CommonFeeExists(string id)
        {
            return _context.CommonFees.Count(cf => cf.Id == id) > 0;
        }
    }
}
