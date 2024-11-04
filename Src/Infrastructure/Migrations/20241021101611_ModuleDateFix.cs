using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModuleDateFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Programmes_ProgrammeId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Timeframe",
                table: "Modules",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Programmes_ProgrammeId",
                table: "AspNetUsers",
                column: "ProgrammeId",
                principalTable: "Programmes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Programmes_ProgrammeId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Timeframe",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Programmes_ProgrammeId",
                table: "AspNetUsers",
                column: "ProgrammeId",
                principalTable: "Programmes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
