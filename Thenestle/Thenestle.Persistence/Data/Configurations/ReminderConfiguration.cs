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
    public class ReminderConfiguration : IEntityTypeConfiguration<Reminder>
    {
        public void Configure(EntityTypeBuilder<Reminder> builder)
        {
            builder.ToTable("reminder", "couple_app");

            builder.HasKey(r => r.ReminderId)
                .HasName("reminder_pkey");

            builder.Property(r => r.ReminderId)
                .HasColumnName("reminder_id")
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Title)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.RemindAt)
                .HasColumnName("remind_at")
                .IsRequired();

            builder.Property(r => r.Type)
                .HasColumnName("type")
                .IsRequired()
                .HasMaxLength(20)
                .HasConversion<string>();

            builder.Property(r => r.Status)
                .HasColumnName("status")
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("active")
                .HasConversion<string>();

            builder.Property(r => r.CreatedById)
                .HasColumnName("created_by_id")
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("now()");

            builder.Property(r => r.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired()
                .HasDefaultValueSql("now()");

            builder.HasOne(r => r.CreatedBy)
                .WithMany(u => u.Reminders)
                .HasForeignKey(r => r.CreatedById)
                .HasConstraintName("fk_reminder_user")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasCheckConstraint("ck_reminder_date", "remind_at > created_at");

            builder.HasIndex(r => r.CreatedById)
                .HasDatabaseName("idx_reminder_created_by");

            builder.HasIndex(r => r.RemindAt)
                .HasDatabaseName("idx_reminder_remind_at");
        }
    }
}
