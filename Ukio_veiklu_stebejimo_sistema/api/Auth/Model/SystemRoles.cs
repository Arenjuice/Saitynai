namespace api.Auth.Model
{
    public class SystemRoles
    {
        public const string Admin = nameof(Admin);
        public const string Guest = nameof(Guest);
        public const string Farmer = nameof(Farmer);
        public const string Worker = nameof(Worker);

        public static readonly IReadOnlyCollection<string> All = new[] {Admin, Guest, Farmer, Worker };
    }
}
