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
    public class MoodEntryConfiguration : IEntityTypeConfiguration<MoodEntry>
    {
        public void Configure(EntityTypeBuilder<MoodEntry> builder)
        {
            builder.ToTable("mood_entry", "couple_app");

            builder.HasKey(me => me.MoodEntryId)
                .HasName("mood_entry_pkey");

            builder.Property(me => me.MoodEntryId)
                .HasColumnName("mood_entry_id")
                .ValueGeneratedOnAdd();

            builder.Property(me => me.MoodId)
                .HasColumnName("mood_id");

            builder.Property(me => me.Comment)
                .HasColumnName("comment")
                .HasMaxLength(500);

            builder.Property(me => me.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");

            builder.Property(me => me.UserId)
                .HasColumnName("user_id");

            builder.HasOne(me => me.Mood)
                .WithMany(m => m.MoodEntries)
                .HasForeignKey(me => me.MoodId)
                .HasConstraintName("fk_mood_entry_mood");

            builder.HasOne(me => me.User)
                .WithMany(u => u.MoodEntries)
                .HasForeignKey(me => me.UserId)
                .HasConstraintName("fk_mood_entry_user");

            builder.HasIndex(me => me.UserId)
                .HasDatabaseName("idx_mood_entry_user_id");

            builder.HasIndex(me => me.CreatedAt)
                .HasDatabaseName("idx_mood_entry_created_at");
        }
    }
}
