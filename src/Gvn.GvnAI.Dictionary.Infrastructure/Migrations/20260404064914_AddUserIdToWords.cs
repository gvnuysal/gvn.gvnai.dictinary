using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gvn.GvnAI.Dictionary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_words_Lemma_LanguageId",
                table: "words");

            // Step 1: Add column as nullable
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "words",
                type: "uuid",
                nullable: true);

            // Step 2: Assign existing words to the first user
            migrationBuilder.Sql(
                """
                UPDATE words SET "UserId" = (SELECT "Id" FROM "Users" ORDER BY "CreatedAt" LIMIT 1)
                WHERE "UserId" IS NULL;
                """);

            // Step 3: Make column non-nullable
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "words",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_words_Lemma_LanguageId_UserId",
                table: "words",
                columns: new[] { "Lemma", "LanguageId", "UserId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_words_UserId",
                table: "words",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_words_Users_UserId",
                table: "words",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_words_Users_UserId",
                table: "words");

            migrationBuilder.DropIndex(
                name: "IX_words_Lemma_LanguageId_UserId",
                table: "words");

            migrationBuilder.DropIndex(
                name: "IX_words_UserId",
                table: "words");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "words");

            migrationBuilder.CreateIndex(
                name: "IX_words_Lemma_LanguageId",
                table: "words",
                columns: new[] { "Lemma", "LanguageId" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }
    }
}
