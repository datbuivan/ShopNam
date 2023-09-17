using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using QLBanQuanAoNam.Models;

namespace QLBanQuanAoNam.Controllers
{
    public class AccessController : Controller
    {
        QlshopNamContext db = new QlshopNamContext();

        public IActionResult Login()
        {
            if ( HttpContext.Session.GetString("UserName") == null)
            {
                    return View();
            }
            else
            {
                  return RedirectToAction("Index", "Home");
            }
        }



        [HttpPost]
        public IActionResult Login(User user)
        {
            if (HttpContext.Session.GetString("UseHome") == null)
            {
                var u = db.Users.Where(x => x.UserName == user.UserName && x.PassWord == user.PassWord).FirstOrDefault();
                var role = db.Users.Where(x => x.UserName == user.UserName);

                if (u != null)
                {
                        HttpContext.Session.SetString("UserName", u.UserName.ToString());
                        HttpContext.Session.SetString("Role", u.Role.ToString());
                        if(HttpContext.Session.GetString("Role") == "0")
                        {
                            return RedirectToAction("HomeAdmin", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                }
            }
            
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login", "access");
            }


            return View(user);
        }


        public IActionResult Logout()
            {
                HttpContext.Session.Clear();
                HttpContext.Session.Remove("UserName");
                return RedirectToAction("Login", "access");
            }
        }
}
