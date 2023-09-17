using System;
using System.Collections.Generic;

namespace QLBanQuanAoNam.Models;

public partial class MauSac
{
    public string MaMauSac { get; set; } = null!;

    public string? TenMauSac { get; set; }

    public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; } = new List<ChiTietSanPham>();
}
