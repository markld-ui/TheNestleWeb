using AutoMapper;
using Microsoft.Extensions.Logging;
using Thenestle.API.DTO.User;
using Thenestle.API.DTO.Couples;
using Thenestle.Domain.Models;

namespace Thenestle.API.Helper
{
    /// <summary>
    /// Класс для настройки маппинга между сущностями и их DTO.
    /// </summary>
    public class MappingProfiles : Profile
    {
        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MappingProfiles"/> и настраивает маппинг.
        /// </summary>
        public MappingProfiles()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<User, UpdateUserDTO>();
            CreateMap<UpdateUserDTO, User>();

            CreateMap<Couple, CoupleDTO>();
            CreateMap<CoupleDTO, Couple>();
            CreateMap<Couple, CreateCoupleDTO>();
            CreateMap<CreateCoupleDTO, Couple>();
        }

        #endregion
    }
}