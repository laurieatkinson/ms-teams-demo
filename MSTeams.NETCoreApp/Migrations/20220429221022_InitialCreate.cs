using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSTeams.NETCoreApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    AnswerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.AnswerId);
                });

            migrationBuilder.CreateTable(
                name: "QuestionList",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DocumentId = table.Column<string>(type: "TEXT", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    AnswerId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionList", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_QuestionList_Answer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answer",
                        principalColumn: "AnswerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionList_AnswerId",
                table: "QuestionList",
                column: "AnswerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionList");

            migrationBuilder.DropTable(
                name: "Answer");
        }
    }
}
