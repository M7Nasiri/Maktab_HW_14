using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ATM.Migrations
{
    /// <inheritdoc />
    public partial class AddVerificationCodeTimeToCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VerificationCodeCreatedAt",
                table: "Card",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationCodeCreatedAt",
                table: "Card");
        }
    }
}
