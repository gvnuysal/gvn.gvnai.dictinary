using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gvn.GvnAI.Dictionary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedPartsOfSpeech : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "parts_of_speech",
                columns: new[] { "Id", "Abbreviation", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-0000-0000-0000-000000000001"), "n.", "NOUN", "İsim" },
                    { new Guid("11111111-0000-0000-0000-000000000002"), "v.", "VERB", "Fiil" },
                    { new Guid("11111111-0000-0000-0000-000000000003"), "adj.", "ADJ", "Sıfat" },
                    { new Guid("11111111-0000-0000-0000-000000000004"), "adv.", "ADV", "Zarf" },
                    { new Guid("11111111-0000-0000-0000-000000000005"), "pron.", "PRON", "Zamir" },
                    { new Guid("11111111-0000-0000-0000-000000000006"), "prep.", "PREP", "Edat" },
                    { new Guid("11111111-0000-0000-0000-000000000007"), "conj.", "CONJ", "Bağlaç" },
                    { new Guid("11111111-0000-0000-0000-000000000008"), "interj.", "INTERJ", "Ünlem" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "parts_of_speech",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "parts_of_speech",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "parts_of_speech",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "parts_of_speech",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "parts_of_speech",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "parts_of_speech",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "parts_of_speech",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "parts_of_speech",
                keyColumn: "Id",
                keyValue: new Guid("11111111-0000-0000-0000-000000000008"));
        }
    }
}
