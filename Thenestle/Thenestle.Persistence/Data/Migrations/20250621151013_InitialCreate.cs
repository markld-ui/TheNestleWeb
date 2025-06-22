using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Thenestle.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "couple_app");

            migrationBuilder.CreateTable(
                name: "mood",
                schema: "couple_app",
                columns: table => new
                {
                    mood_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    emoji = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mood_pkey", x => x.mood_id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "couple_app",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_pkey", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "couple",
                schema: "couple_app",
                columns: table => new
                {
                    couple_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    user1_id = table.Column<int>(type: "integer", nullable: false),
                    user2_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("couple_pkey", x => x.couple_id);
                    table.CheckConstraint("ck_couple_users", "user1_id < user2_id");
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "couple_app",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    currency_balance = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    refresh_token = table.Column<string>(type: "text", nullable: true),
                    refresh_token_expiry_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    couple_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_user_couple_couple_id",
                        column: x => x.couple_id,
                        principalSchema: "couple_app",
                        principalTable: "couple",
                        principalColumn: "couple_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "device",
                schema: "couple_app",
                columns: table => new
                {
                    device_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    last_active_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("device_pkey", x => x.device_id);
                    table.CheckConstraint("ck_device_last_active", "last_active_at <= now()");
                    table.ForeignKey(
                        name: "fk_device_user",
                        column: x => x.user_id,
                        principalSchema: "couple_app",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "food_order",
                schema: "couple_app",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    approved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    initiator_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("food_order_pkey", x => x.order_id);
                    table.CheckConstraint("ck_order_approval_date", "approved_at >= created_at OR approved_at IS NULL");
                    table.ForeignKey(
                        name: "fk_order_initiator",
                        column: x => x.initiator_id,
                        principalSchema: "couple_app",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "invite",
                schema: "couple_app",
                columns: table => new
                {
                    invite_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    couple_id = table.Column<int>(type: "integer", nullable: false),
                    inviter_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("invite_pkey", x => x.invite_id);
                    table.CheckConstraint("ck_invite_expiration", "expires_at > created_at");
                    table.ForeignKey(
                        name: "fk_invite_couple",
                        column: x => x.couple_id,
                        principalSchema: "couple_app",
                        principalTable: "couple",
                        principalColumn: "couple_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invite_inviter",
                        column: x => x.inviter_id,
                        principalSchema: "couple_app",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mood_entry",
                schema: "couple_app",
                columns: table => new
                {
                    mood_entry_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mood_id = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mood_entry_pkey", x => x.mood_entry_id);
                    table.ForeignKey(
                        name: "fk_mood_entry_mood",
                        column: x => x.mood_id,
                        principalSchema: "couple_app",
                        principalTable: "mood",
                        principalColumn: "mood_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mood_entry_user",
                        column: x => x.user_id,
                        principalSchema: "couple_app",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "place",
                schema: "couple_app",
                columns: table => new
                {
                    place_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    latitude = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    longitude = table.Column<decimal>(type: "numeric(9,6)", nullable: false),
                    address = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    added_by_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("place_pkey", x => x.place_id);
                    table.CheckConstraint("ck_latitude_range", "latitude BETWEEN -90 AND 90");
                    table.CheckConstraint("ck_longitude_range", "longitude BETWEEN -180 AND 180");
                    table.ForeignKey(
                        name: "fk_place_user",
                        column: x => x.added_by_id,
                        principalSchema: "couple_app",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reminder",
                schema: "couple_app",
                columns: table => new
                {
                    reminder_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    remind_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "active"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("reminder_pkey", x => x.reminder_id);
                    table.CheckConstraint("ck_reminder_date", "remind_at > created_at");
                    table.ForeignKey(
                        name: "fk_reminder_user",
                        column: x => x.created_by_id,
                        principalSchema: "couple_app",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                schema: "couple_app",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_role_pkey", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_role_role",
                        column: x => x.role_id,
                        principalSchema: "couple_app",
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_role_user",
                        column: x => x.user_id,
                        principalSchema: "couple_app",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "currency_transaction",
                schema: "couple_app",
                columns: table => new
                {
                    transaction_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    order_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("currency_transaction_pkey", x => x.transaction_id);
                    table.CheckConstraint("ck_transaction_amount", "amount != 0");
                    table.ForeignKey(
                        name: "fk_transaction_order",
                        column: x => x.order_id,
                        principalSchema: "couple_app",
                        principalTable: "food_order",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_transaction_user",
                        column: x => x.user_id,
                        principalSchema: "couple_app",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reminder_notification",
                schema: "couple_app",
                columns: table => new
                {
                    notification_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    days_before = table.Column<int>(type: "integer", nullable: false),
                    is_sent = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reminder_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("reminder_notification_pkey", x => x.notification_id);
                    table.CheckConstraint("ck_notification_days", "days_before IN (1, 3, 7)");
                    table.ForeignKey(
                        name: "fk_notification_reminder",
                        column: x => x.reminder_id,
                        principalSchema: "couple_app",
                        principalTable: "reminder",
                        principalColumn: "reminder_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notification_user",
                        column: x => x.user_id,
                        principalSchema: "couple_app",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "idx_couple_users_unique",
                schema: "couple_app",
                table: "couple",
                columns: new[] { "user1_id", "user2_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_couple_user2_id",
                schema: "couple_app",
                table: "couple",
                column: "user2_id");

            migrationBuilder.CreateIndex(
                name: "idx_transaction_order_unique",
                schema: "couple_app",
                table: "currency_transaction",
                column: "order_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_transaction_user",
                schema: "couple_app",
                table: "currency_transaction",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_device_user_id",
                schema: "couple_app",
                table: "device",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_food_order_initiator",
                schema: "couple_app",
                table: "food_order",
                column: "initiator_id");

            migrationBuilder.CreateIndex(
                name: "idx_food_order_status",
                schema: "couple_app",
                table: "food_order",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_invite_code_unique",
                schema: "couple_app",
                table: "invite",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_invite_couple_id",
                schema: "couple_app",
                table: "invite",
                column: "couple_id");

            migrationBuilder.CreateIndex(
                name: "IX_invite_inviter_id",
                schema: "couple_app",
                table: "invite",
                column: "inviter_id");

            migrationBuilder.CreateIndex(
                name: "idx_mood_name_unique",
                schema: "couple_app",
                table: "mood",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_mood_entry_created_at",
                schema: "couple_app",
                table: "mood_entry",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "idx_mood_entry_user_id",
                schema: "couple_app",
                table: "mood_entry",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_mood_entry_mood_id",
                schema: "couple_app",
                table: "mood_entry",
                column: "mood_id");

            migrationBuilder.CreateIndex(
                name: "IX_place_added_by_id",
                schema: "couple_app",
                table: "place",
                column: "added_by_id");

            migrationBuilder.CreateIndex(
                name: "idx_reminder_created_by",
                schema: "couple_app",
                table: "reminder",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "idx_reminder_remind_at",
                schema: "couple_app",
                table: "reminder",
                column: "remind_at");

            migrationBuilder.CreateIndex(
                name: "idx_notification_reminder_status",
                schema: "couple_app",
                table: "reminder_notification",
                columns: new[] { "reminder_id", "is_sent" });

            migrationBuilder.CreateIndex(
                name: "IX_reminder_notification_user_id",
                schema: "couple_app",
                table: "reminder_notification",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_role_name_unique",
                schema: "couple_app",
                table: "role",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_user_email",
                schema: "couple_app",
                table: "user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_couple_id",
                schema: "couple_app",
                table: "user",
                column: "couple_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_role_id",
                schema: "couple_app",
                table: "user_role",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "fk_couple_user1",
                schema: "couple_app",
                table: "couple",
                column: "user1_id",
                principalSchema: "couple_app",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_couple_user2",
                schema: "couple_app",
                table: "couple",
                column: "user2_id",
                principalSchema: "couple_app",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_couple_user1",
                schema: "couple_app",
                table: "couple");

            migrationBuilder.DropForeignKey(
                name: "fk_couple_user2",
                schema: "couple_app",
                table: "couple");

            migrationBuilder.DropTable(
                name: "currency_transaction",
                schema: "couple_app");

            migrationBuilder.DropTable(
                name: "device",
                schema: "couple_app");

            migrationBuilder.DropTable(
                name: "invite",
                schema: "couple_app");

            migrationBuilder.DropTable(
                name: "mood_entry",
                schema: "couple_app");

            migrationBuilder.DropTable(
                name: "place",
                schema: "couple_app");

            migrationBuilder.DropTable(
                name: "reminder_notification",
                schema: "couple_app");

            migrationBuilder.DropTable(
                name: "user_role",
                schema: "couple_app");

            migrationBuilder.DropTable(
                name: "food_order",
                schema: "couple_app");

            migrationBuilder.DropTable(
                name: "mood",
                schema: "couple_app");

            migrationBuilder.DropTable(
                name: "reminder",
                schema: "couple_app");

            migrationBuilder.DropTable(
                name: "role",
                schema: "couple_app");

            migrationBuilder.DropTable(
                name: "user",
                schema: "couple_app");

            migrationBuilder.DropTable(
                name: "couple",
                schema: "couple_app");
        }
    }
}
