using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class minimalchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "UserPhotos");

            migrationBuilder.RenameColumn(
                name: "TrainingImageUrl",
                table: "TrainingSelfies",
                newName: "TrainingImageBase64");

            migrationBuilder.RenameColumn(
                name: "TeamImageUrl",
                table: "TeamSelfies",
                newName: "TeamImageBase64");

            migrationBuilder.AddColumn<string>(
                name: "PhotoBase64",
                table: "UserPhotos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoBase64",
                table: "UserPhotos");

            migrationBuilder.RenameColumn(
                name: "TrainingImageBase64",
                table: "TrainingSelfies",
                newName: "TrainingImageUrl");

            migrationBuilder.RenameColumn(
                name: "TeamImageBase64",
                table: "TeamSelfies",
                newName: "TeamImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "UserPhotos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
