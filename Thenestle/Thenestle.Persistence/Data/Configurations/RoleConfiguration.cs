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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        /// <summary>
        /// Настраивает сущность <see cref="Role"/> и ее маппинг в базе данных.
        /// </summary>
        /// <param name="builder">Объект <see cref="EntityTypeBuilder{Role}"/> для настройки сущности.</param>
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("role", "couple_app");

            builder.HasKey(r => r.RoleId)
                .HasName("role_pkey");

            builder.Property(r => r.RoleId)
                .HasColumnName("role_id")
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(r => r.Name)
                .IsUnique()
                .HasDatabaseName("idx_role_name_unique");
        }
    }
}
