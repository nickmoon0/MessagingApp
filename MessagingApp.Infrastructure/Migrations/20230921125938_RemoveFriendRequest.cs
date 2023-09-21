using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessagingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFriendRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendRequest");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "UserFriends",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserFriends_StatusId",
                table: "UserFriends",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFriends_RequestStatus_StatusId",
                table: "UserFriends",
                column: "StatusId",
                principalTable: "RequestStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFriends_RequestStatus_StatusId",
                table: "UserFriends");

            migrationBuilder.DropIndex(
                name: "IX_UserFriends_StatusId",
                table: "UserFriends");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "UserFriends");

            migrationBuilder.CreateTable(
                name: "FriendRequest",
                columns: table => new
                {
                    ToUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FromUserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequest", x => new { x.ToUserId, x.FromUserId });
                    table.ForeignKey(
                        name: "FK_FriendRequest_RequestStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "RequestStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequest_User_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequest_User_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequest_FromUserId",
                table: "FriendRequest",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequest_StatusId",
                table: "FriendRequest",
                column: "StatusId");
        }
    }
}
