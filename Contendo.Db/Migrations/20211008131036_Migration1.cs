using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Contendo.Db.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Shots",
                keyColumn: "Id",
                keyValue: new Guid("02b8a53a-9b37-439a-88d1-d0363d621508"));

            migrationBuilder.DeleteData(
                table: "Shots",
                keyColumn: "Id",
                keyValue: new Guid("0a4263c0-4f89-421c-871c-1a811092c316"));

            migrationBuilder.DeleteData(
                table: "Shots",
                keyColumn: "Id",
                keyValue: new Guid("27b4717e-bc68-484d-b98b-07387425604c"));

            migrationBuilder.DeleteData(
                table: "Shots",
                keyColumn: "Id",
                keyValue: new Guid("7141c807-233b-42de-8b18-878f4c5d6f91"));

            migrationBuilder.DeleteData(
                table: "Shots",
                keyColumn: "Id",
                keyValue: new Guid("a5bb13cb-adb2-4bb6-b490-77bee49182e4"));

            migrationBuilder.DeleteData(
                table: "Shots",
                keyColumn: "Id",
                keyValue: new Guid("d0562f7f-bf94-44a3-a3e0-d8d40d419880"));
        }
    }
}
