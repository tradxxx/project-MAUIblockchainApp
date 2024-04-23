using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBlockChain.Migrations
{
    /// <inheritdoc />
    public partial class InitUpdate6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "Categories",
                newName: "Tag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tag",
                table: "Categories",
                newName: "Icon");
        }
    }
}
