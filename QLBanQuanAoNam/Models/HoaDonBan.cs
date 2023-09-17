using System;
using System.Collections.Generic;

namespace QLBanQuanAoNam.Models;

public partial class HoaDonBan
{
    public string MaHoaDonBan { get; set; } = null!;

    public DateTime? NgayBan { get; set; }

    public string? MaKhachHang { get; set; }

    public string? MaNhanVien { get; set; }

    public double? TongTien { get; set; }

    public virtual ICollection<ChiTietHoaDonBan> ChiTietHoaDonBans { get; set; } = new List<ChiTietHoaDonBan>();

    public virtual KhachHang? MaKhachHangNavigation { get; set; }

    public virtual NhanVien? MaNhanVienNavigation { get; set; }
}
