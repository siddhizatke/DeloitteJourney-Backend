using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class TableCreation3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AboutMe", "Name", "PhotosUrl", "ProfilePictureUrl" },
                values: new object[] { 1, "A passionate developer.", "Siddhi Zatke", "[\"\\u0022C:\\\\Users\\\\szatke\\\\Pictures\\\\New Photo.jpg\\u0022\",\"\\u0022C:\\\\Users\\\\szatke\\\\Pictures\\\\photo.jpg\\u0022\"]", "\"C:\\Users\\szatke\\Pictures\\New Photo.jpg\"" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
