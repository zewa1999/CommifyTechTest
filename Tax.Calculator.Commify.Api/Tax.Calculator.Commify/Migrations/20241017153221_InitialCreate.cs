using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tax.Calculator.Commify.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxBands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BandName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LowerSalaryRange = table.Column<int>(type: "integer", nullable: false),
                    UpperSalaryRange = table.Column<int>(type: "integer", nullable: true),
                    TaxRate = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxBands", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TaxBands",
                columns: new[] { "Id", "BandName", "LowerSalaryRange", "TaxRate", "UpperSalaryRange" },
                values: new object[,]
                {
                    { new Guid("092cbf43-9e33-4730-b5bd-c987e1bd8df1"), "Tax Band C", 20000, 40, null },
                    { new Guid("1102b5a2-4e98-4ae8-b627-34cae4ab4fd9"), "Tax Band A", 0, 0, 5000 },
                    { new Guid("3a734862-f345-4197-9f0f-4bb6f2bf3f39"), "Tax Band B", 5000, 20, 20000 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxBands");
        }
    }
}
