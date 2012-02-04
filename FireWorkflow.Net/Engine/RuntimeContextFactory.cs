using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using FireWorkflow.Net.Engine.Beanfactory;
using FireWorkflow.Net.Engine.Calendar;
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Engine.Taskinstance;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Engine.Condition;
using System.Configuration;

namespace FireWorkflow.Net.Engine
{

    /// <summary>
    /// 在没有spring环境下构建RuntimeContext实例。<br/>
    /// (暂未实现)
    /// </summary>
    public class RuntimeContextFactory
    {
        private static RuntimeContext ctx;

        /// <summary>
        /// Gets the runtime context.
        /// </summary>
        /// <returns></returns>
        public static RuntimeContext getRuntimeContext()
        {
            if (ctx == null)
            {
                ctx = new RuntimeContext();
                ctx.IsEnableTrace = true;
                //转移条件表达式解析服务
                ctx.ConditionResolver = new FireWorkflow.Net.Engine.Condition.ConditionResolver();

                //实例对象存取服务
                Type type = null;
                string dbtype = ConfigurationManager.AppSettings["dbtype"];
                switch (dbtype)
                {
                    case "sqlserver":
                        type = Type.GetType("FireWorkflow.Net.Persistence.SqlServerDAL.PersistenceServiceDAL, FireWorkflow.Net.Persistence.SqlServerDAL");
                        break;
                    case "oracle":
                        type = Type.GetType("FireWorkflow.Net.Persistence.OracleDAL.PersistenceServiceDAL, FireWorkflow.Net.Persistence.OracleDAL");
                        break;
                    default:
                        type = Type.GetType(dbtype);
                        break;
                }
                
                if (type != null)
                {
                    
                    switch (dbtype)
                    {
                        case "sqlserver":
                            ctx.PersistenceService = (IPersistenceService)Activator.CreateInstance(type, new object[] { "SqlServer" });
                            break;
                        case "oracle":
                            ctx.PersistenceService = (IPersistenceService)Activator.CreateInstance(type, new object[] { "OracleServer" });
                            break;
                        default:
                            ctx.PersistenceService = (IPersistenceService)Activator.CreateInstance(type, new object[] {});
                            break;
                    }
                }
                else throw new Exception("默认"+dbtype+"程序集没有引入！");

                //流程定义服务，通过该服务获取流程定义
                DefinitionService4DBMS ds4dbms = new FireWorkflow.Net.Engine.Definition.DefinitionService4DBMS();
                //ds4fs.DefinitionFiles.Add("/org/fireflow/example/workflowdefinition/demo_workflow.xml");
                ctx.DefinitionService = ds4dbms;

                //内核管理器
                ctx.KernelManager = new KernelManager();

                //TaskInstance 管理器，负责TaskInstance的创建、运行、结束。
                BasicTaskInstanceManager btim = new FireWorkflow.Net.Engine.Taskinstance.BasicTaskInstanceManager();
                btim.DefaultTaskInstanceCreator = new FireWorkflow.Net.Engine.Taskinstance.DefaultTaskInstanceCreator();
                btim.DefaultFormTaskInstanceRunner = new FireWorkflow.Net.Engine.Taskinstance.DefaultFormTaskInstanceRunner();
                btim.DefaultToolTaskInstanceRunner = new FireWorkflow.Net.Engine.Taskinstance.DefaultToolTaskInstanceRunner();
                btim.DefaultSubflowTaskInstanceRunner = new FireWorkflow.Net.Engine.Taskinstance.DefaultSubflowTaskInstanceRunner();
                btim.DefaultFormTaskInstanceCompletionEvaluator = new FireWorkflow.Net.Engine.Taskinstance.DefaultFormTaskInstanceCompletionEvaluator();
                btim.DefaultToolTaskInstanceCompletionEvaluator = new FireWorkflow.Net.Engine.Taskinstance.DefaultToolTaskInstanceCompletionEvaluator();
                btim.DefaultSubflowTaskInstanceCompletionEvaluator = new FireWorkflow.Net.Engine.Taskinstance.DefaultSubflowTaskInstanceCompletionEvaluator();
                btim.DefaultTaskInstanceEventListener = new FireWorkflow.Net.Engine.Taskinstance.DefaultTaskInstanceEventListener();

                ctx.TaskInstanceManager = btim;

                //日历服务
                ctx.CalendarService = new FireWorkflow.Net.Engine.Calendar.DefaultCalendarService();

                //bean工厂，fire workflow默认使用spring作为其实现  lwz 2010-3-21 edit 改为 C# type加载
                ctx.BeanFactory = new FireWorkflow.Net.Engine.Beanfactory.BeanFactory();
            }
            return ctx;
        }
    }
}
