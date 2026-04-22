using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUANCUAHANGAPPLE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVoucherAndAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaChiChiTiet",
                table: "ThanhToan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ThanhToan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "ThanhToan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTen",
                table: "ThanhToan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PhieuGiamGiaId",
                table: "ThanhToan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhuongXa",
                table: "ThanhToan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QuanHuyen",
                table: "ThanhToan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoai",
                table: "ThanhToan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "SoTienGiam",
                table: "ThanhToan",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TinhThanh",
                table: "ThanhToan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PhieuGiamGia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ma = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GiaTriGiam = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LoaiGiam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonHangToiThieu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GiamToiDa = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DaSuDung = table.Column<int>(type: "int", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuGiamGia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhieuGiamGiaNguoiDung",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhieuGiamGiaId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NgayNhan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaSuDung = table.Column<bool>(type: "bit", nullable: false),
                    NgaySuDung = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuGiamGiaNguoiDung", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhieuGiamGiaNguoiDung_PhieuGiamGia_PhieuGiamGiaId",
                        column: x => x.PhieuGiamGiaId,
                        principalTable: "PhieuGiamGia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhieuGiamGiaNguoiDung_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_PhieuGiamGiaId",
                table: "ThanhToan",
                column: "PhieuGiamGiaId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuGiamGiaNguoiDung_PhieuGiamGiaId",
                table: "PhieuGiamGiaNguoiDung",
                column: "PhieuGiamGiaId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuGiamGiaNguoiDung_UserId",
                table: "PhieuGiamGiaNguoiDung",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThanhToan_PhieuGiamGia_PhieuGiamGiaId",
                table: "ThanhToan",
                column: "PhieuGiamGiaId",
                principalTable: "PhieuGiamGia",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThanhToan_PhieuGiamGia_PhieuGiamGiaId",
                table: "ThanhToan");

            migrationBuilder.DropTable(
                name: "PhieuGiamGiaNguoiDung");

            migrationBuilder.DropTable(
                name: "PhieuGiamGia");

            migrationBuilder.DropIndex(
                name: "IX_ThanhToan_PhieuGiamGiaId",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "DiaChiChiTiet",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "HoTen",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "PhieuGiamGiaId",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "PhuongXa",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "QuanHuyen",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "SoDienThoai",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "SoTienGiam",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "TinhThanh",
                table: "ThanhToan");
        }
    }
}
