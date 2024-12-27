using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HaberPortal.Models
{
    public class Haber
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Baslik { get; set; }

        [Required]
        public string Icerik { get; set; }

        [StringLength(500)]
        public string ResimUrl { get; set; } // Opsiyonel: Haberle ilişkili bir resim

        [Required]
        public DateTime YayinTarihi { get; set; }

        // Yazar ile ilişki
        [Required]
        public string YazarId { get; set; } // Identity ile ilişkilendirme

        [ForeignKey("YazarId")]
        public ApplicationUser Yazar { get; set; }
    }

}
