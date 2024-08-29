using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TopicAddition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Topics",
                table: "Modules");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Modules",
                newName: "ModuleId");

            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    TopicId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.TopicId);
                    table.ForeignKey(
                        name: "FK_Topic_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Topic_ModuleId",
                table: "Topic",
                column: "ModuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Topic");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "Modules",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Topics",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
