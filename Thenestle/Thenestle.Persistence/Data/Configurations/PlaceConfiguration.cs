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
    public class PlaceConfiguration : IEntityTypeConfiguration<Place>
    {
        public void Configure(EntityTypeBuilder<Place> builder)
        {
            builder.ToTable("place", "couple_app");

            builder.HasKey(p => p.PlaceId)
                .HasName("place_pkey");

            builder.Property(p => p.PlaceId)
                .HasColumnName("place_id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Latitude)
                .HasColumnName("latitude")
                .HasColumnType("decimal(9,6)")
                .IsRequired();

            builder.Property(p => p.Longitude)
                .HasColumnName("longitude")
                .HasColumnType("decimal(9,6)")
                .IsRequired();

            builder.Property(p => p.Address)
                .HasColumnName("address");

            builder.Property(p => p.Category)
                .HasColumnName("category")
                .HasMaxLength(50);

            builder.Property(p => p.Phone)
                .HasColumnName("phone")
                .HasMaxLength(20);

            builder.Property(p => p.Email)
                .HasColumnName("email")
                .HasMaxLength(100);

            builder.Property(p => p.AddedById)
                .HasColumnName("added_by_id")
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired()
                .HasDefaultValueSql("now()");

            builder.HasOne(p => p.AddedBy)
                .WithMany(u => u.Places)
                .HasForeignKey(p => p.AddedById)
                .HasConstraintName("fk_place_user")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasCheckConstraint("ck_latitude_range", "latitude BETWEEN -90 AND 90");
            builder.HasCheckConstraint("ck_longitude_range", "longitude BETWEEN -180 AND 180");
        }
    }
}
