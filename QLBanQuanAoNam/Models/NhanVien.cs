using System;
using System.Collections.Generic;

namespace QLBanQuanAoNam.Models;

public partial class NhanVien
{
    public string MaNhanVien { get; set; } = null!;

    public string? TenNhanVien { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string? UserName { get; set; }

    public virtual ICollection<HoaDonBan> HoaDonBans { get; set; } = new List<HoaDonBan>();

    public virtual ICollection<HoaDonNhap> HoaDonNhaps { get; set; } = new List<HoaDonNhap>();

    public virtual User? UserNameNavigation { get; set; }
}
