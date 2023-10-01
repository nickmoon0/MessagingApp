using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessagingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRequestingUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestingUserId",
                table: "FriendRequest");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RequestingUserId",
                table: "FriendRequest",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");
        }
    }
}
