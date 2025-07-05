using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thenestle.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeConstraintInCouple2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_couple_users_unique",
                schema: "couple_app",
                table: "couple");

            migrationBuilder.DropCheckConstraint(
                name: "ck_couple_users",
                schema: "couple_app",
                table: "couple");

            migrationBuilder.CreateIndex(
                name: "idx_couple_users_unique",
                schema: "couple_app",
                table: "couple",
                columns: new[] { "user1_id", "user2_id" },
                unique: true,
                filter: "user2_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_couple_user1_id",
                schema: "couple_app",
                table: "couple",
                column: "user1_id");

            migrationBuilder.AddCheckConstraint(
                name: "ck_couple_users",
                schema: "couple_app",
                table: "couple",
                sql: "user2_id IS NULL OR user1_id != user2_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_couple_users_unique",
                schema: "couple_app",
                table: "couple");

            migrationBuilder.DropIndex(
                name: "IX_couple_user1_id",
                schema: "couple_app",
                table: "couple");

            migrationBuilder.DropCheckConstraint(
                name: "ck_couple_users",
                schema: "couple_app",
                table: "couple");

            migrationBuilder.CreateIndex(
                name: "idx_couple_users_unique",
                schema: "couple_app",
                table: "couple",
                columns: new[] { "user1_id", "user2_id" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_couple_users",
                schema: "couple_app",
                table: "couple",
                sql: "user2_id IS NULL OR user1_id < user2_id");
        }
    }
}
