using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thenestle.Domain.Models;

namespace Thenestle.Persistence.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user", "couple_app");

            builder.HasKey(u => u.UserId)
                .HasName("user_pkey");

            builder.Property(u => u.UserId)
                .HasColumnName("user_id")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.FirstName)
                .HasColumnName("first_name")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            builder.Property(u => u.CurrencyBalance)
                .HasColumnName("currency_balance")
                .HasDefaultValue(0);

            builder.Property(u => u.Gender)
                .HasColumnName("gender")
                .HasMaxLength(10);

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");

            builder.Property(u => u.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("now()");

            builder.Property(u => u.RefreshToken)
                .HasColumnName("refresh_token")
                .IsRequired(false);

            builder.Property(u => u.RefreshTokenExpiryTime)
                .HasColumnName("refresh_token_expiry_time")
                .IsRequired(false);



            builder.HasOne(u => u.Couple)
                   .WithMany()
                   .HasForeignKey("couple_id")
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("idx_user_email");
        }
    }
}