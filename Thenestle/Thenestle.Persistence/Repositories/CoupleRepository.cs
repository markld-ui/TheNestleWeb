using System;
using Thenestle.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Thenestle.Domain.Models;
using Thenestle.Persistence.Data;
using System.Linq.Dynamic.Core;

namespace Thenestle.Persistence.Repositories
{
    public class CoupleRepository : ICoupleRepository
    {
        private readonly DataContext _context;

        public CoupleRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<(ICollection<Couple> Couples, int TotalCount)> GetCouplesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortField = "CoupleId",
            bool ascending = true)
        {
            var query = _context.Couples.AsQueryable();

            var sortDirection = ascending ? "ASC" : "DESC";
            query = query.OrderBy($"{sortField} {sortDirection}");

            var totalCount = await query.CountAsync();

            var couples = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (couples, totalCount);
        }

        public async Task<Couple?> GetCoupleByIdAsync(int id)
        {
            return await _context.Couples.FindAsync(id);
        }

        public async Task<bool> CoupleExistsByIdAsync(int id)
        {
            return await _context.Couples.AnyAsync(c => c.CoupleId == id);
        }

        public async Task AddCoupleAsync(Couple couple)
        {
            await _context.Couples.AddAsync(couple);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCoupleAsync(Couple couple)
        {
            _context.Couples.Remove(couple);
            await _context.SaveChangesAsync();
        }

        public async Task<Couple?> GetCoupleByUserIdAsync(int userId)
        {
            return await _context.Couples
                .Include(c => c.User1) // Подгружаем данные первого пользователя
                .Include(c => c.User2) // Подгружаем данные второго пользователя
                .AsNoTracking() // Для read-only операций повышаем производительность
                .FirstOrDefaultAsync(c => c.User1Id == userId || c.User2Id == userId);
        }

        public async Task UpdateCoupleAsync(Couple couple)
        {
            _context.Couples.Update(couple);
            await _context.SaveChangesAsync();
        }
    }
}
