using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModuleInstructorFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FacilitatorName",
                table: "Modules",
                newName: "InstructorName");

            migrationBuilder.RenameColumn(
                name: "FacilitatorId",
                table: "Modules",
                newName: "InstructorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InstructorName",
                table: "Modules",
                newName: "FacilitatorName");

            migrationBuilder.RenameColumn(
                name: "InstructorId",
                table: "Modules",
                newName: "FacilitatorId");
        }
    }
}
