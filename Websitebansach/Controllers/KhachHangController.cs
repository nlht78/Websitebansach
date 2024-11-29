using System;
using System.Web.Mvc;
using Websitebansach.Models;
using System.Linq;

namespace YourNamespace.Controllers
{
    public class KhachHangController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();

        // GET: KhachHang/DangKy
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        // POST: KhachHang/DangKy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(KhachHang kh, string XacNhanMatKhau)
        {
            if (ModelState.IsValid)
            {
                if (kh.MatKhau != XacNhanMatKhau)
                {
                    ModelState.AddModelError("XacNhanMatKhau", "Xác nhận mật khẩu không khớp.");
                    return View(kh);
                }

                kh.quyen = 0; // Mặc định quyền là 0 (người dùng)
                db.KhachHangs.Add(kh);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đăng ký thành công";
                return RedirectToAction("DangNhap", "KhachHang");
            }

            return View(kh);
        }

        // GET: KhachHang/DangNhap
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        // POST: KhachHang/DangNhap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(string TaiKhoan, string MatKhau)
        {
            var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.TaiKhoan == TaiKhoan && kh.MatKhau == MatKhau);
            if (khachHang != null)
            {
                Session["KhachHang"] = khachHang;
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Tên tài khoản hoặc mật khẩu không đúng.");
            return View();
        }

        // GET: KhachHang/DangXuat
        public ActionResult DangXuat()
        {
            Session["KhachHang"] = null;
            return RedirectToAction("Index", "Home");
        }

        // GET: KhachHang/ChinhSua
        [HttpGet]
        public ActionResult ChinhSua()
        {
            if (Session["KhachHang"] == null)
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }

            KhachHang kh = (KhachHang)Session["KhachHang"];
            return View(kh);
        }

        // POST: KhachHang/ChinhSua
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua(KhachHang updatedKhachHang)
        {
            if (Session["KhachHang"] == null)
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }

            if (ModelState.IsValid)
            {
                KhachHang currentKhachHang = (KhachHang)Session["KhachHang"];
                KhachHang khFromDb = db.KhachHangs.Find(currentKhachHang.MaKH);

                if (khFromDb != null)
                {
                    // Cập nhật thông tin
                    khFromDb.HoTen = updatedKhachHang.HoTen;
                    khFromDb.DiaChi = updatedKhachHang.DiaChi;
                    khFromDb.Email = updatedKhachHang.Email;
                    khFromDb.DienThoai = updatedKhachHang.DienThoai;

                    db.SaveChanges();

                    // Cập nhật lại thông tin trong session
                    Session["KhachHang"] = khFromDb;

                    TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Không tìm thấy tài khoản.");
                }
            }

            return View(updatedKhachHang);
        }
    }
}
