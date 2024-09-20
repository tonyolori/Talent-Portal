using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModuleStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Modules_ModuleId",
                table: "Quizzes");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModuleStatus",
                table: "Modules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ModuleStatusDes",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Modules_ModuleId",
                table: "Quizzes",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Modules_ModuleId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "ModuleStatus",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "ModuleStatusDes",
                table: "Modules");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "Quizzes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Modules_ModuleId",
                table: "Quizzes",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id");
        }
    }
}
