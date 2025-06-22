using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thenestle.Domain.Models;

namespace Thenestle.Domain.Interfaces.Repositories
{
    public interface IInviteRepository
    {
        Task<Invite?> GetInviteByIdAsync(int inviteId);
        Task AddInviteAsync(Invite invite);
        Task UpdateInviteAsync(Invite invite);
        Task DeleteInviteAsync(Invite invite);

        // Дополнительно, если нужно:
        Task<ICollection<Invite>> GetInvitesAsync(int pageNumber, int pageSize);

        Task<ICollection<Invite>> GetInvitesByCodeAsync(string code);
    }
}
