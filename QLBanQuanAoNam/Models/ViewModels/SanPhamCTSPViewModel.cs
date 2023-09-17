
using System.Reflection.Metadata;

namespace QLBanQuanAoNam.Models.ViewModels
{
    public class SanPhamCTSPViewModel
    {
        QlshopNamContext db = new QlshopNamContext();
        public SanPham sanPham { get; set; }
        public List<KichThuoc> kichThuoc { get; set; }
        public List<MauSac> mauSac { get; set; }
    }
}
