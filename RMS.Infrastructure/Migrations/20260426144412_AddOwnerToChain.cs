using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerToChain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chain_AspNetUsers_UserId",
                table: "Chain");

            migrationBuilder.DropForeignKey(
                name: "FK_Chain_Owners_OwnerId",
                table: "Chain");

            migrationBuilder.DropIndex(
                name: "IX_Chain_UserId",
                table: "Chain");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Chain");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Chain",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chain_Owners_OwnerId",
                table: "Chain",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chain_Owners_OwnerId",
                table: "Chain");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Chain",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Chain",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Chain_UserId",
                table: "Chain",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chain_AspNetUsers_UserId",
                table: "Chain",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chain_Owners_OwnerId",
                table: "Chain",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id");
        }
    }
}
