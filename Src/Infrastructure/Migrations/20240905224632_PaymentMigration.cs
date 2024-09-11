using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PaymentMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Answers_CorrectOptionId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CorrectOptionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CorrectOptionId",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "Quizzes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "Quizzes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredProgramme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationalLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmploymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_ModuleId",
                table: "Quizzes",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_StudentId",
                table: "Quizzes",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_AspNetUsers_StudentId",
                table: "Quizzes",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Modules_ModuleId",
                table: "Quizzes",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_AspNetUsers_StudentId",
                table: "Quizzes");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Modules_ModuleId",
                table: "Quizzes");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_ModuleId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_StudentId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "Answers");

            migrationBuilder.AddColumn<int>(
                name: "CorrectOptionId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CorrectOptionId",
                table: "Questions",
                column: "CorrectOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Answers_CorrectOptionId",
                table: "Questions",
                column: "CorrectOptionId",
                principalTable: "Answers",
                principalColumn: "Id");
        }
    }
}
