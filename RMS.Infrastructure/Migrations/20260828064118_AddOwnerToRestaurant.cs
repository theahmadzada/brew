using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerToRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Chains_ChainId",
                table: "Restaurants");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChainId",
                table: "Restaurants",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Restaurants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Chains_ChainId",
                table: "Restaurants",
                column: "ChainId",
                principalTable: "Chains",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Owners_OwnerId",
                table: "Restaurants",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Chains_ChainId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Owners_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Restaurants");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChainId",
                table: "Restaurants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Chains_ChainId",
                table: "Restaurants",
                column: "ChainId",
                principalTable: "Chains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
