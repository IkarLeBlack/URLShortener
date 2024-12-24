using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data;
using URL_Shortener.Models;
using System.Threading.Tasks;

namespace URL_Shortener.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }


        public async Task<UserModel> GetUserByCredentialsAsync(string username, string passwordHash)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash);
        }


        public async Task CreateNewUserAsync(string username, string passwordHash)
        {
            var defaultRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == "User");
            var newUser = new UserModel
            {
                Username = username,
                PasswordHash = passwordHash,
                Role = defaultRole ?? new RoleModel { RoleName = "User" }
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }
    }
}