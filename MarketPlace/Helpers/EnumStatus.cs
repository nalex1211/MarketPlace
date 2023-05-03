using MarketPlace.Models;

namespace MarketPlace.Helpers;

public static class EnumStatus
{
    public static string GetEnumStatus(Status status)
    {
        switch (status)
        {
            case Status.Delivered:
            return "Delivered";

            case Status.OnTheWay:
            return "Product is on the way";

            case Status.Lost:
            return "Product has been lost";

            case Status.Cancelled:
            return "Delivery has been cancelled";
            default:
            return "Error";
        }
    }
}
