using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thenestle.Domain.Models;

namespace Thenestle.Persistence.Data.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        /// <summary>
        /// Настраивает сущность <see cref="UserRole"/> и ее маппинг в базе данных.
        /// </summary>
        /// <param name="builder">Объект <see cref="EntityTypeBuilder{UserRole}"/> для настройки сущности.</param>
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_role", "couple_app");

            builder.HasKey(ur => new { ur.UserId, ur.RoleId })
                .HasName("user_role_pkey");

            builder.Property(ur => ur.UserId)
                .HasColumnName("user_id");

            builder.Property(ur => ur.RoleId)
                .HasColumnName("role_id");

            builder.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .HasConstraintName("fk_user_role_user")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .HasConstraintName("fk_user_role_role")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
