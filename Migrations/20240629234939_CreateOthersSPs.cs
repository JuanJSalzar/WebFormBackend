using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebForm.Migrations
{
    /// <inheritdoc />
    public partial class CreateOthersSPs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.Sql(@"
            CREATE PROCEDURE dbo.select_person_by_id
                @Id INT
            AS
            BEGIN
                SELECT * FROM People WHERE Id = @Id;
            END
        ");

                migrationBuilder.Sql(@"
            CREATE PROCEDURE dbo.insert_person
                @FirstName NVARCHAR(100),
                @LastName NVARCHAR(100),
                @IdentificationNumber NVARCHAR(20),
                @Email NVARCHAR(100),
                @IdentificationType NVARCHAR(50)
            AS
            BEGIN
                BEGIN TRANSACTION;

                INSERT INTO People (FirstName, LastName, IdentificationNumber, Email, IdentificationType, CreationDate)
                VALUES (@FirstName, @LastName, @IdentificationNumber, @Email, @IdentificationType, GETDATE());
                
                COMMIT TRANSACTION;

                SELECT * FROM People WHERE Id = SCOPE_IDENTITY();
            END
        ");

                migrationBuilder.Sql(@"
            CREATE PROCEDURE dbo.update_person
                @Id INT,
                @FirstName NVARCHAR(100),
                @LastName NVARCHAR(100),
                @Email NVARCHAR(100),
                @IdentificationType NVARCHAR(50)
            AS
            BEGIN
                BEGIN TRANSACTION;

                UPDATE People
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    Email = @Email,
                    IdentificationType = @IdentificationType
                WHERE Id = @Id;
                
                COMMIT TRANSACTION;

                SELECT * FROM People WHERE Id = @Id;
            END
        ");
                migrationBuilder.Sql(@"
            CREATE PROCEDURE dbo.delete_person
                @Id INT
            AS
            BEGIN
                DECLARE @DeletedPerson TABLE (
                    Id INT,
                    FirstName NVARCHAR(100),
                    LastName NVARCHAR(100),
                    IdentificationNumber NVARCHAR(20),
                    Email NVARCHAR(100),
                    IdentificationType NVARCHAR(50),
                    CreationDate DATETIME
                );

                BEGIN TRANSACTION;

                DELETE FROM People
                OUTPUT DELETED.Id, DELETED.FirstName, DELETED.LastName, DELETED.IdentificationNumber, DELETED.Email, DELETED.IdentificationType, DELETED.CreationDate INTO @DeletedPerson
                WHERE Id = @Id;

                COMMIT TRANSACTION;

                SELECT * FROM @DeletedPerson;
            END
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.select_person_by_id");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.insert_person");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.update_person");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.delete_person");
        }
    }
}
