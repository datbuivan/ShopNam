using System.Security;

namespace QLBanQuanAoNam.Models.CartModels
{
    public class CartItem
    {
        public SanPham sanpham { get; set; }
        public int quantity { get; set; } 
        public string maKT { get; set; }
        public string maMS { get; set; }
    }
    
}
