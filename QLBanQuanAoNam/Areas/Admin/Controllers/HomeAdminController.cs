using Azure;
using JetBrains.Annotations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QLBanQuanAoNam.Areas.Admin.ViewModels;
using QLBanQuanAoNam.Models;
using QLBanQuanAoNam.Models.Authentication;
using System.Data.Entity;
using System.Reflection.Metadata;
using X.PagedList;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace QLBanQuanAoNam.Areas.Admin.Controllers
{
	[Authentication]
	[Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        private readonly IWebHostEnvironment _webhost;
        public HomeAdminController(IWebHostEnvironment webhost)
        {
            _webhost = webhost;
        }
        QlshopNamContext db = new QlshopNamContext();
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
		{
			return View();
		}

        [Route("DanhSachSanPham")]
        public IActionResult DanhSachSanPham(int? page)
        {
            int pageNumber = page == null || page < 1 ? 1 : page.Value;
            int pageSize = 8;
            var lstSanpham = db.SanPhams.AsNoTracking().OrderBy(x => x.MaSanPham);
            PagedList<SanPham> lst = new PagedList<SanPham>(lstSanpham, pageNumber, pageSize);
            ViewBag.page = lst.PageCount;
            return View(lst);
        }
        [Route("ThemSanPham")]
        public IActionResult ThemSanPham()
        {
            ViewBag.MaLoaiSp = new SelectList(db.LoaiSps.ToList(),"MaLoaiSp","TenLoaiSp");
            return View();
        }
        [Route("ThemSanPham")]
        [HttpPost]
        public IActionResult ThemSanPham(SanPham sanPham)
        {
            bool checkTonTai = db.SanPhams.Any(x => x.MaSanPham == sanPham.MaSanPham);
            if (checkTonTai)
            {
                ViewBag.Msg = "Sản phẩm đã tồn tại trong cơ sở dữ liệu";
                return RedirectToAction("ThemSanPham", "Admin");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.SanPhams.Add(sanPham);
                    db.SaveChanges();
                    if (sanPham.FileUpload != null)
                    {
                        string masp = sanPham.MaSanPham.ToString(); 
                        var filePath = Path.Combine(_webhost.WebRootPath, "Products/Images",sanPham.FileUpload.FileName);
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        sanPham.FileUpload.CopyTo(fileStream);
                        string _FileName = sanPham.FileUpload.FileName;
                        SanPham sp = db.SanPhams.FirstOrDefault(x => x.MaSanPham == masp);
                        sp.HinhAnhAvatar = _FileName;
                        db.SaveChanges() ;
                    }
                    return RedirectToAction("DanhSachSanPham","Admin");
                }
                return View(sanPham);
            }
            
        }
        [Route("SuaSanPham")]
        public IActionResult SuaSanPham(string maSanPham)
        {
            ViewBag.MaLoaiSp = new SelectList(db.LoaiSps.ToList(), "MaLoaiSp", "TenLoaiSp");
            SanPham sanPham = db.SanPhams.SingleOrDefault(x=>x.MaSanPham==maSanPham);
            return View(sanPham);
        }

        [Route("SuaSanPham")]
        [HttpPost]
        public IActionResult SuaSanPham(SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                if(sanPham.FileUpload !=null)
                {
                    string masp = sanPham.MaSanPham.ToString();
                    var filePath = Path.Combine(_webhost.WebRootPath, "Products/Images", sanPham.FileUpload.FileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    sanPham.FileUpload.CopyTo(fileStream);
                    string _FileName = sanPham.FileUpload.FileName;
                    SanPham sp = db.SanPhams.FirstOrDefault(x => x.MaSanPham == masp);
                    sp.HinhAnhAvatar = _FileName;
                }
                db.SaveChanges();
                return RedirectToAction("DanhSachSanPham", "Admin");
            }
            return View(sanPham);
        }
        [Route("XoaSanPham")]
        [HttpGet]
        public IActionResult XoaSanPham(string maSanPham)
        {
            TempData["Message"] = "";
            var listChiTiet = db.ChiTietSanPhams.Where(x=>x.MaSanPham == maSanPham).ToList();
            foreach (var item in listChiTiet)
            {
                if (db.ChiTietSanPhams.Where(x => x.MaSanPham == item.MaSanPham) != null)
                {
                    TempData["Message"] = "Khong xoa duoc san pham";
                    return RedirectToAction("DanhSachSanPham");
                }
            }
            var listAnh = db.HinhAnhSps.Where(x => x.MaSanPham == maSanPham);
            if (listAnh != null) db.RemoveRange(listAnh);
            if (listChiTiet != null) db.RemoveRange(listChiTiet);
            if (maSanPham != null)
            {
                db.RemoveRange(db.SanPhams.Find(maSanPham));
            }

            db.SaveChanges();
            TempData["Message"] = "San pham da duoc xoa";
            return RedirectToAction("DanhSachSanPham");
        }
        [Route("ThemChiTietSanPham")]
        public IActionResult ThemChiTietSanPham(string maSanPham)
        {
            List<ChiTietSanPham> lstChiTietSanPham = db.ChiTietSanPhams.Where(x=>x.MaSanPham==maSanPham).ToList();  
            ViewBag.CT= lstChiTietSanPham;
            SanPham sp = db.SanPhams.SingleOrDefault(x=>x.MaSanPham==maSanPham);
            ViewBag.SP = sp;
			List<MauSac> lstms = db.MauSacs.ToList();
			ViewBag.MS = lstms;
			List<KichThuoc> lstkt = db.KichThuocs.ToList();
			ViewBag.KT = lstkt;
			int c1 = db.MauSacs.Count();
			int c2 = db.KichThuocs.Count();
			ViewBag.Count = c1 * c2;
			return View();
		}
        [Route("ThemChiTietSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemChiTietSanPham(CTSPViewModel ctsp)
        {
			if (ctsp.ChiTietSanPham == null || !ctsp.ChiTietSanPham.Any())
			{
				ViewBag.Msg = "Hãy chọn ít nhất một chi tiết sản phẩm!";
			}
			else
			{
				db.ChiTietSanPhams.AddRange(ctsp.ChiTietSanPham);
				int c = db.SaveChanges();
				if (c > 0)
				{
					return RedirectToAction("DanhSachSanPham");

				}
			}
			List<ChiTietSanPham> lstct = db.ChiTietSanPhams.Where(x => x.MaSanPham == ctsp.MaSanPham).ToList();
			ViewBag.CT = lstct;
			SanPham sp = db.SanPhams.SingleOrDefault(x => x.MaSanPham == ctsp.MaSanPham);
			ViewBag.SP = sp;
			List<MauSac> lstms = db.MauSacs.ToList();
			ViewBag.MS = lstms;
			List<KichThuoc> lstkt = db.KichThuocs.ToList();
			ViewBag.KT = lstkt;
			int c1 = db.MauSacs.Count();
			int c2 = db.KichThuocs.Count();
			ViewBag.Count = c1 * c2;
			return View(ctsp);
		}
 
    }
}
