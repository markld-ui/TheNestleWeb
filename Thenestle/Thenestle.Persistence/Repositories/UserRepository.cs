using System;
using Thenestle.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Thenestle.Domain.Models;
using Thenestle.Persistence.Data;
using System.Linq.Dynamic.Core;

namespace Thenestle.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<(ICollection<User> Users, int TotalCount)> GetUsersAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortField = "UserId",
            bool ascending = true)
        {
            var query = _context.Users.AsQueryable();

            //if (!string.IsNullOrWhiteSpace(searchTerm))
            //{
            //    query = query.Where(u =>
            //        EF.Functions.ILike(u.FirstName, $"%{searchTerm}%") ||
            //        EF.Functions.ILike(u.LastName, $"%{searchTerm}%") ||
            //        EF.Functions.ILike(u.Email, $"%{searchTerm}%"));
            //}

            var sortDirection = ascending ? "ASC" : "DESC";
            query = query.OrderBy($"{sortField} {sortDirection}");

            var totalCount = await query.CountAsync();
            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, totalCount);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UserExistsByIdAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.UserId == id);
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserRefreshTokenAsync(int userId, string? refreshToken, DateTime? refreshTokenExpiryTime)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }
    }
}
