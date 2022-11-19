using Microsoft.EntityFrameworkCore.Migrations;

namespace TallerFedexMotos.Data.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "marcas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreMarca = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marcas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "modelos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreModelo = table.Column<string>(nullable: false),
                    marcaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modelos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_modelos_marcas_marcaId",
                        column: x => x.marcaId,
                        principalTable: "marcas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "motos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    modeloId = table.Column<int>(nullable: false),
                    año = table.Column<int>(nullable: false),
                    imagenMoto = table.Column<string>(nullable: true),
                    MarcaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_motos_marcas_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "marcas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_motos_modelos_modeloId",
                        column: x => x.modeloId,
                        principalTable: "modelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(nullable: false),
                    apellido = table.Column<string>(nullable: false),
                    motoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_clientes_motos_motoId",
                        column: x => x.motoId,
                        principalTable: "motos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "motosEnReparacion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    motoId = table.Column<int>(nullable: false),
                    trabajoRealizado = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motosEnReparacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_motosEnReparacion_motos_motoId",
                        column: x => x.motoId,
                        principalTable: "motos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "motosEnVenta",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    motoId = table.Column<int>(nullable: false),
                    precio = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motosEnVenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_motosEnVenta_motos_motoId",
                        column: x => x.motoId,
                        principalTable: "motos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clientes_motoId",
                table: "clientes",
                column: "motoId");

            migrationBuilder.CreateIndex(
                name: "IX_modelos_marcaId",
                table: "modelos",
                column: "marcaId");

            migrationBuilder.CreateIndex(
                name: "IX_motos_MarcaId",
                table: "motos",
                column: "MarcaId");

            migrationBuilder.CreateIndex(
                name: "IX_motos_modeloId",
                table: "motos",
                column: "modeloId");

            migrationBuilder.CreateIndex(
                name: "IX_motosEnReparacion_motoId",
                table: "motosEnReparacion",
                column: "motoId");

            migrationBuilder.CreateIndex(
                name: "IX_motosEnVenta_motoId",
                table: "motosEnVenta",
                column: "motoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clientes");

            migrationBuilder.DropTable(
                name: "motosEnReparacion");

            migrationBuilder.DropTable(
                name: "motosEnVenta");

            migrationBuilder.DropTable(
                name: "motos");

            migrationBuilder.DropTable(
                name: "modelos");

            migrationBuilder.DropTable(
                name: "marcas");
        }
    }
}
