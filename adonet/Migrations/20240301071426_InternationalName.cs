using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace adonet.Migrations
{
    /// <inheritdoc />
    public partial class InternationalName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InternationalName",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("131ef84b-f06e-494b-848f-bb4bc0604266"),
                column: "InternationalName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("1ef7268c-43a8-488c-b761-90982b31df4e"),
                column: "InternationalName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("415b36d9-2d82-4a92-a313-48312f8e18c6"),
                column: "InternationalName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("624b3bb5-0f2c-42b6-a416-099aab799546"),
                column: "InternationalName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("8dcc3969-1d93-47a9-8b79-a30c738db9b4"),
                column: "InternationalName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("d2469412-0e4b-46f7-80ec-8c522364d099"),
                column: "InternationalName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("d3c376e4-bce3-4d85-aba4-e3cf49612c94"),
                column: "InternationalName",
                value: "ITDepartment");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ManagerId",
                table: "Sales",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ProductId",
                table: "Sales",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_IdChief",
                table: "Managers",
                column: "IdChief");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_IdMainDep",
                table: "Managers",
                column: "IdMainDep");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_IdSecDep",
                table: "Managers",
                column: "IdSecDep");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Departments_IdMainDep",
                table: "Managers",
                column: "IdMainDep",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Departments_IdSecDep",
                table: "Managers",
                column: "IdSecDep",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Managers_IdChief",
                table: "Managers",
                column: "IdChief",
                principalTable: "Managers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Managers_ManagerId",
                table: "Sales",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Products_ProductId",
                table: "Sales",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Departments_IdMainDep",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Departments_IdSecDep",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Managers_IdChief",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Managers_ManagerId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Products_ProductId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ManagerId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ProductId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Managers_IdChief",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_IdMainDep",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_IdSecDep",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "InternationalName",
                table: "Departments");
        }
    }
}
