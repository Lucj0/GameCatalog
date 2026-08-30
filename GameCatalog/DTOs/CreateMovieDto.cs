using System.ComponentModel.DataAnnotations;

namespace GameCatalog.DTOs;

public class CreateMovieDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [Required]
    [StringLength(100)]
    public string Genre { get; set; }

    [Range(1888, 2100)]
    public int ReleaseYear { get; set; }
}