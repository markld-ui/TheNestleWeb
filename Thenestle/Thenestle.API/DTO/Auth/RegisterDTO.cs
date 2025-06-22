using System.ComponentModel.DataAnnotations;

namespace Thenestle.API.DTO.Auth
{
    #region Класс RegisterDTO
    /// <summary>
    /// Data Transfer Object (DTO) для передачи данных, необходимых для регистрации пользователя.
    /// </summary>
    public class RegisterDTO
    {
        #region Свойства

        /// <summary>
        /// Имя пользователя. Используется для идентификации пользователя в системе.
        /// </summary>
        /// <example>user123</example>
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [MinLength(3, ErrorMessage = "Имя пользователя должно содержать минимум 3 символа")]
        [MaxLength(20, ErrorMessage = "Имя пользователя не должно превышать 20 символов")]
        public string FirstName { get; set; }

        /// <summary>
        /// Имя пользователя. Используется для идентификации пользователя в системе.
        /// </summary>
        /// <example>user123</example>
        [Required(ErrorMessage = "Фамилия пользователя обязательна")]
        [MinLength(3, ErrorMessage = "Фамилия пользователя должна содержать минимум 3 символа")]
        [MaxLength(20, ErrorMessage = "Фамилия пользователя не должна превышать 20 символов")]
        public string LastName { get; set; }

        /// <summary>
        /// Электронная почта пользователя. Используется для идентификации и связи с пользователем.
        /// </summary> 
        /// <example>user@example.com</example>
        [Required(ErrorMessage = "Почта обязательна")]
        [EmailAddress(ErrorMessage = "Введите корректный адрес электронной почты")]
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя. Используется для аутентификации.
        /// </summary>
        /// <example>Pa$$w0rd</example>
        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(8, ErrorMessage = "Пароль должен содержать минимум 8 символов")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
                ErrorMessage = "Пароль должен содержать цифры, заглавные и строчные буквы")]
        public string Password { get; set; }

        /// <summary>
        /// Гендер пользователя.
        /// </summary> 
        [Required(ErrorMessage = "Гендер обязателен")]
        public string Gender { get; set; }

        #endregion
    }

    #endregion
}
