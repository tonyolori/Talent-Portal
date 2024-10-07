using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TaskFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentModules_Modules_ModuleId",
                table: "StudentModules");

            migrationBuilder.DropIndex(
                name: "IX_StudentModules_ModuleId",
                table: "StudentModules");

            migrationBuilder.DropColumn(
                name: "Courses",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "InstructorId",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "Courses",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentModules_ModuleId",
                table: "StudentModules",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentModules_Modules_ModuleId",
                table: "StudentModules",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
