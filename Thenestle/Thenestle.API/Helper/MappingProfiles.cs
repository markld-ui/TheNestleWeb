using AutoMapper;
using Microsoft.Extensions.Logging;
using Thenestle.API.DTO.User;
using Thenestle.API.DTO.Couples;
using Thenestle.API.DTO.Invites;
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
            CreateMap<Users, UserDTO>();
            CreateMap<UserDTO, Users>();

            CreateMap<Users, UpdateUserDTO>();
            CreateMap<UpdateUserDTO, Users>();

            CreateMap<Couple, CoupleDTO>();
            CreateMap<CoupleDTO, Couple>();
            CreateMap<Couple, CreateCoupleDTO>();
            CreateMap<CreateCoupleDTO, Couple>();

            CreateMap<Invite, InviteDTO>();
            CreateMap<Invite, AcceptInviteDTO>();
            CreateMap<Invite, CreateInviteDTO>();
        }

        #endregion
    }
}