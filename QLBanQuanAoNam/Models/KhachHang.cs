using System;
using System.Collections.Generic;

namespace QLBanQuanAoNam.Models;

public partial class KhachHang
{
    public string MaKhachHang { get; set; } = null!;

    public string? TenKhachHang { get; set; }

    public string? DiaChi { get; set; }

    public string? SoDienThoai { get; set; }

    public string? UserName { get; set; }

    public virtual ICollection<HoaDonBan> HoaDonBans { get; set; } = new List<HoaDonBan>();

    public virtual User? UserNameNavigation { get; set; }
}
