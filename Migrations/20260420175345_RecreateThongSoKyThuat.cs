using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUANCUAHANGAPPLE.Migrations
{
    /// <inheritdoc />
    public partial class RecreateThongSoKyThuat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa bảng cũ trước vì cấu trúc thay đổi hoàn toàn và không thể đổi IDENTITY
            migrationBuilder.DropTable(name: "ThongSoKyThuat");

            migrationBuilder.CreateTable(
                name: "LoaiThongSo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiThongSo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThongSoKyThuat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SanPhamId = table.Column<int>(type: "int", nullable: false),
                    LoaiThongSoId = table.Column<int>(type: "int", nullable: false),
                    TenThongSo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiaTri = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongSoKyThuat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThongSoKyThuat_LoaiThongSo_LoaiThongSoId",
                        column: x => x.LoaiThongSoId,
                        principalTable: "LoaiThongSo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThongSoKyThuat_SanPham_SanPhamId",
                        column: x => x.SanPhamId,
                        principalTable: "SanPham",
                        principalColumn: "SanPhamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "BienTheId",
                table: "ChiTietThanhToan",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThongSoKyThuat_LoaiThongSoId",
                table: "ThongSoKyThuat",
                column: "LoaiThongSoId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongSoKyThuat_SanPhamId",
                table: "ThongSoKyThuat",
                column: "SanPhamId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThanhToan_BienTheId",
                table: "ChiTietThanhToan",
                column: "BienTheId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietThanhToan_BienTheSanPham_BienTheId",
                table: "ChiTietThanhToan",
                column: "BienTheId",
                principalTable: "BienTheSanPham",
                principalColumn: "BienTheId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietThanhToan_BienTheSanPham_BienTheId",
                table: "ChiTietThanhToan");

            migrationBuilder.DropTable(name: "ThongSoKyThuat");
            migrationBuilder.DropTable(name: "LoaiThongSo");

            migrationBuilder.DropColumn(
                name: "BienTheId",
                table: "ChiTietThanhToan");

            // Tạo lại bảng theo cấu trúc cũ nếu cần Down
            migrationBuilder.CreateTable(
                name: "ThongSoKyThuat",
                columns: table => new
                {
                    ThongSoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SanPhamId = table.Column<int>(type: "int", nullable: false),
                    ManHinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Chipset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraSau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraTruoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bluetooth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WiFi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongSoKyThuat", x => x.ThongSoId);
                    table.ForeignKey(
                        name: "FK_ThongSoKyThuat_SanPham_SanPhamId",
                        column: x => x.SanPhamId,
                        principalTable: "SanPham",
                        principalColumn: "SanPhamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThongSoKyThuat_SanPhamId",
                table: "ThongSoKyThuat",
                column: "SanPhamId",
                unique: true);
        }
    }
}
