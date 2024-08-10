using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessagingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedFriendRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_User_ReceivingUserId",
                table: "FriendRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_User_UserId",
                table: "FriendRequest");

            migrationBuilder.DropIndex(
                name: "IX_FriendRequest_UserId",
                table: "FriendRequest");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FriendRequest");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_User_ReceivingUserId",
                table: "FriendRequest",
                column: "ReceivingUserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_User_ReceivingUserId",
                table: "FriendRequest");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "FriendRequest",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequest_UserId",
                table: "FriendRequest",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_User_ReceivingUserId",
                table: "FriendRequest",
                column: "ReceivingUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_User_UserId",
                table: "FriendRequest",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
