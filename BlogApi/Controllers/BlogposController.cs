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

        // Meglévő blogbejegyzés módosítása
        [HttpPut("{azonosito}")]
        public async Task<IActionResult> BejegyzesModositasa(
            int azonosito, Blogpost modositottBejegyzes)
        {
            // Megkeressük a módosítandó bejegyzést
            var bejegyzes = await adatbazis.Blogpostok.FindAsync(azonosito);

            if (bejegyzes == null)
            {
                return NotFound("A megadott blogbejegyzés nem található.");
            }

            // Ellenőrizzük, hogy létezik-e a megadott blogger
            bool bloggerLetezik = await adatbazis.Bloggerek
                .AnyAsync(blogger => blogger.Id == modositottBejegyzes.BlogId);

            if (!bloggerLetezik)
            {
                return BadRequest("A megadott blogger nem létezik.");
            }

            // A bejegyzés adatainak frissítése
            bejegyzes.Title = modositottBejegyzes.Title;
            bejegyzes.Content = modositottBejegyzes.Content;
            bejegyzes.BlogId = modositottBejegyzes.BlogId;
            bejegyzes.UpdateTime = DateTime.Now;

            // A módosítások mentése
            await adatbazis.SaveChangesAsync();

            return NoContent();

        }

        // Blogbejegyzés törlése azonosító alapján
        [HttpDelete("{azonosito}")]
        public async Task<IActionResult> BejegyzesTorlese(int azonosito)
        {
            // Megkeressük a törlendő bejegyzést
            var bejegyzes = await adatbazis.Blogpostok.FindAsync(azonosito);

            if (bejegyzes == null)
            {
                return NotFound("A megadott blogbejegyzés nem található.");
            }

            // Bejegyzés törlése az adatbázisból
            adatbazis.Blogpostok.Remove(bejegyzes);
            await adatbazis.SaveChangesAsync();

            return NoContent();
        }

        // Egy blogger nevének és összes bejegyzésének lekérdezése
        // INNER JOIN a blogger és blogpost táblák között
        [HttpGet("blogger-bejegyzesei/{azonosito}")]
        public async Task<IActionResult> BloggerBejegyzesei(int azonosito)
        {
            var bejegyzesek = await (
                from blogger in adatbazis.Bloggerek
                join bejegyzes in adatbazis.Blogpostok
                    on blogger.Id equals bejegyzes.BlogId
                where blogger.Id == azonosito
                select new
                {
                    BloggerNev = blogger.Name,
                    BejegyzesCim = bejegyzes.Title,
                    BejegyzesTartalom = bejegyzes.Content
                }
            ).ToListAsync();

            return Ok(bejegyzesek);
        }

        // Az adatbázisban található összes blogbejegyzés száma
        [HttpGet("darabszam")]
        public async Task<IActionResult> BejegyzesekDarabszama()
        {
            int bejegyzesekSzama = await adatbazis.Blogpostok.CountAsync();

            return Ok(new
            {
                OsszesBejegyzes = bejegyzesekSzama
            });
        }



    }
}
