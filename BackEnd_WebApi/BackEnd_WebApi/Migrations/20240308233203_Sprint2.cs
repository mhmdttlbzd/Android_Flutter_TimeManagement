using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackEndWebApi.Migrations
{
    /// <inheritdoc />
    public partial class Sprint2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForgotPassword",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationTaskId",
                table: "TimeHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationTaskId",
                table: "TimeHistories",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "مطالعه" },
                    { 2, "اوقات فراغت" },
                    { 3, "ورزش" },
                    { 4, "کار" },
                    { 5, "آرامش" },
                    { 7, "نظافت" },
                    { 8, "خرید" },
                    { 9, "سرگرمی" },
                    { 10, "خانواده" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeHistories_ApplicationTaskId",
                table: "TimeHistories",
                column: "ApplicationTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeHistories_Tasks_ApplicationTaskId",
                table: "TimeHistories",
                column: "ApplicationTaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeHistories_Tasks_ApplicationTaskId",
                table: "TimeHistories");

            migrationBuilder.DropIndex(
                name: "IX_TimeHistories_ApplicationTaskId",
                table: "TimeHistories");

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DropColumn(
                name: "ForgotPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ApplicationTaskId",
                table: "TimeHistories");

            migrationBuilder.DropColumn(
                name: "ApplicationTaskId",
                table: "TimeHistories");
        }
    }
}
