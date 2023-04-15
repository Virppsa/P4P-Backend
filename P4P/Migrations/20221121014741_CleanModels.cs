using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P4P.Migrations
{
    public partial class CleanModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Location_LocationId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Stars",
                table: "Post");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Post",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Location_LocationId",
                table: "Post",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Location_LocationId",
                table: "Post");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Post",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "Stars",
                table: "Post",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Location_LocationId",
                table: "Post",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");
        }
    }
}
