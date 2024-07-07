using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEndWebApi.Migrations
{
    /// <inheritdoc />
    public partial class sprint5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Usernames",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Usernames",
                table: "Tasks");
        }
    }
}
