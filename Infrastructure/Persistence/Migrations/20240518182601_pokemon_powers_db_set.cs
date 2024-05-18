using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class pokemon_powers_db_set : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PokemonPower_Power_PowersId",
                table: "PokemonPower");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Power",
                table: "Power");

            migrationBuilder.RenameTable(
                name: "Power",
                newName: "Powers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Powers",
                table: "Powers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonPower_Powers_PowersId",
                table: "PokemonPower",
                column: "PowersId",
                principalTable: "Powers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PokemonPower_Powers_PowersId",
                table: "PokemonPower");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Powers",
                table: "Powers");

            migrationBuilder.RenameTable(
                name: "Powers",
                newName: "Power");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Power",
                table: "Power",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonPower_Power_PowersId",
                table: "PokemonPower",
                column: "PowersId",
                principalTable: "Power",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
