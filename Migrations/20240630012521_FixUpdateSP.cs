using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebForm.Migrations
{
    /// <inheritdoc />
    public partial class FixUpdateSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            ALTER PROCEDURE [dbo].[update_person]
                @Id INT,
                @FirstName NVARCHAR(100),
                @LastName NVARCHAR(100),
                @Email NVARCHAR(100),
                @IdentificationType NVARCHAR(50)
            AS
            BEGIN
                SET NOCOUNT ON;
                BEGIN TRY
                    BEGIN TRANSACTION;
                    IF NOT EXISTS (SELECT 1 FROM People WHERE Id = @Id)
                    BEGIN
                        THROW 51001, 'Person not found', 1;
                    END
                    IF EXISTS (SELECT 1 FROM People WHERE Email = @Email AND Id <> @Id)
                    BEGIN
                        THROW 51002, 'Email already exists', 1;
                    END
                    UPDATE People
                    SET FirstName = @FirstName,
                        LastName = @LastName,
                        Email = @Email,
                        IdentificationType = @IdentificationType
                    WHERE Id = @Id;
                    COMMIT TRANSACTION;
                    SELECT * FROM People WHERE Id = @Id;
                END TRY
                BEGIN CATCH
                    IF @@TRANCOUNT > 0
                        ROLLBACK TRANSACTION;
                    THROW;
                END CATCH
            END
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
