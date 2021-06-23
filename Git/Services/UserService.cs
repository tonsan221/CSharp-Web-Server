using Git.Data;
using Git.Data.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Git.Services
{
    public class UserService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UserService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public string CreateUser(string username, string email, string password)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                Password = HashPassword(password)
            };

            this.db.Users.Add(user);
            this.db.SaveChanges();

            return user.Id;
        }

        public string GetUserId(string username, string password)
            => this.db.Users
                .Where(x => x.Username == username && x.Password == HashPassword(password))
                .Select(x => x.Id)
                .FirstOrDefault();

        public bool IsEmailAvailable(string email)
            => !this.db.Users.Any(x => x.Email == email);

        public bool IsUsernameAvailable(string username)
            => !this.db.Users.Any(x => x.Username == username);

        private string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return string.Empty;
            }

            // Create a SHA256   
            using var sha256Hash = SHA256.Create();

            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convert byte array to a string   
            var builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
