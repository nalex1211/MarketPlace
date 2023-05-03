namespace MarketPlace.Models;

public class Cart
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public ApplicationUsers? User { get; set; }
    public int? ProductId { get; set; }
    public Products? Product { get; set; }
}
