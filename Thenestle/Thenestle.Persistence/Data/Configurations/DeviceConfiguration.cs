using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Thenestle.Domain.Models;

namespace Thenestle.Persistence.Data.Configurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.ToTable("device", "couple_app");

            builder.HasKey(d => d.DeviceId)
                .HasName("device_pkey");

            builder.Property(d => d.DeviceId)
                .HasColumnName("device_id")
                .ValueGeneratedOnAdd();

            builder.Property(d => d.DeviceType)
                .HasColumnName("device_type")
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion<string>();

            builder.Property(d => d.IsActive)
                .HasColumnName("is_active")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(d => d.LastActiveAt)
                .HasColumnName("last_active_at")
                .IsRequired()
                .HasDefaultValueSql("now()");

            builder.Property(d => d.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.HasOne(d => d.User)
                .WithMany(u => u.Devices)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_device_user")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasCheckConstraint("ck_device_last_active", "last_active_at <= now()");
        }
    }
}
