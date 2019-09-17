using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiTest.Data.Migrations
{
    public partial class SeedPeopleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO People (Firstname, Lastname, Email, City, DateCreated) VALUES ('Per','Persson','per.persson@email.com','New York','2019-07-29 12:00:00')");
            migrationBuilder.Sql("INSERT INTO People (Firstname, Lastname, Email, City, DateCreated) VALUES ('Karl','Karlsson','karl.karlsson@email.com','Stockholm','2019-08-01 12:00:00')");
            migrationBuilder.Sql("INSERT INTO People (Firstname, Lastname, Email, City, DateCreated) VALUES ('Anders','Andersson','anders.andersson@email.com','London','2019-08-10 12:00:00')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
