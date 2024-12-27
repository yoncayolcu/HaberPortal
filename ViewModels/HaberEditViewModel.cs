using System.ComponentModel.DataAnnotations;

public class HaberEditViewModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Başlık gereklidir")]
    [StringLength(200, ErrorMessage = "Başlık 200 karakterden uzun olamaz")]
    public string Baslik { get; set; }

    [Required(ErrorMessage = "İçerik gereklidir")]
    public string Icerik { get; set; }

    [Url(ErrorMessage = "Geçerli bir URL giriniz")]
    [Display(Name = "Resim URL")]
    public string ResimUrl { get; set; }
}
