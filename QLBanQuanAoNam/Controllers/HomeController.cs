using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBanQuanAoNam.Models;
using QLBanQuanAoNam.Models.Authentication;
using QLBanQuanAoNam.Models.CartModels;
using QLBanQuanAoNam.Models.ViewModels;
using System.Data.Entity;
using System.Diagnostics;
using X.PagedList;

namespace QLBanQuanAoNam.Controllers
{
	[Authentication]
	public class HomeController : Controller
    {
        QlshopNamContext db = new QlshopNamContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

		public IActionResult Index(int? page , string? search)
		{
			int pageNumber = page == null || page < 1 ? 1 : page.Value;
			int pageSize = 8;
			var lstSanpham = from sp in db.SanPhams select sp;
			if (!String.IsNullOrEmpty(search))
			{
				lstSanpham = lstSanpham.Where(x => x.TenSanPham.Contains(search));
			}
			PagedList<SanPham> lst = new PagedList<SanPham>(lstSanpham, pageNumber, pageSize);
			ViewBag.page = lst.PageCount;
			return View(lst);
		}
		public IActionResult SanPhamTheoLoai(int? page, string maLoaiSp, int? giaMax, int? giaMin, string? sort)
		{
			int pageNumber = page == null || page < 1 ? 1 : page.Value;
			int pageSize = 8;
			var lstSanpham = from sp in db.SanPhams select sp;
			if (maLoaiSp != null)
			{
				lstSanpham = lstSanpham.Where(x => x.MaLoaiSp == maLoaiSp);
				ViewBag.maLoaiSp = maLoaiSp;
			}
			if (giaMax != null && giaMin != null)
			{
				lstSanpham = lstSanpham.Where(x => x.GiaBan >= giaMin && x.GiaBan <= giaMax);
				ViewBag.giaMax = giaMax;
				ViewBag.giaMin = giaMin;
			}
			if (sort != null)
			{
				if (sort == "giamDan") { lstSanpham = lstSanpham.OrderByDescending(x => x.GiaBan); ViewBag.sort = sort; }
				if (sort == "tangDan") { lstSanpham = lstSanpham.OrderBy(x => x.GiaBan); ViewBag.sort = sort; }
			}
			PagedList<SanPham> lst = new PagedList<SanPham>(lstSanpham, pageNumber, pageSize);
			ViewBag.page = lst.PageCount;

			return View(lst);
		}

		public IActionResult ChiTietSanPham(string maSanPham)
		{
			var anhSanPham = db.HinhAnhSps.Where(x => x.MaSanPham == maSanPham).ToList();
			ViewBag.anhSanPham = anhSanPham;
			var kichthuoc = (from kt in db.KichThuocs
							 join ctsp in db.ChiTietSanPhams on kt.MaKichThuoc equals ctsp.MaKichThuoc
							 where ctsp.MaSanPham == maSanPham
							 select kt).Distinct().ToList();
			ViewBag.KT = kichthuoc;
			var mausac = (from ms in db.MauSacs
						  join ctsp in db.ChiTietSanPhams on ms.MaMauSac equals ctsp.MaMauSac
						  where ctsp.MaSanPham == maSanPham
						  select ms).Distinct().ToList();
			ViewBag.MS=mausac;
			var sanpham = db.SanPhams.SingleOrDefault(x => x.MaSanPham == maSanPham);
			if (sanpham == null)
			{
				return RedirectToAction("Index");
			}
			else
			{
				return View(sanpham);
			}
		}

		//public IActionResult ChiTietSanPham(string maSanPham)
		//{
		//	var anhSanPham = db.HinhAnhSps.Where(x => x.MaSanPham == maSanPham).ToList();
		//	ViewBag.anhSanPham = anhSanPham;
		//	var sanpham = db.SanPhams.SingleOrDefault(x => x.MaSanPham == maSanPham);
		//	var kichthuoc = (from kt in db.KichThuocs
		//					 join ctsp in db.ChiTietSanPhams on kt.MaKichThuoc equals ctsp.MaKichThuoc
		//					 where ctsp.MaSanPham == maSanPham
		//					 select kt).Distinct().ToList();
		//	var mausac = (from ms in db.MauSacs
		//					 join ctsp in db.ChiTietSanPhams on ms.MaMauSac equals ctsp.MaMauSac
		//					 where ctsp.MaSanPham == maSanPham
		//					 select ms).Distinct().ToList();
		//	var sanPhamCTSPViewModel = new SanPhamCTSPViewModel
		//	{
		//		sanPham = sanpham,
		//		kichThuoc = kichthuoc,
		//		mauSac = mausac
		//	};
		//	return View(sanPhamCTSPViewModel);
		//}
		public IActionResult Shop(int? page, string? search)
		{
            int pageNumber = page == null || page < 1 ? 1 : page.Value;
            int pageSize = 8;
            var lstSanpham = from sp in db.SanPhams select sp;
            if (!String.IsNullOrEmpty(search) )
            {
                lstSanpham = lstSanpham.Where(x => x.TenSanPham.Contains(search));
            }
            PagedList<SanPham> lst = new PagedList<SanPham>(lstSanpham, pageNumber, pageSize);
            ViewBag.page = lst.PageCount;
            return View(lst);
        }

		public IActionResult SanPhamTheoGia(int? page , int? giaMax , int? giaMin)
		{
			int pageNumber = page == null || page < 1 ? 1 : page.Value;
			int pageSize = 8;
			var lstSanpham = from sp in db.SanPhams select sp;
			if(giaMax !=null && giaMin != null)
			{
				lstSanpham = lstSanpham.Where(x=>x.GiaBan >=giaMin && x.GiaBan <=giaMax);
			}
			else
			{
				if (giaMin != null && giaMax == null)
				{
					lstSanpham = lstSanpham.Where(x => x.GiaBan >= giaMin);
				}
			}
			PagedList<SanPham> lst = new PagedList<SanPham>(lstSanpham, pageNumber, pageSize);
			return View(lst);

		}


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}