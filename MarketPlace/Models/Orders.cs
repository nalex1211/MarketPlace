using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models;

public class Orders
{
    [Key]
    public int Id { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    [ForeignKey("Products")]
    public int? ProductId { get; set; }
    public Products? Product { get; set; }
    [ForeignKey("ApplicationUsers")]
    public string? UserId { get; set; }
    public ApplicationUsers? User { get; set; }
}
