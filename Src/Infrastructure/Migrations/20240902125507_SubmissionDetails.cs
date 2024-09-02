using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SubmissionDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacilitatorFeedBack",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "SubmissionLink",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "Modules",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubmissionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grade = table.Column<float>(type: "real", nullable: true),
                    SubmissionLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacilitatorFeedBack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskStatus = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_StudentId",
                table: "Modules",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionDetails_TaskId_StudentId",
                table: "SubmissionDetails",
                columns: new[] { "TaskId", "StudentId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_AspNetUsers_StudentId",
                table: "Modules",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_AspNetUsers_StudentId",
                table: "Modules");

            migrationBuilder.DropTable(
                name: "SubmissionDetails");

            migrationBuilder.DropIndex(
                name: "IX_Modules_StudentId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Modules");

            migrationBuilder.AddColumn<string>(
                name: "FacilitatorFeedBack",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SubmissionLink",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
