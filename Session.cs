namespace SportPitShop
{
    public sealed class Session
    {
        public string Role { get; set; } = "Гость";
        public string DisplayName { get; set; } = "Гость";
        public int? EmployeeId { get; set; }
    }
}
