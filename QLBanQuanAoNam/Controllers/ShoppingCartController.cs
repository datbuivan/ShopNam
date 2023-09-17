using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBanQuanAoNam.Models;
using QLBanQuanAoNam.Models.Authentication;
using QLBanQuanAoNam.Models.CartModels;

namespace QLBanQuanAoNam.Controllers
{
	[Authentication]
	public class ShoppingCartController : Controller
    {
        QlshopNamContext db = new QlshopNamContext();
        public const string CARTKEY = "cart";
        // Lấy cart từ Session (danh sách CartItem)
        List<CartItem> GetCartItems()
        {

            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }
        /// Thêm sản phẩm vào cart
        public IActionResult AddToCart(string maSanPham, IFormCollection Form)
        {

            var sanpham = db.SanPhams
                .Where(p => p.MaSanPham == maSanPham)
                .FirstOrDefault();
			if (sanpham == null)
                return NotFound("Không có sản phẩm");
            string maKT = Form["kichthuocsl"];
            string maMS = Form["mausacsl"];
            if(maKT == null && maMS == null)
            {
                return NotFound("khong co mau sac kich thuoc");
            }
            // Xử lý đưa vào Cart ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.sanpham.MaSanPham == maSanPham);
            var cartitemkt = cart.Find(p=>p.maKT== maKT);
            var cartitemms = cart.Find(p=>p.maMS== maMS);
            if (cartitem != null && cartitemkt!=null && cartitemms!=null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartItem() { quantity = 1, sanpham = sanpham, maKT = maKT , maMS = maMS});
            }

            // Lưu cart vào Session
            SaveCartSession(cart);
            // Chuyển đến trang hiện thị Cart
            return RedirectToAction("Cart","ShoppingCart");
        }
        /// xóa item trong cart
        public IActionResult RemoveCart(string maSanPham)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.sanpham.MaSanPham == maSanPham);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            return RedirectToAction("Cart","ShoppingCart");
        }
        /// Cập nhật
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart(string maSanPham, int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.sanpham.MaSanPham == maSanPham);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity = quantity;
            }
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }
        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {
            return View(GetCartItems());
        }
        public IActionResult CheckOut()
        {
			var cart = HttpContext.Session.GetString(CARTKEY);
			var list = new List<CartItem>();
			if (cart != null)
			{
				list = JsonConvert.DeserializeObject<List<CartItem>>(cart);
			}
			string dem = ((from hdb in db.HoaDonBans select hdb.MaHoaDonBan).Count()+1).ToString();
            HoaDonBan hoaDonBan = new HoaDonBan();
            string maHoaDonBan = "";
            if (Int32.Parse(dem) < 10)
            {
                maHoaDonBan= "HDB0" + dem;	
            }
            else
            {
				maHoaDonBan = "HDB" + dem;
			}
            hoaDonBan.MaHoaDonBan = maHoaDonBan;
            hoaDonBan.NgayBan = DateTime.Now;
            db.HoaDonBans.Add(hoaDonBan);
            db.SaveChanges();
            Double TongTien = 0;
            foreach(var item in list)
            {
                var thanhtien = item.sanpham.GiaBan.Value * item.quantity;
                TongTien += thanhtien; 
                ChiTietHoaDonBan chiTietBan = new ChiTietHoaDonBan();
                chiTietBan.MaHoaDonBan = maHoaDonBan;
                chiTietBan.SoLuong = item.quantity;
                chiTietBan.MaSanPham = item.sanpham.MaSanPham;
                chiTietBan.DonGiaBan = item.sanpham.GiaBan;
                chiTietBan.MaMauSac = item.maMS;
                chiTietBan.MaKichThuoc = item.maKT;
                db.ChiTietHoaDonBans.Add(chiTietBan);
			}
            HoaDonBan hdban = db.HoaDonBans.FirstOrDefault(x => x.MaHoaDonBan == hoaDonBan.MaHoaDonBan);
            hdban.TongTien = TongTien;
            db.SaveChanges();
            ClearCart();
            return Content("Da gui don hang");
        }
        
    }
}
