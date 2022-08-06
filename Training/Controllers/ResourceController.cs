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
    public class ResourceController : BaseController
    {
        /// <summary>
        /// 活动列表(资源)
        /// </summary>
        /// <returns></returns>
        // GET: Resource
        public ActionResult Index()
        {
            ViewBag.Title = "营销活动";
            return View(db.App_Resource.ToList());
        }
        /// <summary>
        /// 活动详情
        /// </summary>
        /// <param name="id">资源ID</param>
        /// <returns></returns>
        // GET: Resource/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Content(ScriptHelper.ShowAlert(DataWasNoDefined));
            }
            App_Resource app_Resource = db.App_Resource.Find(id);
            if (app_Resource == null)
            {
                ViewBag.Title = DataWasDisabledOrDoesNotExist;
                return View();
            }
            return View(app_Resource);
        }

        /// <summary>
        /// 销售分配列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Resource/Allot/5
        public ActionResult Allot(int? id)
        {
            if (id == null)
            {
                return Content(ScriptHelper.ShowAlert(DataWasNoDefined));
            }
            ViewBag.Title = "销售分配";
            ViewBag.ImgPrix = SalesImgUrl;
            ViewData["id"] = id;

            //显示活动名称
            var res = db.App_Resource.Find(id);
            if (res != null)
            {
                ViewBag.RName = res.Title;
            }
            //显示已分配情况
            var sAllots = db.App_SalesAllot.Where(w => w.Resource_Id.Equals(id.Value)).OrderBy(o => o.Sort_Id).ThenBy(t => t.Sales_Id);
            if (sAllots.Any())
            {
                return View(sAllots.ToList());
            }
            else
            {
                ViewBag.SubTitle = DataListDoesNotExistAnyItems;
                return View();
            }
        }
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="rid">rowId</param>
        /// <param name="sid">ResourceId</param>
        /// <returns></returns>
        public ActionResult MoveUp(int? rid, int? sid)
        {
            if ((rid == null) || (sid == null))
            {
                return Content(ScriptHelper.ShowAlert(DataWasNoDefined));
            }
            DAL.OrderDal.MoveSalesAllot(rid.Value, 0);
            return RedirectToAction("Allot", new { id = sid.Value });
        }

        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="rid">rowId</param>
        /// <param name="sid">ResourceId</param>
        /// <returns></returns>
        public ActionResult MoveDown(int? rid, int? sid)
        {
            if ((rid == null) || (sid == null))
            {
                return Content(ScriptHelper.ShowAlert(DataWasNoDefined));
            }
            DAL.OrderDal.MoveSalesAllot(rid.Value, 1);
            return RedirectToAction("Allot", new { id = sid.Value });
        }

        /// <summary>
        /// 销售分配订单列表
        /// </summary>
        /// <param name="rid">ResourceID</param>
        /// <param name="sid">SalesID</param>
        /// <returns></returns>
        // GET: Resource/SOrders/5
        public ActionResult SOrders(int? rid, int? sid)
        {
            if ((rid == null) || (sid == null))
            {
                return Content(ScriptHelper.ShowAlert(DataWasNoDefined));
            }
            //显示销售名称
            var sa = db.App_Sales.Where(w => w.Sales_Id.Equals(sid.Value));
            if (sa.Any())
            {
                ViewBag.SName = sa.First().Name;
            }
            else
            {
                Common.ScriptHelper.ShowAlert("销售不存在");
            }
            //显示活动名称
            var res = db.App_Resource.Where(w => w.RowId.Equals(rid.Value));
            if (sa.Any())
            {
                ViewBag.RName = res.First().Title;
            }
            else
            {
                Common.ScriptHelper.ShowAlert("活动不存在");
            }

            ViewBag.Title = "销售的订单列表";
            ViewBag.Resource_Id = rid;

            //显示已分配情况,最近100行
            var sAllots = db.App_OrderAllot.Where(w => w.ResourceId.Equals(rid.Value) && w.Sales_Id.Equals(sid.Value)).OrderByDescending(o => o.Create_Time).Take(100);
            if (sAllots.Any())
            {
                return View(sAllots.ToList());
            }
            else
            {
                ViewBag.SubTitle = DataListDoesNotExistAnyItems;
                return View();
            }
        }

        // GET: Resource/ACreate/5
        /// <summary>
        /// 新增销售分配
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ACreate(int? id)
        {
            if (id == null)
            {
                return Content(ScriptHelper.ShowAlert(DataWasNoDefined));
            }
            ViewBag.ImgPrix = SalesImgUrl;
            ViewData["id"] = id;
            //显示活动名称
            var res = db.App_Resource.Find(id);
            if (res != null)
            {
                ViewBag.RName = res.Title;
            }
            int saleType = 1;
            //社群销售列表
            ViewData["salesList"] = db.App_Sales.Where(w => w.Is_Enable.Equals(true) && w.Sales_Type.Equals(saleType)).OrderBy(o => o.Sales_Id).Select(row => new SelectListItem() { Text = row.Name, Value = row.Sales_Id.ToString() }).ToList();
            App_SalesAllot sa = new App_SalesAllot() { Resource_Id = id.Value, Sort_Id = 0, Post_Count = 0, Term_Count = 0, Change_Count = 5, Is_Enable = true };
            return View(sa);
        }

        // POST: Resource/ACreate
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 新增销售分配提交
        /// </summary>
        /// <param name="sa"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ACreate([Bind(Include = "Resource_Id,Sales_Id,Name,Private_Code,QrCode,Is_Enable,Sort_Id,Change_Count,Term_Count,Post_Count,Remark,Expire_Time")] App_SalesAllot sa)
        {
            sa.Create_Time = DateTime.Now;
            int opResult = 0;
            if (ModelState.IsValid)
            {
                try
                {
                    db.App_SalesAllot.Add(sa);
                    opResult = db.SaveChanges();
                }
                catch (Exception)
                {
                    string msg = string.Format("该销售(Id:{0},名称:{1})已分配，请勿重复操作！", sa.Sales_Id, sa.Name);
                    return Content(ScriptHelper.ShowAlert(msg));
                }
            }

            if (opResult > 0)
            {
                return RedirectToAction("Allot", new { id = sa.Resource_Id });
            }
            else
            {
                return Content(ScriptHelper.ShowAlert("分配错误，请检查必填项"));
            }
        }

        // GET: Resource/ADelete/5
        /// <summary>
        /// 删除销售分配
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ADelete(int? id)
        {
            if (id == null)
            {
                return Content(ScriptHelper.ShowAlert(DataWasNoDefined));
            }
            App_SalesAllot app_Resource = db.App_SalesAllot.Find(id);
            if (app_Resource == null)
            {
                return HttpNotFound();
            }
            return View(app_Resource);
        }

        // POST: Resource/ADelete/5
        /// <summary>
        /// 删除销售分配提交
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("ADelete")]
        [ValidateAntiForgeryToken]
        public ActionResult ADeleteConfirmed(int id)
        {
            App_SalesAllot sa = db.App_SalesAllot.Find(id);
            int rid = sa.Resource_Id;
            db.App_SalesAllot.Remove(sa);
            db.SaveChanges();
            return RedirectToAction("Allot", new { id = rid });
        }

        // GET: Resource/AEdit/5
        /// <summary>
        /// 修改销售分配
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AEdit(long? id)
        {
            if (id == null)
            {
                return Content(ScriptHelper.ShowAlert(DataWasNoDefined));
            }
            App_SalesAllot sa = db.App_SalesAllot.Find(id);
            sa.QrCode = SalesImgUrl + sa.QrCode;
            if (sa == null)
            {
                return Content(ScriptHelper.ShowAlert(DataWasDisabledOrDoesNotExist));
            }
            return View(sa);
        }

        // POST: Resource/AEdit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 修改销售分配提交
        /// </summary>
        /// <param name="sa"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AEdit([Bind(Include = "RowId,Resource_Id,Name,Private_Code,QrCode,Is_Enable,Sort_Id,Change_Count,Term_Count,Post_Count,Remark,Expire_Time")] App_SalesAllot sa)
        {
            if (ModelState.IsValid)
            {
                int rid = sa.Resource_Id;
                //全字段更新
                db.Entry(sa).State = EntityState.Modified;
                //指定不更新创建日期
                DbEntityEntry<App_SalesAllot> entry = db.Entry(sa);
                entry.Property(t => t.Resource_Id).IsModified = false;
                entry.Property(t => t.Sales_Id).IsModified = false;
                entry.Property(t => t.Create_Time).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Allot", new { id = rid });
            }
            return View(sa);
        }
        //// GET: Resource/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    App_Resource app_Resource = db.App_Resource.Find(id);
        //    if (app_Resource == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(app_Resource);
        //}

        //// POST: Resource/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "RowId,Title,Summary,Content,Img_Url,CImg_Url,Sale_Status,Price,Line_Price,View_Count,Total_Chapters,Author,SubSet_Count,Audio_Length,Describe,Resource_Type,Content_Url,Creator,Create_Time,Update_Time,Remark,Status")] App_Resource app_Resource)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(app_Resource).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(app_Resource);
        //}

        //// GET: Resource/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    App_Resource app_Resource = db.App_Resource.Find(id);
        //    if (app_Resource == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(app_Resource);
        //}

        //// POST: Resource/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    App_Resource app_Resource = db.App_Resource.Find(id);
        //    db.App_Resource.Remove(app_Resource);
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
