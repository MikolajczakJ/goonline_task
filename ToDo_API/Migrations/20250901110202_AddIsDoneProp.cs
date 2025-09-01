using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo_API.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDoneProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "ToDos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "ToDos");
        }
    }
}
