using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gvn.GvnAI.Dictionary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserApiSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClaudeApiKey",
                table: "Users",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleTranslateApiKey",
                table: "Users",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranslateProvider",
                table: "Users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "claude");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaudeApiKey",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GoogleTranslateApiKey",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TranslateProvider",
                table: "Users");
        }
    }
}
