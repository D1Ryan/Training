using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SAPI.Models;
using SAPI.Common;

namespace SAPI.Controllers
{
    public class SalesController : BaseController
    {
        // GET: Sales
        public ActionResult Index()
        {
            ViewBag.Title = "销售人员管理";
            ViewBag.ImgPrix = SalesImgUrl;
            return View(db.App_Sales.ToList());
        }

        // GET: Sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Content(ScriptHelper.ShowAlert(DataWasNoDefined));
            }
            var sas = db.App_Sales.Where(w => w.Sales_Id.Equals(id.Value));
            if (sas.Any())
            {
                var sa = sas.First();
                sa.QrCode = SalesImgUrl + sa.QrCode;
                return View(sa);
            }
            else
            {
                ViewBag.Title = DataWasDisabledOrDoesNotExist;
                return Content(ScriptHelper.ShowAlert(DataWasDisabledOrDoesNotExist));
            }
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            var sa = new App_Sales();
            sa.Sales_Type = 1;
            sa.Is_Enable = true;
            return View(sa);
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,English_Name,Sales_Type,Mail,Phone,Private_Code,QrCode,Summary,Is_Enable,Remark")] App_Sales app_Sales)
        {
            if (ModelState.IsValid)
            {
                app_Sales.Create_Time = DateTime.Now;
                db.App_Sales.Add(app_Sales);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(app_Sales);
        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Content(ScriptHelper.ShowAlert(DataWasNoDefined));
            }
            var sas = db.App_Sales.Where(w => w.Sales_Id.Equals(id.Value));
            if (sas.Any())
            {
                var sa = sas.First();
                sa.QrCode = SalesImgUrl + sa.QrCode;
                return View(sa);
            }
            else
            {
                return Content(ScriptHelper.ShowAlert(DataWasDisabledOrDoesNotExist));
            }
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Sales_Id,Name,English_Name,Sales_Type,Mail,Phone,Private_Code,QrCode,Summary,Is_Enable,Remark")] App_Sales app_Sales)
        {
            if (ModelState.IsValid)
            {
                //全字段更新
                db.Entry(app_Sales).State = EntityState.Modified;
                //指定不更新创建日期
                DbEntityEntry<App_Sales> entry = db.Entry(app_Sales);
                entry.Property(t => t.Create_Time).IsModified = false;
                app_Sales.Update_Time = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(app_Sales);
        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Content(ScriptHelper.ShowAlert(DataWasNoDefined));
            }
            App_Sales app_Sales = db.App_Sales.Find(id);
            if (app_Sales == null)
            {
                ViewBag.Title = DataWasDisabledOrDoesNotExist;
                return View();
            }
            return View(app_Sales);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            App_Sales app_Sales = db.App_Sales.Find(id);
            db.App_Sales.Remove(app_Sales);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Sales/GetQRBySId/5
        /// <summary>
        /// 通过销售编号获得二维码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetQRBySId(int? id)
        {
            if (id == null)
            {
                return "{\"code\":\"1\", \"msg\":\"获取失败\",\"data\":\"" + DataWasNoDefined + "\"}";
            }
            App_Sales sa = db.App_Sales.Find(id);
            if (sa == null)
            {
                return "{\"code\":\"2\", \"msg\":\"获取失败\",\"data\":\"" + DataWasDisabledOrDoesNotExist + "\"}";
            }
            return "{\"code\":\"0\", \"msg\":\"获取成功\",\"data\":\"" + sa.QrCode + "\",\"url\":\"" + SalesImgUrl + sa.QrCode + "\"}";
        }

        // POST: Sales/UpLoadFile
        /// <summary>
        /// 上传销售二维码
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpLoadFile(HttpPostedFileBase fileData)
        {
            if (fileData != null)
            {

                string isImg = System.IO.Path.GetExtension(fileData.FileName).ToString().ToLower();
                if (isImg != ".jpg" && isImg != ".png")
                {
                    return "{\"code\":\"1\", \"msg\":\"图片仅支持,.png|.jpg\"}";
                }
                UploadResult up = FileHelper.GetUploadFile(fileData, "Images");
                if (string.IsNullOrEmpty(up.ErrMsg))
                {
                    return "{\"code\":\"0\", \"msg\":\"上传成功\",\"data\":\"" + up.FileName + "\",\"url\":\"" + up.HttpPath + "\"}";
                }
                else
                {
                    return "{\"code\":\"2\", \"msg\":\"上传失败\",\"data\":\"" + up.ErrMsg + "\"}";
                }
            }
            else
            {
                return "{\"code\":\"3\", \"msg\":\"请选择要上传的文件\"}";
            }
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
