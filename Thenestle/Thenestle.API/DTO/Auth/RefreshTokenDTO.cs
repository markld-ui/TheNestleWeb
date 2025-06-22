namespace Thenestle.API.DTO.Auth
{
    #region Класс RefreshTokenDTO
    /// <summary>
    /// Data Transfer Object (DTO) для передачи данных о токене и refresh-токене.
    /// </summary>
    public class RefreshTokenDTO
    {
        #region Свойства

        /// <summary>
        /// JWT-токен, который требуется обновить.
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public string Token { get; set; }

        /// <summary>
        /// Refresh-токен, используемый для обновления JWT-токена.
        /// </summary>
        /// <example>def50200b4a9c4a2a2f6f8...</example>
        public string RefreshToken { get; set; }

        #endregion
    }
    #endregion
}
