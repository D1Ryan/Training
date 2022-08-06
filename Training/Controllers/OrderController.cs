using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SAPI.Models;

namespace SAPI.Controllers
{
    public class OrderController : Controller
    {
        private readonly ScheduleEntities db = new ScheduleEntities();

        /// <summary>
        /// 初始页面
        /// </summary>
        /// <param name="ResourceId"></param>
        /// <param name="Mobile"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        // GET: App_Order
        public ActionResult Index(int? ResourceId, string Mobile = "", string StartDate = "", string EndDate = "")
        {
            //默认为当日
            if (string.IsNullOrWhiteSpace(StartDate))
            {
                StartDate = DateTime.Today.ToString("yyyy-MM-dd");
                EndDate = StartDate;
            }
            ViewBag.Title = "订单查询";
            OrderList olist = new OrderList();
            olist.ResourceId = ResourceId;
            olist.Mobile = Mobile;
            olist.StartDate = StartDate;
            olist.EndDate = EndDate;
            olist.ResourceList = db.App_Resource.Select(row => new SelectListItem() { Text = row.Title, Value = row.RowId.ToString(), Selected = row.RowId.Equals(ResourceId.Value) }).ToList();
            olist.AllList = SAPI.DAL.OrderDal.QueryOrderList(olist);
            // db.App_Order.OrderBy(o => o.RowId).ToList();
            return View(olist);
        }

        // GET: App_Order/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            App_Order app_Order = db.App_Order.Find(id);
            if (app_Order == null)
            {
                return HttpNotFound();
            }
            return View(app_Order);
        }

        //// GET: App_Order/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: App_Order/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "RowId,OrderId,UserId,UnionId,OpenId,UserName,ResourceId,Resource_Type,Title,Purchase_Name,Price,Cash_Pay,Card_Pay,Discount,Paid_Amount,NoPaid_Amount,No_Invoiced,Invoiced_Amount,Show_City,Show_Address,Show_Time,Order_Status,Audit_Status,PayId,Pay_Status,Pay_Way,Pay_Time,Refund_Time,Settle_Time,Expire_Time,Client_Type,User_Phone,User_Mail,Creator,Auditor,Create_Time,Update_Time,Audit_Time,Remark")] App_Order app_Order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.App_Order.Add(app_Order);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(app_Order);
        //}

        //// GET: App_Order/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    App_Order app_Order = db.App_Order.Find(id);
        //    if (app_Order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(app_Order);
        //}

        //// POST: App_Order/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "RowId,OrderId,UserId,UnionId,OpenId,UserName,ResourceId,Resource_Type,Title,Purchase_Name,Price,Cash_Pay,Card_Pay,Discount,Paid_Amount,NoPaid_Amount,No_Invoiced,Invoiced_Amount,Show_City,Show_Address,Show_Time,Order_Status,Audit_Status,PayId,Pay_Status,Pay_Way,Pay_Time,Refund_Time,Settle_Time,Expire_Time,Client_Type,User_Phone,User_Mail,Creator,Auditor,Create_Time,Update_Time,Audit_Time,Remark")] App_Order app_Order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(app_Order).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(app_Order);
        //}

        //// GET: App_Order/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    App_Order app_Order = db.App_Order.Find(id);
        //    if (app_Order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(app_Order);
        //}

        //// POST: App_Order/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    App_Order app_Order = db.App_Order.Find(id);
        //    db.App_Order.Remove(app_Order);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
