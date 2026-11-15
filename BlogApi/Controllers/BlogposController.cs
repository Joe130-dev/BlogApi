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

        // Új blogbejegyzés létrehozása
        [HttpPost]
        public async Task<ActionResult<Blogpost>> UjBejegyzes(Blogpost ujBejegyzes)
        {
            // Ellenőrizzük, hogy létezik-e a megadott blogger
            bool bloggerLetezik = await adatbazis.Bloggerek
                .AnyAsync(blogger => blogger.Id == ujBejegyzes.BlogId);

            if (!bloggerLetezik)
            {
                return BadRequest("A megadott blogger nem létezik.");
            }

            // A bejegyzés létrehozási idejének beállítása
            ujBejegyzes.PostTime = DateTime.Now;
            ujBejegyzes.UpdateTime = DateTime.Now;

            // Új bejegyzés mentése az adatbázisba
            adatbazis.Blogpostok.Add(ujBejegyzes);
            await adatbazis.SaveChangesAsync();

            return StatusCode(201, ujBejegyzes);
        }

    }
}
