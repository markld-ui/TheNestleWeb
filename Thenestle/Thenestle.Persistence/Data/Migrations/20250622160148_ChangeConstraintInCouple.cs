using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thenestle.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeConstraintInCouple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_couple_users",
                schema: "couple_app",
                table: "couple");

            migrationBuilder.AddCheckConstraint(
                name: "ck_couple_users",
                schema: "couple_app",
                table: "couple",
                sql: "user2_id IS NULL OR user1_id < user2_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_couple_users",
                schema: "couple_app",
                table: "couple");

            migrationBuilder.AddCheckConstraint(
                name: "ck_couple_users",
                schema: "couple_app",
                table: "couple",
                sql: "user1_id < user2_id");
        }
    }
}
