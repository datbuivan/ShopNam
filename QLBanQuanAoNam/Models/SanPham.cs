using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace QLBanQuanAoNam.Models;

public partial class SanPham
{
    public string MaSanPham { get; set; } = null!;

    public string? TenSanPham { get; set; }

    public string? ChatLieu { get; set; }

    public double? GiaNhap { get; set; }

    public double? GiaBan { get; set; }

    public string? HinhAnhAvatar { get; set; }

    public string? MaLoaiSp { get; set; }

    public double? Vote { get ; set;  }

    public int? Slvote { get ;  set  ; }

    public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; } = new List<ChiTietSanPham>();

    public virtual ICollection<HinhAnhSp> HinhAnhSps { get; set; } = new List<HinhAnhSp>();

    public virtual LoaiSp? MaLoaiSpNavigation { get; set; }
    [Display(Name = "File Upload")]
    [NotMapped]
    public IFormFile? FileUpload { get; set; }
}
