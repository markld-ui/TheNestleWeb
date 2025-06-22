using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenestle.Domain.Models
{
    /// <summary>
    /// Класс, представляющий сущность "Связь пользователя и роли".
    /// </summary>
    public class UserRole
    {
        #region Свойства

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Пользователь, связанный с ролью.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Идентификатор роли.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Роль, связанная с пользователем.
        /// </summary>
        public Role Role { get; set; }

        #endregion
    }
}
