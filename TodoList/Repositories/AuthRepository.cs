using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Interfaces;
using TodoList.Model;

namespace TodoList.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context) => _context = context;

        public async Task<User> Register(User user, string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.Include(u => u.Todos).FirstOrDefaultAsync(x => x.Username == username);
            if (user == null) return null!;

            using var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            if (!computedHash.SequenceEqual(user.PasswordHash)) return null!;

            return user;
        }

        public async Task<bool> UserExists(string username)
            => await _context.Users.AnyAsync(x => x.Username == username);
    }
}
