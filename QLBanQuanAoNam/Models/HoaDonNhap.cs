using System;
using System.Collections.Generic;

namespace QLBanQuanAoNam.Models;

public partial class HoaDonNhap
{
    public string MaHoaDonNhap { get; set; } = null!;

    public DateOnly? NgayNhap { get; set; }

    public string? MaNhaCungCap { get; set; }

    public string? MaNhanVien { get; set; }

    public double? TongTien { get; set; }

    public virtual ICollection<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; } = new List<ChiTietHoaDonNhap>();

    public virtual NhaCungCap? MaNhaCungCapNavigation { get; set; }

    public virtual NhanVien? MaNhanVienNavigation { get; set; }
}
