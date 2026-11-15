using Microsoft.EntityFrameworkCore;
using BlogApi.Models;

namespace BlogApi.Data
{
    public class BlogContext : DbContext
    {
        // Az adatbázis-kapcsolat beállításainak átvétele
        public BlogContext(DbContextOptions<BlogContext> beallitasok)
            : base(beallitasok)
        {
        }

        // A MySQL blogger táblájának elérése
        public DbSet<Blogger> Bloggerek { get; set; }

        // A blogbejegyzések adatbázistáblája
        public DbSet<Blogpost> Blogpostok { get; set; }

        // A modellek hozzárendelése a MySQL táblákhoz
        protected override void OnModelCreating(ModelBuilder modellEpito)
        {
            modellEpito.Entity<Blogger>().ToTable("blogger");
            modellEpito.Entity<Blogpost>().ToTable("blogpost");
        }
    }

}
