using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using FireWorkflow.Net.Engine;

namespace WebDemo.Example.WorkflowExtension
{
    /// <summary>
    /// 为了便于页面展示，所有的业务TaskInstance均须实现该接口。
    /// </summary>
    public interface IExampleTaskInstance
    {
        /// <summary>
        /// 获得当前的业务信息，将扩展taskInstance中的业务字段组成一个string返回，
        /// 便于在待办工单和已办工单界面进行统一的显示
        /// </summary>
        String BizInfo { get; }
        /// <summary>
        /// 获得或设置当前TaskInstance的所有workItem列表
        /// </summary>
        /// <returns></returns>
        List<IWorkItem> WorkItems { get; }
    }
}
