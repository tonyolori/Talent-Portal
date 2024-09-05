using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuestionMigration : Migration
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

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
