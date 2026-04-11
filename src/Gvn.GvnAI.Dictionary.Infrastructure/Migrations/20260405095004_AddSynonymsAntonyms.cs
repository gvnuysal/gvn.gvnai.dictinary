using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gvn.GvnAI.Dictionary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSynonymsAntonyms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Antonyms",
                table: "words",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Synonyms",
                table: "words",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Antonyms",
                table: "words");

            migrationBuilder.DropColumn(
                name: "Synonyms",
                table: "words");
        }
    }
}
