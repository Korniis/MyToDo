using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyToDo.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCreateDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatDate",
                table: "User",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CreatDate",
                table: "ToDo",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CreatDate",
                table: "Memo",
                newName: "CreateDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "User",
                newName: "CreatDate");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "ToDo",
                newName: "CreatDate");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Memo",
                newName: "CreatDate");
        }
    }
}
