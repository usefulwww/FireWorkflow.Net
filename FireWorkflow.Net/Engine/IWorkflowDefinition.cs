using System;

namespace FireWorkflow.Net.Engine
{
	/// <summary>
	/// Description of IWorkflowDefinition.
	/// </summary>
	public interface IWorkflowDefinition
	{
		#region 属性
        /// <summary>获取或设置主键</summary>
         String Id { get; set; }
        /// <summary>获取或设置流程id</summary>
         String ProcessId { get; set; }
        /// <summary>获取或设置流程英文名称</summary>
         String Name { get; set; }
        /// <summary>获取或设置流程显示名称</summary>
         String DisplayName { get; set; }
        /// <summary>获取或设置流程业务说明</summary>
         String Description { get; set; }
        /// <summary>获取或设置版本号</summary>
         Int32 Version { get; set; }
        /// <summary>获取或设置是否发布，1=已经发布,0未发布</summary>
         Boolean State { get; set; }
        /// <summary>获取或设置上载到数据库的操作员</summary>
         String UploadUser { get; set; }
        /// <summary>获取或设置上载到数据库的时间</summary>
         DateTime UploadTime { get; set; }
        /// <summary>获取或设置发布人</summary>
         String PublishUser { get; set; }
        /// <summary>获取或设置发布时间</summary>
         DateTime PublishTime { get; set; }
        /// <summary>获取或设置定义文件的语言类型，fpdl,xpdl,bepl...</summary>
         String DefinitionType { get; set; }//
        #endregion
		 /// <summary>获取或设置流程定义文件的内容。</summary>
         String ProcessContent { get; set; }//
	}
}
