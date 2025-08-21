using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WooriOptical.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PrescriptionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    Frame = table.Column<string>(type: "TEXT", nullable: true),
                    FramePrice = table.Column<string>(type: "TEXT", nullable: true),
                    Lens = table.Column<string>(type: "TEXT", nullable: true),
                    LensPrice = table.Column<string>(type: "TEXT", nullable: true),
                    TotalAmount = table.Column<double>(type: "REAL", nullable: false),
                    Discount = table.Column<string>(type: "TEXT", nullable: true),
                    FinalAmount = table.Column<string>(type: "TEXT", nullable: true),
                    Deposit = table.Column<string>(type: "TEXT", nullable: true),
                    Balance = table.Column<string>(type: "TEXT", nullable: true),
                    PayoffStatus = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    PrescriptionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RSphere = table.Column<double>(type: "REAL", nullable: false),
                    RCylinder = table.Column<double>(type: "REAL", nullable: false),
                    RAxis = table.Column<double>(type: "REAL", nullable: false),
                    LSphere = table.Column<double>(type: "REAL", nullable: false),
                    LCylinder = table.Column<double>(type: "REAL", nullable: false),
                    LAxis = table.Column<double>(type: "REAL", nullable: false),
                    PD = table.Column<double>(type: "REAL", nullable: false),
                    Add = table.Column<double>(type: "REAL", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.PrescriptionId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Prescriptions");
        }
    }
}
