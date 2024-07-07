using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEndWebApi.Migrations
{
    /// <inheritdoc />
    public partial class sprint4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlternatingAlarms");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Alarms",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "DaysInWeek",
                table: "Alarms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Alarms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysInWeek",
                table: "Alarms");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Alarms");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Alarms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AlternatingAlarms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    DaysInWeek = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlternatingAlarms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlternatingAlarms_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlternatingAlarms_TaskId",
                table: "AlternatingAlarms",
                column: "TaskId");
        }
    }
}
