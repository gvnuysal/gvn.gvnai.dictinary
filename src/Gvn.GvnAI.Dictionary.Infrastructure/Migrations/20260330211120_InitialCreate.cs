using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gvn.GvnAI.Dictionary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "domains",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_domains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "languages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NativeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Direction = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "parts_of_speech",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Abbreviation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parts_of_speech", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "registers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "words",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Lemma = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    PartOfSpeechId = table.Column<Guid>(type: "uuid", nullable: false),
                    FrequencyRank = table.Column<int>(type: "integer", nullable: true),
                    DifficultyLevel = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    IsCompound = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsIdiom = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsProperNoun = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_words", x => x.Id);
                    table.ForeignKey(
                        name: "FK_words_languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_words_parts_of_speech_PartOfSpeechId",
                        column: x => x.PartOfSpeechId,
                        principalTable: "parts_of_speech",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "etymologies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WordId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginLanguageId = table.Column<Guid>(type: "uuid", nullable: true),
                    Text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_etymologies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_etymologies_languages_OriginLanguageId",
                        column: x => x.OriginLanguageId,
                        principalTable: "languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_etymologies_words_WordId",
                        column: x => x.WordId,
                        principalTable: "words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pronunciations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WordId = table.Column<Guid>(type: "uuid", nullable: false),
                    IpaTranscription = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Variant = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsStandard = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pronunciations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pronunciations_words_WordId",
                        column: x => x.WordId,
                        principalTable: "words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "senses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WordId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenseNumber = table.Column<int>(type: "integer", nullable: false),
                    Definition = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    DefinitionShort = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RegisterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DomainId = table.Column<Guid>(type: "uuid", nullable: true),
                    FrequencyRank = table.Column<int>(type: "integer", nullable: true),
                    DifficultyLevel = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_senses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_senses_domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_senses_registers_RegisterId",
                        column: x => x.RegisterId,
                        principalTable: "registers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_senses_words_WordId",
                        column: x => x.WordId,
                        principalTable: "words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "word_relationships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceWordId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetWordId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_word_relationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_word_relationships_words_SourceWordId",
                        column: x => x.SourceWordId,
                        principalTable: "words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_word_relationships_words_TargetWordId",
                        column: x => x.TargetWordId,
                        principalTable: "words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sense_antonyms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SenseId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    SenseId2 = table.Column<Guid>(type: "uuid", nullable: false),
                    Strength = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sense_antonyms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sense_antonyms_senses_SenseId1",
                        column: x => x.SenseId1,
                        principalTable: "senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sense_antonyms_senses_SenseId2",
                        column: x => x.SenseId2,
                        principalTable: "senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sense_synonyms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SenseId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    SenseId2 = table.Column<Guid>(type: "uuid", nullable: false),
                    Strength = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sense_synonyms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sense_synonyms_senses_SenseId1",
                        column: x => x.SenseId1,
                        principalTable: "senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sense_synonyms_senses_SenseId2",
                        column: x => x.SenseId2,
                        principalTable: "senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "translations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SenseId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetLanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    TranslationText = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PartOfSpeechId = table.Column<Guid>(type: "uuid", nullable: true),
                    RegisterId = table.Column<Guid>(type: "uuid", nullable: true),
                    EquivalenceType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ConfidenceScore = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_translations_languages_TargetLanguageId",
                        column: x => x.TargetLanguageId,
                        principalTable: "languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_translations_parts_of_speech_PartOfSpeechId",
                        column: x => x.PartOfSpeechId,
                        principalTable: "parts_of_speech",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_translations_registers_RegisterId",
                        column: x => x.RegisterId,
                        principalTable: "registers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_translations_senses_SenseId",
                        column: x => x.SenseId,
                        principalTable: "senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "examples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SenseId = table.Column<Guid>(type: "uuid", nullable: false),
                    TranslationId = table.Column<Guid>(type: "uuid", nullable: true),
                    SourceText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    TargetText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Source = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_examples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_examples_senses_SenseId",
                        column: x => x.SenseId,
                        principalTable: "senses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_examples_translations_TranslationId",
                        column: x => x.TranslationId,
                        principalTable: "translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_domains_Code",
                table: "domains",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_etymologies_OriginLanguageId",
                table: "etymologies",
                column: "OriginLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_etymologies_WordId",
                table: "etymologies",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_examples_SenseId",
                table: "examples",
                column: "SenseId");

            migrationBuilder.CreateIndex(
                name: "IX_examples_TranslationId",
                table: "examples",
                column: "TranslationId");

            migrationBuilder.CreateIndex(
                name: "IX_languages_Code",
                table: "languages",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_parts_of_speech_Code",
                table: "parts_of_speech",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pronunciations_WordId",
                table: "pronunciations",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_registers_Code",
                table: "registers",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sense_antonyms_SenseId1_SenseId2",
                table: "sense_antonyms",
                columns: new[] { "SenseId1", "SenseId2" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sense_antonyms_SenseId2",
                table: "sense_antonyms",
                column: "SenseId2");

            migrationBuilder.CreateIndex(
                name: "IX_sense_synonyms_SenseId1_SenseId2",
                table: "sense_synonyms",
                columns: new[] { "SenseId1", "SenseId2" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sense_synonyms_SenseId2",
                table: "sense_synonyms",
                column: "SenseId2");

            migrationBuilder.CreateIndex(
                name: "IX_senses_DomainId",
                table: "senses",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_senses_RegisterId",
                table: "senses",
                column: "RegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_senses_WordId_SenseNumber",
                table: "senses",
                columns: new[] { "WordId", "SenseNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_translations_PartOfSpeechId",
                table: "translations",
                column: "PartOfSpeechId");

            migrationBuilder.CreateIndex(
                name: "IX_translations_RegisterId",
                table: "translations",
                column: "RegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_translations_SenseId_TargetLanguageId",
                table: "translations",
                columns: new[] { "SenseId", "TargetLanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_translations_TargetLanguageId",
                table: "translations",
                column: "TargetLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_word_relationships_SourceWordId_TargetWordId_Type",
                table: "word_relationships",
                columns: new[] { "SourceWordId", "TargetWordId", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_word_relationships_TargetWordId",
                table: "word_relationships",
                column: "TargetWordId");

            migrationBuilder.CreateIndex(
                name: "IX_words_LanguageId",
                table: "words",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_words_Lemma_LanguageId",
                table: "words",
                columns: new[] { "Lemma", "LanguageId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_words_PartOfSpeechId",
                table: "words",
                column: "PartOfSpeechId");

            migrationBuilder.CreateIndex(
                name: "IX_words_Status",
                table: "words",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "etymologies");

            migrationBuilder.DropTable(
                name: "examples");

            migrationBuilder.DropTable(
                name: "pronunciations");

            migrationBuilder.DropTable(
                name: "sense_antonyms");

            migrationBuilder.DropTable(
                name: "sense_synonyms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "word_relationships");

            migrationBuilder.DropTable(
                name: "translations");

            migrationBuilder.DropTable(
                name: "senses");

            migrationBuilder.DropTable(
                name: "domains");

            migrationBuilder.DropTable(
                name: "registers");

            migrationBuilder.DropTable(
                name: "words");

            migrationBuilder.DropTable(
                name: "languages");

            migrationBuilder.DropTable(
                name: "parts_of_speech");
        }
    }
}
