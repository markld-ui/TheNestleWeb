using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thenestle.Domain.Models;

namespace Thenestle.Domain.Interfaces.Repositories
{
    public interface ICoupleRepository
    {
        /// <summary>
        /// Получить пары с пагинацией и фильтрацией
        /// </summary>
        Task<(ICollection<Couple> Couples, int TotalCount)> GetCouplesAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string sortField = "CoupleId",
        bool ascending = true);

        /// <summary>
        /// Получить пару по ID
        /// </summary>
        Task<Couple?> GetCoupleByIdAsync(int id);

        /// <summary>
        /// Проверить существование пары по ID
        /// </summary>
        Task<bool> CoupleExistsByIdAsync(int id);

        /// <summary>
        /// Добавить новой пары
        /// </summary>
        Task AddCoupleAsync(Couple couple);

        /// <summary>
        /// Обновляет пару
        /// </summary>
        Task UpdateCoupleAsync(Couple couple);

        /// <summary>
        /// Удалить пару
        /// </summary>
        Task DeleteCoupleAsync(Couple couple);

        Task<Couple?> GetCoupleByUserIdAsync(int userId);
    }
}
