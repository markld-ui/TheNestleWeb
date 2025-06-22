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
    public class InviteConfiguration : IEntityTypeConfiguration<Invite>
    {
        public void Configure(EntityTypeBuilder<Invite> builder)
        {
            builder.ToTable("invite", "couple_app");

            builder.HasKey(i => i.InviteId)
                .HasName("invite_pkey");

            builder.Property(i => i.InviteId)
                .HasColumnName("invite_id")
                .ValueGeneratedOnAdd();

            builder.Property(i => i.Code)
                .HasColumnName("code")
                .IsRequired()
                .HasMaxLength(8);

            builder.Property(i => i.IsUsed)
                .HasColumnName("is_used")
                .HasDefaultValue(false);

            builder.Property(i => i.Status)
                .HasColumnName("status")
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("pending");

            builder.Property(i => i.ExpiresAt)
                .HasColumnName("expires_at");

            builder.Property(i => i.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");

            builder.Property(i => i.CoupleId)
                .HasColumnName("couple_id");

            builder.Property(i => i.InviterId)
                .HasColumnName("inviter_id");

            builder.HasOne(i => i.Couple)
                .WithMany(c => c.Invites)
                .HasForeignKey(i => i.CoupleId)
                .HasConstraintName("fk_invite_couple");

            builder.HasOne(i => i.Inviter)
                .WithMany()
                .HasForeignKey(i => i.InviterId)
                .HasConstraintName("fk_invite_inviter");

            builder.HasIndex(i => i.Code)
                .IsUnique()
                .HasDatabaseName("idx_invite_code_unique");

            builder.HasCheckConstraint("ck_invite_expiration", "expires_at > created_at");
        }
    }
}
