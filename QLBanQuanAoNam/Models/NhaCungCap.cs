using System;
using System.Collections.Generic;

namespace QLBanQuanAoNam.Models;

public partial class NhaCungCap
{
    public string MaNhaCungCap { get; set; } = null!;

    public string? TenNhaCungCap { get; set; }

    public string? DiaChi { get; set; }

    public string? DienThoai { get; set; }

    public virtual ICollection<HoaDonNhap> HoaDonNhaps { get; set; } = new List<HoaDonNhap>();
}
