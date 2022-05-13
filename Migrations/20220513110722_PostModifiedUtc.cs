using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shipstone.OpenBook.Migrations
{
    public partial class PostModifiedUtc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedUtc",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedUtc",
                table: "Posts");
        }
    }
}
