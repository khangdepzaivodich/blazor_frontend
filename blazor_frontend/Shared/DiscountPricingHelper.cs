using blazor_frontend.Models.BackendDTOs;

namespace blazor_frontend.Shared;

public static class DiscountPricingHelper
{
    public static MaGiamGiaDto? GetBestApplicableDiscount(
        SanPhamDto? product,
        decimal price,
        IReadOnlyCollection<LoaiDanhMucDto> loaiDanhMucs,
        IReadOnlyCollection<DanhMucDto> danhMucs,
        IEnumerable<MaGiamGiaDto> discounts)
    {
        if (product == null || price <= 0)
        {
            return null;
        }

        var currentCategory = danhMucs.FirstOrDefault(x => x.MaDM == product.MaDM);
        var currentLoaiDanhMucId = currentCategory?.MaLDM;

        return discounts
            .Where(discount => IsApplicable(discount, product, currentLoaiDanhMucId))
            .OrderByDescending(discount => GetDiscountAmount(discount, price))
            .FirstOrDefault();
    }

    public static bool IsApplicable(MaGiamGiaDto discount, SanPhamDto product, Guid? currentLoaiDanhMucId)
    {
        return discount.ApDungCho switch
        {
            "TatCa" => true,
            "TenLDM" => discount.MaLDM.HasValue && currentLoaiDanhMucId.HasValue && discount.MaLDM.Value == currentLoaiDanhMucId.Value,
            "TenDM" => discount.MaDM.HasValue && discount.MaDM.Value == product.MaDM,
            "SanPham" => (discount.MaSP.HasValue && discount.MaSP.Value == product.MaSP)
                || (discount.MaSPs?.Contains(product.MaSP) == true),
            _ => false
        };
    }

    public static string GetScopeDisplayText(MaGiamGiaDto item, IReadOnlyCollection<LoaiDanhMucDto> loaiDanhMucs, IReadOnlyCollection<DanhMucDto> danhMucs, IReadOnlyCollection<SanPhamDto> sanPhams)
    {
        return item.ApDungCho switch
        {
            "TenLDM" => $"Loại danh mục: {loaiDanhMucs.FirstOrDefault(x => x.MaLDM == item.MaLDM)?.TenLDM ?? item.MaLDM?.ToString() ?? "N/A"}",
            "TenDM" => $"Danh mục: {danhMucs.FirstOrDefault(x => x.MaDM == item.MaDM)?.TenDM ?? item.MaDM?.ToString() ?? "N/A"}",
            "SanPham" => item.MaSPs?.Count > 1
                ? $"Danh mục > Sản phẩm: {item.MaSPs.Count} sản phẩm"
                : $"Sản phẩm: {sanPhams.FirstOrDefault(x => x.MaSP == ((item.MaSPs ?? new List<Guid>()).FirstOrDefault() != Guid.Empty ? item.MaSPs!.FirstOrDefault() : item.MaSP))?.TenSP ?? item.MaSP?.ToString() ?? "N/A"}",
            _ => "Tất cả"
        };
    }

    public static IEnumerable<MaGiamGiaDto> GetApplicableDiscounts(
        SanPhamDto? product,
        decimal price,
        IReadOnlyCollection<LoaiDanhMucDto> loaiDanhMucs,
        IReadOnlyCollection<DanhMucDto> danhMucs,
        IEnumerable<MaGiamGiaDto> discounts)
    {
        if (product == null || price <= 0)
        {
            return Array.Empty<MaGiamGiaDto>();
        }

        var currentCategory = danhMucs.FirstOrDefault(x => x.MaDM == product.MaDM);
        var currentLoaiDanhMucId = currentCategory?.MaLDM;

        return discounts
            .Where(discount => IsApplicable(discount, product, currentLoaiDanhMucId))
            .OrderByDescending(discount => GetDiscountAmount(discount, price))
            .ThenBy(discount => discount.MaCode);
    }

    public static IEnumerable<MaGiamGiaDto> GetApplicableDiscountsForCart(
        IEnumerable<BasketItemDto> items,
        IReadOnlyDictionary<string, SanPhamDto> productById,
        IReadOnlyCollection<LoaiDanhMucDto> loaiDanhMucs,
        IReadOnlyCollection<DanhMucDto> danhMucs,
        IEnumerable<MaGiamGiaDto> discounts)
    {
        var matched = new List<MaGiamGiaDto>();

        foreach (var item in items)
        {
            if (!productById.TryGetValue(item.ProductId, out var product))
            {
                continue;
            }

            var currentCategory = danhMucs.FirstOrDefault(x => x.MaDM == product.MaDM);
            var currentLoaiDanhMucId = currentCategory?.MaLDM;

            matched.AddRange(discounts.Where(discount => IsApplicable(discount, product, currentLoaiDanhMucId)));
        }

        return matched
            .GroupBy(x => x.MaGG)
            .Select(g => g.First())
            .OrderByDescending(x => x.SoTien)
            .ThenBy(x => x.MaCode);
    }

    public static decimal GetDiscountAmount(MaGiamGiaDto discount, decimal price)
    {
        if (price <= 0)
        {
            return 0;
        }

        return discount.Loai switch
        {
            "PhanTram" => Math.Min(price * discount.SoTien / 100m, discount.GiaTriGiamToiDa ?? decimal.MaxValue),
            "Tien" => Math.Min(discount.SoTien, price),
            _ => 0
        };
    }

    public static decimal GetFinalPrice(MaGiamGiaDto? discount, decimal price)
        => discount == null ? price : Math.Max(price - GetDiscountAmount(discount, price), 0);
}
