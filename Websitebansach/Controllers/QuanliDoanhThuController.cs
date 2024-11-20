using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Websitebansach.Filters;
using Websitebansach.Models;

namespace Websitebansach.Controllers
{
    [AdminAuthorize]
    public class QuanliDoanhThuController : Controller
    {
        private QuanLyBanSachEntities db = new QuanLyBanSachEntities();


        

        // GET: QuanliDoanhThu
        public ActionResult ThongKeDoanhThu(DateTime? startDate, DateTime? endDate)
        {
            // Nếu không có tham số, mặc định là từ đầu năm đến ngày hiện tại
            startDate = startDate ?? new DateTime(DateTime.Now.Year, 1, 1);
            endDate = endDate ?? DateTime.Now;

            var doanhThu = db.ChiTietDonHangs
                             .Where(ct => ct.DonHang.DaThanhToan == 1
                                          && ct.DonHang.NgayDat >= startDate
                                          && ct.DonHang.NgayDat <= endDate) // lọc theo ngày đặt trong khoảng thời gian
                             .Sum(ct => ct.SoLuong * ct.DonGia); // tổng doanh thu = số lượng * đơn giá

            ViewBag.DoanhThu = doanhThu;
            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");

            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
