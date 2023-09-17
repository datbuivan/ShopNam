using System;
using System.Collections.Generic;

namespace QLBanQuanAoNam.Models;

public partial class HinhAnhSp
{
    public string TenFileAnh { get; set; } = null!;

    public string MaSanPham { get; set; } = null!;

    public int? ViTri { get; set; }

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
