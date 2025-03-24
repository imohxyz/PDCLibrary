using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema9.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class M01Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cinema9");

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "cinema9",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVARCHAR(5)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                schema: "cinema9",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    Synopsis = table.Column<string>(type: "NVARCHAR(2000)", nullable: true),
                    ReleaseDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    Budget = table.Column<decimal>(type: "MONEY", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "cinema9",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CountryId",
                schema: "cinema9",
                table: "Movies",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies",
                schema: "cinema9");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "cinema9");
        }
    }
}
