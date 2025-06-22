using Thenestle.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Thenestle.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для работы с сущностью <see cref="Users"/>
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Получить пользователей с пагинацией и фильтрацией
        /// </summary>
        Task<(ICollection<Users> Users, int TotalCount)> GetUsersAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string sortField = "UserId",
        bool ascending = true);

        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        Task<Users?> GetUserByIdAsync(int id);

        /// <summary>
        /// Получить пользователя по email
        /// </summary>
        Task<Users?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Проверить существование пользователя по ID
        /// </summary>
        Task<bool> UserExistsByIdAsync(int id);

        /// <summary>
        /// Проверить существование пользователя по email
        /// </summary>
        Task<bool> UserExistsByEmailAsync(string email);

        /// <summary>
        /// Добавить нового пользователя
        /// </summary>
        Task AddUserAsync(Users user);

        /// <summary>
        /// Обновить пользователя
        /// </summary>
        Task UpdateUserAsync(Users user);

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        Task DeleteUserAsync(Users user);

        /// <summary>
        /// Обновить рефреш-токен пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refreshToken"></param>
        /// <param name="refreshTokenExpiryTime"></param>
        /// <returns></returns>
        Task UpdateUserRefreshTokenAsync(int userId, string? refreshToken, DateTime? refreshTokenExpiryTime);

        /// <summary>
        /// Получить рефреш-токен пользователя
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<Users?> GetUserByRefreshTokenAsync(string refreshToken);
    }
}