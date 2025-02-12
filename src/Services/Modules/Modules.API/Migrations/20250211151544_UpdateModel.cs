using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModuleModelModuleId",
                table: "Modules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ModuleModelModuleId",
                table: "Modules",
                column: "ModuleModelModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Modules_ModuleModelModuleId",
                table: "Modules",
                column: "ModuleModelModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Modules_ModuleModelModuleId",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_ModuleModelModuleId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "ModuleModelModuleId",
                table: "Modules");
        }
    }
}
