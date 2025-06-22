using System.ComponentModel.DataAnnotations;

namespace Thenestle.API.DTO.Auth
{
    /// <summary>
    /// Модель данных для изменения пароля пользователя
    /// </summary>
    public class ChangePasswordDTO
    {
        #region Свойства
        /// <summary>
        /// Текущий пароль пользователя
        /// </summary>
        [Required(ErrorMessage = "Текущий пароль обязателен")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// Новый пароль пользователя
        /// </summary>
        [Required(ErrorMessage = "Новый пароль обязателен")]
        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        public string NewPassword { get; set; }
        #endregion
    }

}
