using System;

namespace SeattleMafiaClub
{
    public class Item
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public Host Host { get; set; }
    }

    public class Host
    {
        // json values
        public string Login { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string Timestamp { get; set; }
    }
}
