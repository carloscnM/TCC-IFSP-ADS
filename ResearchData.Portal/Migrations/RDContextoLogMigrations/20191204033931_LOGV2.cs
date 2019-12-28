using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ResearchData.Portal.Migrations.RDContextoLogMigrations
{
    public partial class LOGV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RESEARCHLOG",
                columns: table => new
                {
                    log_id = table.Column<Guid>(nullable: false),
                    log_message = table.Column<string>(nullable: true),
                    log_captura = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESEARCHLOG", x => x.log_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RESEARCHLOG");
        }
    }
}
