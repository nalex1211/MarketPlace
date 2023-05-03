using DocumentFormat.OpenXml.Drawing.Charts;
using MarketPlace.Models;

namespace MarketPlace.ViewModels;

public class BuyViewModel
{
    public List<Products> Products { get; set; }
    public ApplicationUsers ApplicationUsers { get; set; }
    public Orders Order { get; set; } = new Orders();
}
