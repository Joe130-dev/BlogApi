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
    }
}
