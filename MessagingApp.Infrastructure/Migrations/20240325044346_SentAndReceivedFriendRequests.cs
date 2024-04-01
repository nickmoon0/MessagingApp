using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessagingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SentAndReceivedFriendRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_User_ReceivingUserId",
                table: "FriendRequest");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_User_ReceivingUserId",
                table: "FriendRequest",
                column: "ReceivingUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequest_User_ReceivingUserId",
                table: "FriendRequest");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequest_User_ReceivingUserId",
                table: "FriendRequest",
                column: "ReceivingUserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
