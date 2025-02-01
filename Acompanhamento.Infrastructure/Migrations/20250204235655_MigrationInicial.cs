using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acompanhamento.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "Seq_CodAcompanhamento");

            migrationBuilder.CreateTable(
                name: "Acompanhamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoAcompanhamento = table.Column<short>(type: "smallint", nullable: false, defaultValueSql: "nextval('\"Seq_CodAcompanhamento\"')"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IdPedido = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acompanhamento", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Status",
                table: "Acompanhamento",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acompanhamento");

            migrationBuilder.DropSequence(
                name: "Seq_CodAcompanhamento");
        }
    }
}
