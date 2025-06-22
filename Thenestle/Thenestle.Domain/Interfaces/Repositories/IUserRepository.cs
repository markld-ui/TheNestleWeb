using Thenestle.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Thenestle.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для работы с сущностью <see cref="User"/>
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Получить пользователей с пагинацией и фильтрацией
        /// </summary>
        Task<(ICollection<User> Users, int TotalCount)> GetUsersAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string sortField = "UserId",
        bool ascending = true);

        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        Task<User?> GetUserByIdAsync(int id);

        /// <summary>
        /// Получить пользователя по email
        /// </summary>
        Task<User?> GetUserByEmailAsync(string email);

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
        Task AddUserAsync(User user);

        /// <summary>
        /// Обновить пользователя
        /// </summary>
        Task UpdateUserAsync(User user);

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        Task DeleteUserAsync(User user);

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
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    }
}