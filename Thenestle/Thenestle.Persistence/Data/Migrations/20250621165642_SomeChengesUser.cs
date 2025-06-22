using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thenestle.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class SomeChengesUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_couple_couple_id",
                schema: "couple_app",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_couple_id",
                schema: "couple_app",
                table: "user");

            migrationBuilder.AddColumn<int>(
                name: "couple_id1",
                schema: "couple_app",
                table: "user",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_couple_id1",
                schema: "couple_app",
                table: "user",
                column: "couple_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_user_couple_couple_id1",
                schema: "couple_app",
                table: "user",
                column: "couple_id1",
                principalSchema: "couple_app",
                principalTable: "couple",
                principalColumn: "couple_id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_couple_couple_id1",
                schema: "couple_app",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_couple_id1",
                schema: "couple_app",
                table: "user");

            migrationBuilder.DropColumn(
                name: "couple_id1",
                schema: "couple_app",
                table: "user");

            migrationBuilder.CreateIndex(
                name: "IX_user_couple_id",
                schema: "couple_app",
                table: "user",
                column: "couple_id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_couple_couple_id",
                schema: "couple_app",
                table: "user",
                column: "couple_id",
                principalSchema: "couple_app",
                principalTable: "couple",
                principalColumn: "couple_id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
