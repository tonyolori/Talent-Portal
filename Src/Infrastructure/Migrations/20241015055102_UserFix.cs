using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_AspNetUsers_StudentId",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_StudentId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "ApplicationType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StatusDes",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Student_EnrollmentDate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ApplicationType",
                table: "Transactions",
                newName: "PaymentType");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Tasks",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_StudentId",
                table: "Tasks",
                newName: "IX_Tasks_UserId");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Modules",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_StudentId",
                table: "Modules",
                newName: "IX_Modules_UserId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "AspNetUsers",
                newName: "PaymentType");

            migrationBuilder.RenameColumn(
                name: "RoleDesc",
                table: "AspNetUsers",
                newName: "UserTypeDesc");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "AspNetUsers",
                newName: "UserType");

            migrationBuilder.AlterColumn<string>(
                name: "UserStatusDes",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserStatus",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnrollmentDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(128)", 
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_AspNetUsers_UserId",
                table: "Modules",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_AspNetUsers_UserId",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "Transactions",
                newName: "ApplicationType");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Tasks",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                newName: "IX_Tasks_StudentId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Modules",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_UserId",
                table: "Modules",
                newName: "IX_Modules_StudentId");

            migrationBuilder.RenameColumn(
                name: "UserTypeDesc",
                table: "AspNetUsers",
                newName: "RoleDesc");

            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "AspNetUsers",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "AspNetUsers",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "Progress",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserStatusDes",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "UserStatus",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EnrollmentDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationType",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusDes",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Student_EnrollmentDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_AspNetUsers_StudentId",
                table: "Modules",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_StudentId",
                table: "Tasks",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
           
        }
    }
}
