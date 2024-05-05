using crud.DataContext;
using crud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public BrandController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brands>>> Get()
        {
            if (_context.Brands == null)
            {
                return NotFound();
            }

            return await _context.Brands.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brands>> GetBrands(int id)
        {
            if (_context.Brands == null)
            {
                return NotFound();
            }

            //if there is data then first find the data and then return it
            Brands? brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return brand;
        }


        [HttpPost]
        public async Task<ActionResult<Brands>> PostBrand(Brands brands)
        {
            //brand object is now added to the database
            _context.Brands.Add(brands);
            await _context.SaveChangesAsync();

            //now redirect at action 
            return CreatedAtAction(nameof(GetBrands), new { id = brands.Id },brands);
        }

        [HttpPut("{id}")]
        //the put is get id of the particular thing and changed in database
        public async Task<IActionResult> PutBrand(int id, Brands brands)
        {
            if (id != brands.Id)
            {
                return BadRequest();
            }

            _context.Entry(brands).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        private bool BrandAvailable(int id)
        {
            return (_context.Brands?.Any(s => s.Id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Brands>> DeleteBrand(int id)
        {
            var brand = await _context.Brands.FirstOrDefaultAsync(s => s.Id == id);

            if (brand == null)
            {
                return NotFound();
            }

             _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
