using System.ComponentModel.DataAnnotations;

namespace GameCatalog.Entities;

public class Game
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }
    
    [Range(0, 200)]
    public decimal Price { get; set; }

    [Required]
    [StringLength(100)]
    public string Publisher { get; set; }
}