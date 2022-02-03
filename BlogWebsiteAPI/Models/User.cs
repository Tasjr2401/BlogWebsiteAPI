namespace BlogWebsiteAPI.Models
{
    public class User
    {
        public User() { }
        public User(string userName, string firstName, string lastName)
        {
            Username = userName;
            FirstName = firstName;
            LastName = lastName;
        }
        public User(string userName, string firstName, string lastName, string role)
        {
            Username = userName;
            FirstName = firstName;
            Role = role;
            LastName = lastName;
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }

    public enum ValidRoles
    {
        Base,
        Admin
    }

    public class UserLogin
    {
        //Class Unnecessary at current moment come back to later
        public UserLogin(string userName, string password)
        {

        }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Salt { get; set; }
        public int UserId { get; set; }
    }

    public class UserPasswordCheckModel
    {
        public UserPasswordCheckModel() { }
        public UserPasswordCheckModel(string hashedPassword, byte[] salt, int userId) 
        {
            HashedPassword = hashedPassword;
            Salt = salt;
            UserId = userId;
        }
        public string HashedPassword { get; set; }
        public byte[] Salt { get; set; }
        public int UserId { get; set; }
    }
}
