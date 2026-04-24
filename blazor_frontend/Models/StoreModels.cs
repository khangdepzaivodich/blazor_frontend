namespace blazor_frontend.Models
{
    public class DanhMuc
    {
        public Guid MaDM { get; set; } = Guid.NewGuid();
        public string TenDM { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }

    public class LoaiDanhMuc
    {
        public Guid MaLDM { get; set; } = Guid.NewGuid();
        public Guid MaDM { get; set; }
        public string TenLDM { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        public DanhMuc? DanhMuc { get; set; }
    }

    public class ChiTietSanPham
    {
        public Guid MaCTSP { get; set; } = Guid.NewGuid();
        public Guid MaSP { get; set; }
        public string Mau { get; set; } = string.Empty;
        public string KichCo { get; set; } = string.Empty;
        public string Anh { get; set; } = string.Empty;
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
    }

    public class SanPham
    {
        public Guid MaSP { get; set; } = Guid.NewGuid();
        public Guid MaLDM { get; set; }
        public string TenSP { get; set; } = string.Empty;
        public string AnhDaiDien { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public bool TrangThai { get; set; }

        public LoaiDanhMuc? LoaiDanhMuc { get; set; }
        public List<ChiTietSanPham> ChiTietSanPhams { get; set; } = new();
    }

    public class TaiKhoan
    {
        public Guid MaTK { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string VaiTro { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public DateTime? NgaySinh { get; set; }
        public string TrangThai { get; set; } = "Active";
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public DateTime? LastActiveAt { get; set; }
    }

    public class CuocTroChuyen
    {
        public Guid MaCTC { get; set; } = Guid.NewGuid();
        public Guid MaKhachHang { get; set; }
        public Guid? MaNhanVien { get; set; }
        public int SoTinNhanChuaDocNV { get; set; } = 0;
        public int SoTinNhanChuaDocKH { get; set; } = 0;
        public string LastMessagePreview { get; set; } = string.Empty;
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        public TaiKhoan? KhachHang { get; set; }
        public TaiKhoan? NhanVien { get; set; }
        public List<TinNhan> TinNhans { get; set; } = new();
    }

    public class TinNhan
    {
        public long MaTN { get; set; }
        public Guid MaCTC { get; set; }
        public Guid MaNguoiGui { get; set; }
        public string NoiDung { get; set; } = string.Empty;
        public DateTime NgayGui { get; set; } = DateTime.Now;
        public bool DaDoc { get; set; } = false;

        public CuocTroChuyen? CuocTroChuyen { get; set; }
        public TaiKhoan? NguoiGui { get; set; }
    }

    // Mock Data sản phẩm
    public static class MockStoreData
    {
        public static List<LoaiDanhMuc> GetMockLoaiDanhMucs()
        {
            var dmAoId = Guid.NewGuid();
            var dmQuanId = Guid.NewGuid();

            var dmAo = new DanhMuc { MaDM = dmAoId, TenDM = "Áo", Slug = "ao" };
            var dmQuan = new DanhMuc { MaDM = dmQuanId, TenDM = "Quần", Slug = "quan" };

            return new List<LoaiDanhMuc>
            {
                new LoaiDanhMuc { MaLDM = Guid.NewGuid(), TenLDM = "Áo thun", MaDM = dmAoId, Slug = "ao-thun", DanhMuc = dmAo },
                new LoaiDanhMuc { MaLDM = Guid.NewGuid(), TenLDM = "Áo khoác", MaDM = dmAoId, Slug = "ao-khoac", DanhMuc = dmAo },
                new LoaiDanhMuc { MaLDM = Guid.NewGuid(), TenLDM = "Áo sơ mi", MaDM = dmAoId, Slug = "ao-so-mi", DanhMuc = dmAo },
                new LoaiDanhMuc { MaLDM = Guid.NewGuid(), TenLDM = "Quần Jean", MaDM = dmQuanId, Slug = "quan-jean", DanhMuc = dmQuan },
                new LoaiDanhMuc { MaLDM = Guid.NewGuid(), TenLDM = "Quần Tây", MaDM = dmQuanId, Slug = "quan-tay", DanhMuc = dmQuan }
            };
        }

        public static List<SanPham> GetMockSanPhams()
        {
            var dmAoId = Guid.NewGuid();
            var dmAo = new DanhMuc { MaDM = dmAoId, TenDM = "Áo", Slug = "ao" };

            var ldmAoThunId = Guid.NewGuid();
            var ldmAoKhoacId = Guid.NewGuid();

            var ldmAoThun = new LoaiDanhMuc { MaLDM = ldmAoThunId, TenLDM = "Áo thun", MaDM = dmAoId, Slug = "ao-thun", DanhMuc = dmAo };
            var ldmAoKhoac = new LoaiDanhMuc { MaLDM = ldmAoKhoacId, TenLDM = "Áo khoác", MaDM = dmAoId, Slug = "ao-khoac", DanhMuc = dmAo };

            var sp1Id = Guid.NewGuid();
            var sp2Id = Guid.NewGuid();

            return new List<SanPham>
            {
                new SanPham
                {
                    MaSP = sp1Id,
                    TenSP = "Áo thun Polo Basic Nam",
                    AnhDaiDien = "https://placehold.co/100x100/e2e8f0/64748b?text=Polo",
                    MaLDM = ldmAoThunId,
                    TrangThai = true,
                    MoTa = "Áo thun Polo chất liệu cotton thoáng mát.",
                    LoaiDanhMuc = ldmAoThun,
                    ChiTietSanPhams = new List<ChiTietSanPham>
                    {
                        new ChiTietSanPham { MaCTSP = Guid.NewGuid(), MaSP = sp1Id, Mau = "Đỏ", KichCo = "M", SoLuong = 50, Gia = 250000 },
                        new ChiTietSanPham { MaCTSP = Guid.NewGuid(), MaSP = sp1Id, Mau = "Đỏ", KichCo = "L", SoLuong = 30, Gia = 250000 },
                        new ChiTietSanPham { MaCTSP = Guid.NewGuid(), MaSP = sp1Id, Mau = "Xanh", KichCo = "M", SoLuong = 20, Gia = 250000 },
                        new ChiTietSanPham { MaCTSP = Guid.NewGuid(), MaSP = sp1Id, Mau = "Xanh", KichCo = "L", SoLuong = 10, Gia = 250000 },
                        new ChiTietSanPham { MaCTSP = Guid.NewGuid(), MaSP = sp1Id, Mau = "Đen", KichCo = "XL", SoLuong = 5, Gia = 270000 }
                    }
                },
                new SanPham
                {
                    MaSP = sp2Id,
                    TenSP = "Áo khoác Hoodie Mùa đông 2023",
                    AnhDaiDien = "https://placehold.co/100x100/e2e8f0/64748b?text=Hoodie",
                    MaLDM = ldmAoKhoacId,
                    TrangThai = false,
                    MoTa = "Áo khoác nỉ bông ấm áp.",
                    LoaiDanhMuc = ldmAoKhoac,
                    ChiTietSanPhams = new List<ChiTietSanPham>
                    {
                        new ChiTietSanPham { MaCTSP = Guid.NewGuid(), MaSP = sp2Id, Mau = "Xám", KichCo = "L", SoLuong = 0, Gia = 450000 },
                        new ChiTietSanPham { MaCTSP = Guid.NewGuid(), MaSP = sp2Id, Mau = "Đen", KichCo = "XL", SoLuong = 0, Gia = 450000 }
                    }
                }
            };
        }

        public static List<TaiKhoan> GetMockTaiKhoans()
        {
            return new List<TaiKhoan>
            {
                new TaiKhoan { MaTK = Guid.NewGuid(), HoTen = "Nguyễn Văn Admin", SoDienThoai = "0901234567", VaiTro = "Admin", DiaChi = "123 Q1, TP.HCM", TrangThai = "Active", NgayTao = new DateTime(2023, 10, 1) },
                new TaiKhoan { MaTK = Guid.NewGuid(), HoTen = "Trần Thị Staff", SoDienThoai = "0987654321", VaiTro = "Staff", DiaChi = "456 Q3, TP.HCM", TrangThai = "Active", NgayTao = new DateTime(2023, 11, 15) },
                new TaiKhoan { MaTK = Guid.NewGuid(), HoTen = "Lê Khách Hàng", SoDienThoai = "0123456789", VaiTro = "Customer", DiaChi = "789 Q7, TP.HCM", TrangThai = "Active", NgayTao = DateTime.Now.AddDays(-2) },
                new TaiKhoan { MaTK = Guid.NewGuid(), HoTen = "Phạm Khách Cũ", SoDienThoai = "0333222111", VaiTro = "Customer", DiaChi = "111 Gò Vấp, TP.HCM", TrangThai = "Locked", NgayTao = DateTime.Now.AddDays(-20) }
            };
        }
    }
}