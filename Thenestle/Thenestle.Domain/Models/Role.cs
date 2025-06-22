using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thenestle.Domain.Models
{
    /// <summary>
    /// Класс, представляющий сущность "Роль".
    /// </summary>
    public class Role
    {
        #region Свойства

        /// <summary>
        /// Идентификатор роли.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Коллекция связей между ролями и пользователями (Many-to-Many: Role <-> Users).
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        #endregion
    }
}
