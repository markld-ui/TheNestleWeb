using System.ComponentModel.DataAnnotations;

namespace Thenestle.API.DTO.Auth
{
    #region Класс LoginDTO
    /// <summary>
    /// Data Transfer Object (DTO) для передачи данных, необходимых для аутентификации пользователя.
    /// </summary>
    public class LoginDTO
    {
        #region Свойства

        /// <summary>
        /// Электронная почта пользователя. Используется для идентификации пользователя.
        /// </summary>
        /// <example>user@example.com</example>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя. Используется для проверки подлинности.
        /// </summary>
        /// <example>Pa$$w0rd</example>
        [Required]
        public string Password { get; set; }

        #endregion
    }
    #endregion
}
