using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Web;

using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Persistence;
using FireWorkflow.Net.Engine.Taskinstance;
using FireWorkflow.Net.Kernel;
using WebDemo.Example.LoanProcess.Persistence;

namespace WebDemo.Example.LoanProcess.WorkflowExtension
{
    public class ApproveApplicationTaskCompletionEvaluator : ITaskInstanceCompletionEvaluator
    {
        ApproveInfoDAO approveInfoDAO = new ApproveInfoDAO();
        LoanInfoDAO loanInfoDAO = new LoanInfoDAO();

        public bool taskInstanceCanBeCompleted(IWorkflowSession currentSession, RuntimeContext runtimeContext, IProcessInstance processInstance, ITaskInstance taskInstance)
        {
            IPersistenceService persistenceService = runtimeContext.PersistenceService;
            IList<IWorkItem> workItems = persistenceService.FindWorkItemsForTaskInstance(taskInstance.Id);

            //从流程变量中获取业务流水号
            String sn = (String)ProcessInstanceHelper.getProcessInstanceVariable(processInstance,"sn");

            //已经完成的WorkItem数量
            int completedWorkItemCount = 0;

            //审批同意的决定的数量
            int approvedDecitionCount = 0;

            StringBuilder examinerList = new StringBuilder();//所有审核人名单
            StringBuilder approverList = new StringBuilder();//同意者名单
            StringBuilder opponentList = new StringBuilder();//不同意者名单
            for (int i = 0; i < workItems.Count; i++)
            {
                IWorkItem wi = workItems[i];

                if (wi.State == WorkItemEnum.COMPLETED)
                {
                    completedWorkItemCount++;
                    WebDemo.Example.LoanProcess.Persistence.ApproveInfo approveInfo = approveInfoDAO.findBySnAndUserId(sn, wi.ActorId);
                    if (approveInfo != null)
                    {
                        examinerList.Append(approveInfo.Approver).Append(",");
                        if (approveInfo.Decision)
                        {
                            approvedDecitionCount++;
                            approverList.Append(approveInfo.Approver).Append(",");
                        }
                        else
                        {
                            opponentList.Append(approveInfo.Approver).Append(",");
                        }
                    }
                }
            }


            //------------------判断是否可以结束该汇签任务-----------
            float size =(float)workItems.Count;
            float theRule = 2 / 3f;
            float currentCompletedPercentage = completedWorkItemCount / size;//已经完成的工单占总工单的比例
            float currentAggreePercentage = approvedDecitionCount / size;//已经同意的比例



            //如果完成的工单数量小于2/3，则直接返回false,即不可以结束TaskInstance
            if (currentCompletedPercentage < theRule)
            {
                return false;
            }
            //如果同意的数量达到2/3则直接结束TaskInstance
            else if (currentAggreePercentage >= theRule)
            {

                //修改流程变量的值
                ProcessInstanceHelper.setProcessInstanceVariable(processInstance,"Decision", true);

                //将最终审批决定纪录到业务表中
                LoanInfo loanInfo = loanInfoDAO.findBySn(sn);
                if (loanInfo!=null)
                {
                    loanInfo.Decision = true;
                    loanInfo.ExaminerList=examinerList.ToString();
                    loanInfo.ApproverList=approverList.ToString();
                    loanInfo.OpponentList=opponentList.ToString();
                    loanInfoDAO.attachDirty(loanInfo);
                }

                return true;
            }
            //当所有的workItem结束时，可以结束TaskInstance 
            else if (completedWorkItemCount == workItems.Count)
            {
                LoanInfo loanInfo = loanInfoDAO.findBySn(sn);

                if (currentAggreePercentage < theRule)
                {
                    //修改流程变量的值
                    ProcessInstanceHelper.setProcessInstanceVariable(processInstance,"Decision", false);

                    //将最终审批决定记录到业务表中
                    if (loanInfo != null) loanInfo.Decision = false;
                    loanInfo.ExaminerList=examinerList.ToString();
                    loanInfo.ApproverList=approverList.ToString();
                    loanInfo.OpponentList=opponentList.ToString();
                    loanInfoDAO.attachDirty(loanInfo);
                }
                else
                {
                    //修改流程变量的值
                    ProcessInstanceHelper.setProcessInstanceVariable(processInstance,"Decision", true);

                    //将最终审批决定记录到业务表中
                    if (loanInfo != null) loanInfo.Decision = true;
                    loanInfo.ExaminerList=examinerList.ToString();
                    loanInfo.ApproverList=approverList.ToString();
                    loanInfo.OpponentList=opponentList.ToString();
                    loanInfoDAO.attachDirty(loanInfo);
                }

                return true;
            }
            return false;
        }

    }
}
