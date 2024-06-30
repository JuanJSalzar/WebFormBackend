using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebForm.Migrations
{
    /// <inheritdoc />
    public partial class SpSelectFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE dbo.select_people
            AS
            BEGIN
                SET NOCOUNT ON;
                SELECT *
                FROM People;
            END
        ");   
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.select_people");
        }
    }
}
