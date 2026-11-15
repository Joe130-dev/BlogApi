using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogposController : ControllerBase
    {
        private readonly BlogContext adatbazis;

        // Adatbázis-kapcsolat átvétele
        public BlogposController(BlogContext adatbazis)
        {
            this.adatbazis = adatbazis;
        }

        // Az összes blogbejegyzés lekérdezése
        [HttpGet]
        public async Task<ActionResult<List<Blogpost>>> OsszesBejegyzes()
        {
            var bejegyzesek = await adatbazis.Blogpostok.ToListAsync();

            return Ok(bejegyzesek);
        }
    }
}
