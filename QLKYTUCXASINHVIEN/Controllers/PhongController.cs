using QLKYTUCXASINHVIEN.Models; // Đảm bảo đúng namespace của bạn
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

public class PhongController : Controller
{
    // Phải khai báo db ở đây và đúng kiểu KTXContext
    private KTXContext db = new KTXContext();

    public ActionResult Index(string search, string toaNha, string tinhTrang)
    {
        var phongs = db.Phongs.AsQueryable();

        // Xử lý lọc (giữ nguyên code cũ của bạn)
        if (!string.IsNullOrEmpty(search))
        {
            phongs = phongs.Where(p => p.SoPhong.Contains(search) || p.ToAnha.Contains(search));
        }

        ViewBag.DanhSachPhong = phongs.ToList();
        return View();
    }

    // GET: Phong/Edit/A101
    public ActionResult Edit(string id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        // Tìm phòng theo mã phòng (string)
        Phong phong = db.Phongs.Find(id);

        if (phong == null)
        {
            return HttpNotFound();
        }
        return View(phong);
    }

    // POST: Phong/Edit/A101
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "MaPhong,SoPhong,LoaiPhong,GiaPhong,TrangThai,ToaNha")] Phong phong)
    {
        if (ModelState.IsValid)
        {
            // Đánh dấu là đã thay đổi và lưu vào database
            db.Entry(phong).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(phong);
    }



}

