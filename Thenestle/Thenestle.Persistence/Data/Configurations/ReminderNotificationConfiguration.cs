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
    public class ReminderNotificationConfiguration : IEntityTypeConfiguration<ReminderNotification>
    {
        public void Configure(EntityTypeBuilder<ReminderNotification> builder)
        {
            builder.ToTable("reminder_notification", "couple_app");

            builder.HasKey(rn => rn.NotificationId)
                .HasName("reminder_notification_pkey");

            builder.Property(rn => rn.NotificationId)
                .HasColumnName("notification_id")
                .ValueGeneratedOnAdd();

            builder.Property(rn => rn.DaysBefore)
                .HasColumnName("days_before")
                .IsRequired();

            builder.Property(rn => rn.IsSent)
                .HasColumnName("is_sent")
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(rn => rn.SentAt)
                .HasColumnName("sent_at");

            builder.Property(rn => rn.ReminderId)
                .HasColumnName("reminder_id")
                .IsRequired();

            builder.Property(rn => rn.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.HasOne(rn => rn.Reminder)
                .WithMany(r => r.Notifications)
                .HasForeignKey(rn => rn.ReminderId)
                .HasConstraintName("fk_notification_reminder")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rn => rn.User)
                .WithMany()
                .HasForeignKey(rn => rn.UserId)
                .HasConstraintName("fk_notification_user")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasCheckConstraint("ck_notification_days", "days_before IN (1, 3, 7)");

            builder.HasIndex(rn => new { rn.ReminderId, rn.IsSent })
                .HasDatabaseName("idx_notification_reminder_status");
        }
    }
}
