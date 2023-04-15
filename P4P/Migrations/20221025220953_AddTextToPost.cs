using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P4P.Migrations
{
    public partial class AddTextToPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Post",
                newName: "Text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Post",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Post",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
