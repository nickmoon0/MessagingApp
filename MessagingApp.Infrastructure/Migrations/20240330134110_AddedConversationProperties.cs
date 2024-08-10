using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessagingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedConversationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Conversation",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Conversation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Conversation");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Conversation");
        }
    }
}
