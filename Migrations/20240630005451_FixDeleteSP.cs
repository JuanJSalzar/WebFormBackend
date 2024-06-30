using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebForm.Migrations
{
    /// <inheritdoc />
    public partial class FixDeleteSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER PROCEDURE dbo.delete_person
                    @Id INT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    DECLARE @ErrorMessage NVARCHAR(4000);

                    BEGIN TRY
                        BEGIN TRANSACTION;

                        IF NOT EXISTS (SELECT 1 FROM People WHERE Id = @Id)
                        BEGIN
                            SET @ErrorMessage = 'Person with specified Id does not exist.';
                            THROW 50000, @ErrorMessage, 1;
                        END

                        DECLARE @DeletedPerson TABLE (
                            Id INT,
                            FirstName NVARCHAR(100),
                            LastName NVARCHAR(100),
                            IdentificationNumber NVARCHAR(20),
                            Email NVARCHAR(100),
                            IdentificationType NVARCHAR(50),
                            CreationDate DATETIME
                        );

                        DELETE FROM People
                        OUTPUT DELETED.Id, DELETED.FirstName, DELETED.LastName, DELETED.IdentificationNumber, DELETED.Email, DELETED.IdentificationType, DELETED.CreationDate INTO @DeletedPerson
                        WHERE Id = @Id;

                        COMMIT TRANSACTION;

                        SELECT * FROM @DeletedPerson;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;

                        SET @ErrorMessage = ERROR_MESSAGE();
                        THROW 50000, @ErrorMessage, 1;
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
