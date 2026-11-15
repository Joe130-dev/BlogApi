
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApi.Models
{
    [Table("blogger")]
    public class Blogger
    {
        // A blogger egyedi azonosítója
        [Key]
        [Column("id")]
        public int Id { get; set; }

        // A blogger neve
        [Column("name")]
        public string? Name { get; set; }

        // A blogger e-mail-címe
        [Column("email")]
        public string? Email { get; set; }

        // A blogger életkora
        [Column("age")]
        public int? Age { get; set; }

        // A blogger jelszava
        [Column("password")]
        public string? Password { get; set; }

        // A regisztráció időpontja
        [Column("RegistrationTime")]
        public DateTime? RegistrationTime { get; set; }
    }
}
