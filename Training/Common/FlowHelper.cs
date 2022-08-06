using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAPI.Models;
using SAPI.DAL;

namespace SAPI.Common
{
    public class FlowHelper
    {
        /// <summary>
        /// 预定义审批流程集
        /// </summary>
        /// <returns></returns>
        private static List<Flow> GetFlows()
        {
            /*
            报销单/差旅报销单，请款单，冲账单：
            a1： 部门经理审批，预算负责人审批，总经理审批，财务经理审批
            a2： 部门经理审批，预算负责人审批，财务经理审批
            a3： 部门经理审批，预算负责人审批，财务经理审批，总经理审批

            暂支单
            b1： 部门经理审批，总经理审批，财务经理审批
            b2： 部门经理审批，财务经理审批
            b3： 部门经理审批，财务经理审批，总经理审批
            */
            List<Flow> flows = new List<Flow>();
            flows.Add(new Flow { FlowId = 1, FlowName = "请款单总经理先", NodeCount = 4, NodeQueue = new int[] { 1, 2, 4, 3 } });
            flows.Add(new Flow { FlowId = 2, FlowName = "请款单总经理无", NodeCount = 3, NodeQueue = new int[] { 1, 2, 3 } });
            flows.Add(new Flow { FlowId = 3, FlowName = "请款单总经理后", NodeCount = 4, NodeQueue = new int[] { 1, 2, 3, 4 } });
            flows.Add(new Flow { FlowId = 4, FlowName = "暂支单总经理先", NodeCount = 3, NodeQueue = new int[] { 1, 4, 3 } });
            flows.Add(new Flow { FlowId = 5, FlowName = "暂支单总经理无", NodeCount = 2, NodeQueue = new int[] { 1, 3 } });
            flows.Add(new Flow { FlowId = 6, FlowName = "暂支单总经理后", NodeCount = 3, NodeQueue = new int[] { 1, 3, 4 } });
            return flows;
        }

        /// <summary>
        /// 审批节点集
        /// </summary>
        private static List<FlowNode> GetFlowNodes()
        {
            List<FlowNode> nodes = new List<FlowNode>();
            nodes.Add(new FlowNode { NodeId = 1, NodeName = "上级主管", AuditCount = 1 });
            nodes.Add(new FlowNode { NodeId = 2, NodeName = "预算负责人" });
            nodes.Add(new FlowNode { NodeId = 3, NodeName = "财务经理", AuditCount = 1 });
            nodes.Add(new FlowNode { NodeId = 4, NodeName = "总经理", AuditCount = 1 });
            return nodes;
        }

        /// <summary>
        /// 获取用户列表By节点编号
        /// </summary>
        /// <returns></returns>
        private static List<UserBrief> GetUsersByNodeId(int nodeId, string deptLeader, string budgetOwner)
        {
            List<UserBrief> brief = new List<UserBrief>();
            switch (nodeId)
            {
                case 1:
                    brief = DAL.UserDal.GetUsersByString(deptLeader);
                    break;
                case 2:
                    brief = DAL.UserDal.GetUsersByString(budgetOwner);
                    break;
                case 3:
                    brief.Add(new UserBrief() { User_Id = 93, User_Name = "彭明婷" });
                    break;
                case 4:
                    brief.Add(new UserBrief() { User_Id = 6, User_Name = "马爽" });
                    break;
                default:
                    break;
            }
            return brief;
        }

        /// <summary>
        /// 获取指定流程的节点编号
        /// </summary>
        /// <param name="flowId"></param>
        /// <returns></returns>
        public static int[] GetNodes(int flowId)
        {
            return GetFlows().Where(w => w.FlowId.Equals(flowId)).FirstOrDefault().NodeQueue;
        }

        ///// <summary>
        ///// 生成所有审核节点
        ///// </summary>
        ///// <param name="requestId"></param>
        ///// <param name="flowId"></param>
        ///// <param name="deptLeader"></param>
        ///// <param name="budgetOwner"></param>
        ///// <returns></returns>
        //public static List<FlowAudit> GetAllAuditNodes(int requestId, int flowId, string deptLeader, string budgetOwner)
        //{
        //    List<FlowAudit> aNodes = new List<FlowAudit>();
        //    List<UserBrief> briefs;
        //    int[] nodes = GetNodes(flowId);
        //    int stepCount = nodes.Length;
        //    int isCurrent = 1;
        //    int isFinal = 0;
        //    int nodeId, nextId;
        //    FlowAudit flow;
        //    for (int i = 1; i <= stepCount; i++)
        //    {
        //        nodeId = nodes[i - 1];
        //        nextId = i + 1;
        //        briefs = GetUsersByNodeId(nodeId, deptLeader, budgetOwner);
        //        //同一个节点，可有多人审批
        //        foreach (UserBrief ub in briefs)
        //        {
        //            flow = new FlowAudit() { RequestID = requestId, FlowID = flowId, StepID = i, IsAudit = 0, CreateTime = DateTime.Now };
        //            flow.UserId = ub.User_Id;
        //            flow.UserName = ub.User_Name;
        //        //    flow.AuditCount = briefs.Count;
        //            flow.IsCurrent = isCurrent;
        //            //标记最后一个节点
        //            if (i == stepCount)
        //            {
        //                isFinal = 1;
        //                nextId = 0;
        //            }
        //            flow.NextId = nextId;
        //        //    flow.IsFinal = isFinal;
        //            aNodes.Add(flow);
        //        }
        //        isCurrent = 0;
        //    }
        //    return aNodes;
        //}
    }
}