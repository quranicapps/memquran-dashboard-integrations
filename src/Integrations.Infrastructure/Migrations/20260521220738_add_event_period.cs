using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integrations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_event_period : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventPeriod",
                table: "EventAggregation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventPeriod",
                table: "EventAggregation");
        }
    }
}
