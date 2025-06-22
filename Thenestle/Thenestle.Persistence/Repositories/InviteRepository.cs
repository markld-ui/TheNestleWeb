using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Thenestle.Domain.Interfaces.Repositories;
using Thenestle.Domain.Models;
using Thenestle.Persistence.Data;

namespace Thenestle.Persistence.Repositories
{
    public class InviteRepository : IInviteRepository
    {
        private readonly DataContext _context;

        public InviteRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Invite?> GetInviteByIdAsync(int inviteId)
        {
            return await _context.Invites
                .Include(i => i.Couple)
                .Include(i => i.Inviter)
                .FirstOrDefaultAsync(i => i.InviteId == inviteId);
        }

        public async Task AddInviteAsync(Invite invite)
        {
            await _context.Invites.AddAsync(invite);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInviteAsync(Invite invite)
        {
            _context.Invites.Update(invite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInviteAsync(Invite invite)
        {
            _context.Invites.Remove(invite);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Invite>> GetInvitesAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _context.Invites
                .Include(i => i.Couple)
                .Include(i => i.Inviter)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<ICollection<Invite>> GetInvitesByCodeAsync(string code)
        {
            return await _context.Invites
                .Where(i => i.Code == code)
                .ToListAsync();
        }

        public async Task<ICollection<Invite>> GetInvitesByUserIdAsync(int userId)
        {
            return await _context.Invites
                .Where(i => i.InviterId == userId)
                .Include(i => i.Couple)
                .ToListAsync();
        }

        public async Task<Invite?> GetInviteByCodeAsync(string code)
        {
            return await _context.Invites
                .FirstOrDefaultAsync(i => i.Code == code && !i.IsUsed && i.ExpiresAt > DateTime.UtcNow);
        }
    }
}
