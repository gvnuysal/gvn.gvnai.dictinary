using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gvn.GvnAI.Dictionary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedLanguages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "languages",
                columns: new[] { "Id", "Code", "Direction", "Name", "NativeName" },
                values: new object[,]
                {
                    { new Guid("22222222-0000-0000-0000-000000000001"), "en", "LTR", "English", "English" },
                    { new Guid("22222222-0000-0000-0000-000000000002"), "tr", "LTR", "Turkish", "Türkçe" },
                    { new Guid("22222222-0000-0000-0000-000000000003"), "de", "LTR", "German", "Deutsch" },
                    { new Guid("22222222-0000-0000-0000-000000000004"), "fr", "LTR", "French", "Français" },
                    { new Guid("22222222-0000-0000-0000-000000000005"), "es", "LTR", "Spanish", "Español" },
                    { new Guid("22222222-0000-0000-0000-000000000006"), "it", "LTR", "Italian", "Italiano" },
                    { new Guid("22222222-0000-0000-0000-000000000007"), "ru", "LTR", "Russian", "Русский" },
                    { new Guid("22222222-0000-0000-0000-000000000008"), "ar", "RTL", "Arabic", "العربية" },
                    { new Guid("22222222-0000-0000-0000-000000000009"), "ja", "LTR", "Japanese", "日本語" },
                    { new Guid("22222222-0000-0000-0000-00000000000a"), "zh", "LTR", "Chinese", "中文" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "languages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "languages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "languages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "languages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "languages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "languages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "languages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "languages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "languages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "languages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-00000000000a"));
        }
    }
}
