using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SubmissionDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmissionDate",
                table: "Tasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmissionDate",
                table: "SubmissionDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaskStatusDesc",
                table: "SubmissionDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmissionDate",
                table: "SubmissionDetails");

            migrationBuilder.DropColumn(
                name: "TaskStatusDesc",
                table: "SubmissionDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmissionDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
