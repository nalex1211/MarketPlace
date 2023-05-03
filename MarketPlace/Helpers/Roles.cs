namespace MarketPlace.Helpers;

public static class Roles
{
    public static class Role
    {
        public const string Admin = "admin";
        public const string User = "user";
        public const string Vendor = "vendor";
        public static ICollection<string> All => new[] { Admin, User, Vendor };
    }
}
