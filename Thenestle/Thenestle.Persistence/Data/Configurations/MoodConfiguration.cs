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
    public class MoodConfiguration : IEntityTypeConfiguration<Mood>
    {
        public void Configure(EntityTypeBuilder<Mood> builder)
        {
            builder.ToTable("mood", "couple_app");

            builder.HasKey(m => m.MoodId)
                .HasName("mood_pkey");

            builder.Property(m => m.MoodId)
                .HasColumnName("mood_id")
                .ValueGeneratedOnAdd();

            builder.Property(m => m.Emoji)
                .HasColumnName("emoji")
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(m => m.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(m => m.Name)
                .IsUnique()
                .HasDatabaseName("idx_mood_name_unique");
        }
    }
}
