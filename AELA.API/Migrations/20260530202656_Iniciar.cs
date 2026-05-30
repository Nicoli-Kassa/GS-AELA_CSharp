using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AELA.API.Migrations
{
    /// <inheritdoc />
    public partial class Iniciar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Astronautas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MissaoAtual = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DiasEmOrbita = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    TipoAmbiente = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Astronautas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Baselines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    OperadorId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    FrequenciaCardiacaBasal = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    TempoReacaoBasal = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PressaoOcularBasal = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baselines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leituras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    OperadorId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    FrequenciaCardiaca = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    TempoReacao = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PressaoOcular = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leituras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperadoresTerrestres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TipoOperacao = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    TipoAmbiente = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperadoresTerrestres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    OperadorId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Tarefa = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Score = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PercentualDesvio = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    CalcularEm = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Astronautas");

            migrationBuilder.DropTable(
                name: "Baselines");

            migrationBuilder.DropTable(
                name: "Leituras");

            migrationBuilder.DropTable(
                name: "OperadoresTerrestres");

            migrationBuilder.DropTable(
                name: "Scores");
        }
    }
}
