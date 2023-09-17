using System;
using System.Collections.Generic;

namespace QLBanQuanAoNam.Models;

public partial class ChiTietSanPham
{
    public string MaSanPham { get; set; } = null!;

    public string MaMauSac { get; set; } = null!;

    public string MaKichThuoc { get; set; } = null!;

    public int? SoLuong { get; set; }

    public virtual ICollection<ChiTietHoaDonBan> ChiTietHoaDonBans { get; set; } = new List<ChiTietHoaDonBan>();

    public virtual ICollection<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; } = new List<ChiTietHoaDonNhap>();

    public virtual KichThuoc MaKichThuocNavigation { get; set; } = null!;

    public virtual MauSac MaMauSacNavigation { get; set; } = null!;

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
