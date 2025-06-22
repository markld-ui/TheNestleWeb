using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Thenestle.Domain.Interfaces.Auth;

namespace Thenestle.Application.Servicies.Auth
{
    /// <summary>
    /// Сервис для хеширования и проверки паролей.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        #region Методы

        #region Generate
        /// <summary>
        /// Генерирует хеш пароля с использованием алгоритма BCrypt.
        /// </summary>
        /// <param name="password">Пароль, который необходимо хешировать.</param>
        /// <returns>Хешированный пароль.</returns>
        public string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        #endregion

        #region EnhancedVerify
        /// <summary>
        /// Проверяет соответствие пароля его хешу с использованием алгоритма BCrypt.
        /// </summary>
        /// <param name="password">Пароль для проверки.</param>
        /// <param name="hashedPassword">Хешированный пароль для сравнения.</param>
        /// <returns>
        /// <c>true</c>, если пароль соответствует хешу, иначе <c>false</c>.
        /// </returns>
        public bool Verify(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        #endregion

        #endregion
    }
}
