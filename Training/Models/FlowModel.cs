using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAPI.Models
{
    /// <summary>
    /// 审批流程
    /// </summary>
    public class Flow
    {
        /// <summary>
        /// 审批流程编号
        /// </summary>
        public int FlowId { get; set; }
        /// <summary>
        /// 审批流程名称
        /// </summary>
        public string FlowName { get; set; }
        /// <summary>
        /// 节点数
        /// </summary>
        public int NodeCount { get; set; }
        /// <summary>
        /// 节点顺序
        /// </summary>
        public int[] NodeQueue { get; set; }
    }

    /// <summary>
    /// 审批节点
    /// </summary>
    public class FlowNode
    {
        /// <summary>
        /// 节点编号
        /// </summary>
        public int NodeId { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// 审批人数
        /// </summary>
        public int AuditCount { get; set; }
        /// <summary>
        /// 审批人Id
        /// </summary>
        public int[] UserIds { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int NodeResult { get; set; }
        /// <summary>
        /// 后续节点编号
        /// </summary>
        public int NextNodeId { get; set; }
        /// <summary>
        /// 是否终点
        /// </summary>
        public bool IsEnd { get; set; }
    }
}