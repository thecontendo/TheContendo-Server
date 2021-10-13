using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Contendo.Db.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    City = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactRequestStatus = table.Column<int>(type: "integer", nullable: false),
                    Uri = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true),
                    Height = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Challenges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    TimeLimit = table.Column<int>(type: "integer", nullable: false),
                    ShotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChallengerId = table.Column<Guid>(type: "uuid", nullable: false),
                    WinnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    DefenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChallengeStatus = table.Column<int>(type: "integer", nullable: false),
                    ChallengerStatus = table.Column<int>(type: "integer", nullable: false),
                    DefenderStatus = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Challenges_Shots_ShotId",
                        column: x => x.ShotId,
                        principalTable: "Shots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Challenges_Users_ChallengerId",
                        column: x => x.ChallengerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Challenges_Users_DefenderId",
                        column: x => x.DefenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContacts",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContacts", x => new { x.ContactId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserContacts_Users_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContacts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Shots",
                columns: new[] { "Id", "Description", "Icon", "Name" },
                values: new object[,]
                {
                    { new Guid("0a4263c0-4f89-421c-871c-1a811092c316"), null, "pushups.png", "Push Ups" },
                    { new Guid("7141c807-233b-42de-8b18-878f4c5d6f91"), null, "burpees.png", "Burpees" },
                    { new Guid("d0562f7f-bf94-44a3-a3e0-d8d40d419880"), null, "jumpingjacks.png", "Jumping Jacks" },
                    { new Guid("02b8a53a-9b37-439a-88d1-d0363d621508"), null, "classicplank.png", "Classical Plank" },
                    { new Guid("a5bb13cb-adb2-4bb6-b490-77bee49182e4"), null, "straighthandplank.png", "Straight Hand Plank" },
                    { new Guid("27b4717e-bc68-484d-b98b-07387425604c"), null, "sideplank.png", "Side Plank" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AddressId", "Description", "Email", "FirstName", "Gender", "Height", "LastName", "Status", "Title", "Username", "ValidFrom", "ValidTo", "Weight" },
                values: new object[,]
                {
                    { new Guid("2c5b22e6-8b98-460f-98c9-6227e61b8d66"), null, null, "thecontendo@gmail.com", "Super", 0, null, "Admin", 0, "Company", "SuperAdmin", new DateTime(2020, 12, 31, 23, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 12, 31, 22, 59, 59, 999, DateTimeKind.Utc).AddTicks(9999), null },
                    { new Guid("88922a62-7304-4234-8b91-6a901cfbf779"), null, "Entrepreneur", "abhinav9p@gmail.com", "Abhinav", 0, null, "Parankusham", 0, "Mr.", "pac", new DateTime(2020, 12, 31, 23, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 12, 31, 22, 59, 59, 999, DateTimeKind.Utc).AddTicks(9999), null },
                    { new Guid("b074cf2c-20b2-4bba-870e-f86a11f32bb6"), null, "Lead", "soumya9v@gmail.com", "Soumya", 0, null, "Pullakhandam", 0, "Ms.", "SuperAdmin", new DateTime(2020, 12, 31, 23, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 12, 31, 22, 59, 59, 999, DateTimeKind.Utc).AddTicks(9999), null },
                    { new Guid("511b1390-e52c-474d-b64e-1073c881b1e6"), null, "Guitarist", "p2@gmail.com", "P2", 0, null, "Bhikkumalla", 0, "Mr.", "p2", new DateTime(2020, 12, 31, 23, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 12, 31, 22, 59, 59, 999, DateTimeKind.Utc).AddTicks(9999), null },
                    { new Guid("5fcf796a-3c30-4d25-9110-0a84e9eb85a7"), null, "Hello", "u4@gmail.com", "u4", 0, null, "u4", 0, "Ms.", "u4", new DateTime(2020, 12, 31, 23, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 12, 31, 22, 59, 59, 999, DateTimeKind.Utc).AddTicks(9999), null },
                    { new Guid("6d672be4-fa62-4451-8d92-5fc983f61ab6"), null, "Hi", "u5@gmail.com", "u5", 0, null, "u5", 0, "Mr.", "u5", new DateTime(2020, 12, 31, 23, 0, 0, 0, DateTimeKind.Utc), new DateTime(9999, 12, 31, 22, 59, 59, 999, DateTimeKind.Utc).AddTicks(9999), null }
                });

            migrationBuilder.InsertData(
                table: "UserContacts",
                columns: new[] { "ContactId", "UserId" },
                values: new object[,]
                {
                    { new Guid("b074cf2c-20b2-4bba-870e-f86a11f32bb6"), new Guid("88922a62-7304-4234-8b91-6a901cfbf779") },
                    { new Guid("511b1390-e52c-474d-b64e-1073c881b1e6"), new Guid("88922a62-7304-4234-8b91-6a901cfbf779") },
                    { new Guid("511b1390-e52c-474d-b64e-1073c881b1e6"), new Guid("b074cf2c-20b2-4bba-870e-f86a11f32bb6") },
                    { new Guid("b074cf2c-20b2-4bba-870e-f86a11f32bb6"), new Guid("511b1390-e52c-474d-b64e-1073c881b1e6") },
                    { new Guid("5fcf796a-3c30-4d25-9110-0a84e9eb85a7"), new Guid("511b1390-e52c-474d-b64e-1073c881b1e6") },
                    { new Guid("88922a62-7304-4234-8b91-6a901cfbf779"), new Guid("5fcf796a-3c30-4d25-9110-0a84e9eb85a7") },
                    { new Guid("6d672be4-fa62-4451-8d92-5fc983f61ab6"), new Guid("511b1390-e52c-474d-b64e-1073c881b1e6") },
                    { new Guid("5fcf796a-3c30-4d25-9110-0a84e9eb85a7"), new Guid("6d672be4-fa62-4451-8d92-5fc983f61ab6") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_ChallengerId",
                table: "Challenges",
                column: "ChallengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_DefenderId",
                table: "Challenges",
                column: "DefenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_ShotId",
                table: "Challenges",
                column: "ShotId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContacts_UserId",
                table: "UserContacts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressId",
                table: "Users",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username_Email",
                table: "Users",
                columns: new[] { "Username", "Email" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Challenges");

            migrationBuilder.DropTable(
                name: "ContactRequests");

            migrationBuilder.DropTable(
                name: "UserContacts");

            migrationBuilder.DropTable(
                name: "Shots");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
