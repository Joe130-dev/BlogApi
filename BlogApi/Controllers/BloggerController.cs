using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        private readonly BlogContext adatbazis;

        // Adatbázis-kapcsolat átvétele
        public BloggerController(BlogContext adatbazis)
        {
            this.adatbazis = adatbazis;
        }

        // Az összes regisztrált blogger lekérdezése
        [HttpGet]
        public async Task<ActionResult<List<Blogger>>> OsszesBlogger()
        {
            var bloggerek = await adatbazis.Bloggerek.ToListAsync();

            return Ok(bloggerek);
        }
        // Új blogger regisztrálása
        [HttpPost]
        public async Task<ActionResult<Blogger>> UjBlogger(Blogger ujBlogger)
        {
            adatbazis.Bloggerek.Add(ujBlogger);

            await adatbazis.SaveChangesAsync();

            return CreatedAtAction(
                nameof(OsszesBlogger),
                new { id = ujBlogger.Id },
                ujBlogger
            );
        }
        // Egy blogger lekérdezése azonosító alapján
        [HttpGet("{id}")]
        public async Task<ActionResult<Blogger>> BloggerIdAlapjan(int id)
        {
            var blogger = await adatbazis.Bloggerek.FindAsync(id);

            if (blogger == null)
            {
                return NotFound("A megadott azonosítóval nem található blogger.");
            }

            return Ok(blogger);
        }
    }
}
