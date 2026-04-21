using System;
using System.Collections.Generic;

namespace blazor_frontend.Models.BackendDTOs
{
    // IDENTITY
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }

    public class UserDto
    {
        public Guid MaTK { get; set; }
        public string SoDienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string VaiTro { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty;
        public DateTime NgayThangNamSinh { get; set; }
    }

    // ORDERING
    public class DonHangDto
    {
        public Guid MaDH { get; set; }
        public Guid MaTK { get; set; }
        public Guid? MaGG { get; set; }
        public DateTime NgayDat { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThaiDH { get; set; } = string.Empty;
        public string DiaChiGiaoHang { get; set; } = string.Empty;
        public List<ChiTietDonHangDto> ChiTietDonHangs { get; set; } = new();
    }

    public class ChiTietDonHangDto
    {
        public Guid MaCTDH { get; set; }
        public Guid MaCTSP { get; set; }
        public string TenSP_LuuTru { get; set; } = string.Empty;
        public string? Mau_LuuTru { get; set; }
        public string? KichCo_LuuTru { get; set; }
        public decimal Gia_LuuTru { get; set; }
        public int SoLuong { get; set; }
    }

    public class CreateDonHangRequest
    {
        public Guid MaTK { get; set; }
        public Guid? MaGG { get; set; }
        public string DiaChiGiaoHang { get; set; } = string.Empty;
        public List<CreateChiTietDonHangRequest> ChiTietDonHangs { get; set; } = new();
    }

    public class CreateChiTietDonHangRequest
    {
        public Guid MaCTSP { get; set; }
        public string TenSP_LuuTru { get; set; } = string.Empty;
        public string? Mau_LuuTru { get; set; }
        public string? KichCo_LuuTru { get; set; }
        public decimal Gia_LuuTru { get; set; }
        public int SoLuong { get; set; }
    }

    // DISCOUNT
    public class MaGiamGiaDto
    {
        public Guid MaGG { get; set; }
        public string MaCode { get; set; } = string.Empty;
        public string Loai { get; set; } = string.Empty;
        public decimal SoTien { get; set; }
        public decimal? GiaTriGiamToiDa { get; set; }
        public int SoLuong { get; set; }
        public DateTime HanSuDung { get; set; }
    }

    // CATALOG - Categories and Products
    public class DanhMucDto
    {
        public Guid MaDM { get; set; }
        public Guid MaLDM { get; set; }
        public string TenDM { get; set; } = string.Empty;
    }

    public class DanhMucCreateUpdateRequest
    {
        public Guid MaLDM { get; set; }
        public string TenDM { get; set; } = string.Empty;
    }

    public class ChiTietSanPhamDTO
    {
        public Guid MaCTSP { get; set; }
        public Guid MaSP { get; set; }
        public string Mau { get; set; } = string.Empty;
        public string KichCo { get; set; } = string.Empty;
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string? Anh { get; set; }
    }

    public class SanPhamDto
    {
        public Guid MaSP { get; set; }
        public Guid MaDM { get; set; }
        public string TenSP { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public List<ChiTietSanPhamDTO> ChiTietSanPhams { get; set; } = new();
    }

    public class SanPhamCreateRequest
    {
        public Guid MaDM { get; set; }
        public string TenSP { get; set; } = string.Empty;
        public string? MoTa { get; set; }
    }

    // BASKET
    public class BasketDto
    {
        public string UserName { get; set; } = string.Empty;
        public List<BasketItemDto> Items { get; set; } = new();
        public decimal TotalPrice { get; set; }
    }

    public class BasketItemDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }

    // CATALOG
    public class ChiTietSanPhamDto
    {
        public Guid MaCTSP { get; set; }
        public Guid MaSP { get; set; }
        public string Mau { get; set; } = string.Empty;
        public string KichCo { get; set; } = string.Empty;
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string? Anh { get; set; }
    }

    // CHAT
    public class ChatMessageDto
    {
        public Guid MaPhien { get; set; }
        public Guid SenderID { get; set; }
        public string SenderType { get; set; } = string.Empty;
        public string NoiDung { get; set; } = string.Empty;
        public DateTime ThoiGianGui { get; set; }
    }
}