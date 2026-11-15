using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using BlogApi.Data;
namespace BlogApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            // MySQL-adatbázis kapcsolatának beállítása
            var kapcsolatiSzoveg = builder.Configuration.GetConnectionString("BlogAdatbazis");

            builder.Services.AddDbContext<BlogContext>(beallitasok =>
                beallitasok.UseMySql(
                    kapcsolatiSzoveg,
                    ServerVersion.AutoDetect(kapcsolatiSzoveg!)
                )
            );

            var app = builder.Build();
            // MySQL-adatbázis kapcsolatának ellenőrzése
            using (var hatokor = app.Services.CreateScope())
            {
                var adatbazis = hatokor.ServiceProvider.GetRequiredService<BlogContext>();

                Console.WriteLine(
                    adatbazis.Database.CanConnect()
                        ? "Sikeres MySQL-kapcsolat!"
                        : "Nem sikerült csatlakozni a MySQL-adatbázishoz!"
                );
            }


            if (app.Environment.IsDevelopment())
            {
                // OpenAPI dokumentáció létrehozása
                app.MapOpenApi();

                // Scalar grafikus API-tesztfelület
                app.MapScalarApiReference();
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
