namespace BlogWebsiteAPI.Models
{
    public class User
    {
        public User(string userName, string firstName, string lastName)
        {
            Username = userName;
            FirstName = firstName;
            LastName = lastName;
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Role { get; set; }
    }
}
