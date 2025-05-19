using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class AddProfilePictureBase64ToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureBase64",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureBase64",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AboutMe", "AboutMeFormal", "Name", "ProfilePictureUrl" },
                values: new object[] { 1, "A passionate developer.", "A passionate developer with a keen interest in software development and technology.", "Siddhi Zatke", "\"C:\\Users\\szatke\\Pictures\\New Photo.jpg\"" });
        }
    }
}
