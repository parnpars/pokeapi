using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonCatcher.Migrations
{
    /// <inheritdoc />
    public partial class addkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RelationshipId",
                table: "type_relationship",
                newName: "PokemonPK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PokemonPK",
                table: "type_relationship",
                newName: "RelationshipId");
        }
    }
}
