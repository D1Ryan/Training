using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAPI.Models;
using SAPI.Common;

namespace SAPI.Controllers
{
    public class FlowAuditController : BaseController
    {
        /// <summary>
        /// 首次提交审核
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="flowId"></param>
        /// <param name="deptLeader"></param>
        /// <param name="budgetOwner"></param>
        /// <returns></returns>
        public ActionResult SubmitAudit(int requestId, int flowId, string deptLeader, string budgetOwner)
        {
            ////1：有的单据是修改后重新提交审核，需要删除以前的审核记录
            //var audits = db.FlowAudit.Where(w => w.RequestID.Equals(requestId));
            //if (audits.Count() > 0)
            //{
            //    db.FlowAudit.RemoveRange(audits);
            //    db.SaveChanges();
            //}
            ////2：根据当前流程，生成每个审批节点
            //List<FlowAudit> flow = FlowHelper.GetAllAuditNodes(requestId, flowId, deptLeader, budgetOwner);
            int saveCount = 0;
            //try
            //{
            //    db.FlowAudit.AddRange(flow);
            //    saveCount = db.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    string errmsg = string.Format("提交审核失败，requestId:[{0}],flowId:[{1}],deptLeader:[{2}],budgetOwner:[{3}]", requestId, flowId, deptLeader, budgetOwner);
            //    iLogger.Error(errmsg, ex);
            //}
            return Content(saveCount.ToString());
        }

        /// <summary>
        /// 财务增加总经理审批
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="flowId"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult AddLastAuditNode(int requestId, int flowId, int userId, string userName)
        {
            //1：判断是否为财务经理，如果不是，直接返回
            //2：先变更已存在的审核节点的流程编号，
            //var audits = db.FlowAudits.Where(w => w.RequestID.Equals(requestId));
            //if (audits.Count() > 0)
            //{
            //    db.FlowAudits.RemoveRange(audits);
            //    db.SaveChanges();
            //}
            //FlowAudit flow = new FlowAudit();
            //3：增加新的审批节点
            //flow.Add(new FlowAudit() { RequestID = requestId, FlowID = flowId, NodeID = 1, IsAudit = 0, UserId = userId, UserName = userName, CreateTime = DateTime.Now });
            
            int saveCount = 0;
            try
            {
                //db.FlowAudit.Add(flow);
                saveCount = db.SaveChanges();
            }
            catch (Exception ex)
            {
                string errmsg = string.Format("提交BOSS审核失败，requestId:[{0}],flowId:[{1}],userId:[{2}],userName:[{3}]", requestId, flowId, userId, userName);
                iLogger.Error(errmsg, ex);
            }
            return Content(saveCount.ToString());
        }




        /// <summary>
        /// 设置审核
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="flowId"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult SetAudit(int requestId, int flowId, int userId, string userName, int isPass, string auditMsg)
        {

            //如果当前审批人拒绝，则先更新申请单的状态为拒绝状态。整个审批流为拒绝状态，不再往下审批
            //如果当前审批通过时，判断本节点如果为多人审批时，检查是否所有人均已审批通过，如果全部审批通过，才进入到下一个审批节点。
            //FlowAudit flow = new FlowAudit() { RequestID = requestId, FlowID = flowId, StepID = 1, IsAudit = 0, UserId = userId, UserName = userName };
            //flow.CreateTime = DateTime.Now;
            int saveCount = 0;
            try
            {
                //db.FlowAudit.Add(flow);
                saveCount = db.SaveChanges();
            }
            catch (Exception ex)
            {
                string errmsg = string.Format("提交审核失败，requestId:[{0}],flowId:[{1}],userId:[{2}],userName:[{3}]", requestId, flowId, userId, userName);
                iLogger.Error(errmsg, ex);
            }
            return Content(saveCount.ToString());
        }


        //关于财务审批流
        //修改原单据申请页面的新增，修改，及列表显示审批状态
        //单据查看时，增加审批信息(FollowAudit)
        //修改预算归属页面，将预算负责人对应到原表CreateUser字段
        /// <summary>
        /// 生成待审核步骤
        /// </summary>
        public void CreateStep()
        {
            //为当前审批流生成待审核步骤
            //写入流程审批日志表FollowAudits
        }

        /// <summary>
        /// 流程审核
        /// </summary>
        public void FollowAudit()
        {
            //审批当前步骤，更新流程审核日志表FollowAudits
            //判断1：审核是否为拒统，如果拒绝，流程审批结束，如果通过，继续判断2
            //判断2：是否为多人审批，如果是。转入判断3，否则转入判断4。
            //判断3：是否全部审批完成，如果是，转入判断4，否则结束
            //判断4：是否为最后一个审核，如果是，结束审批动作，更新（Request）流程审批状态，否则调用CreateStep。
        }
    }
}