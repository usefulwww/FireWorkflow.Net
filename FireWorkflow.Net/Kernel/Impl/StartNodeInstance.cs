/*
 * Copyright 2003-2008 非也
 * All rights reserved.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation。
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses. *
 * @author 非也,nychen2000@163.com
 * @Revision to .NET 无忧 lwz0721@gmail.com 2010-02
 */
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Condition;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Event;
using FireWorkflow.Net.Kernel.Plugin;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Net;

namespace FireWorkflow.Net.Kernel.Impl
{
	public class StartNodeInstance : AbstractNodeInstance, ISynchronizerInstance
	{
		public const String Extension_Target_Name = "FireWorkflow.Net.Kernel.StartNodeInstance";
		public static List<String> Extension_Point_Names = new List<String>();
		public const String Extension_Point_NodeInstanceEventListener = "NodeInstanceEventListener";

		/// <summary>
		/// volume是同步器的容量  即节点的容量
		/// </summary>
		public int Volume { get; set; }

		static StartNodeInstance()
		{
			Extension_Point_Names.Add(Extension_Point_NodeInstanceEventListener);
		}
		// private int tokenValue = 0;
		private StartNode startNode = null;

		// private Boolean alive = false;
		public StartNodeInstance(StartNode startNd)
		{
			this.startNode = startNd;
			this.Volume = startNode.LeavingTransitions.Count;//  start 节点容量 ==输出弧的数量
		}

		public override String Id
		{
			get { return this.startNode.Id; }
		}

		/// <summary>
		/// 开始节点触发
		/// </summary>
		/// <param name="tk"></param>
		public override void fire(IToken tk)
		{
			if (!tk.IsAlive)//如果token是false，那么直接返回
			{
				return;//
			}
			if (tk.Value != this.Volume)
			{
				KernelException exception = new KernelException(tk.ProcessInstance,
				                                                this.startNode,
				                                                "Error:Illegal StartNodeInstance,the tokenValue MUST be equal to the volume ");
				throw exception;

			}

			tk.NodeId = this.Synchronizer.Id;//到开始节点（同步器）

			IProcessInstance processInstance = tk.ProcessInstance;//从token中获得流程实例对象

			//触发token_entered事件
			NodeInstanceEvent event1 = new NodeInstanceEvent(this);
			event1.Token=tk;
			event1.EventType=NodeInstanceEventEnum.NODEINSTANCE_TOKEN_ENTERED;//token进入
			this.fireNodeEvent(event1);

			//触发fired事件
			NodeInstanceEvent event2 = new NodeInstanceEvent(this);
			event2.Token=tk;
			event2.EventType=NodeInstanceEventEnum.NODEINSTANCE_FIRED;//token 触发
			this.fireNodeEvent(event2);

			//触发leaving事件
			NodeInstanceEvent event4 = new NodeInstanceEvent(this);
			event4.Token=tk;
			event4.EventType=NodeInstanceEventEnum.NODEINSTANCE_LEAVING;//token 离开
			this.fireNodeEvent(event4);


			Boolean activiateDefaultCondition = true;//激活默认弧线的标志
			ITransitionInstance defaultTransInst = null;
			//找到所有开始节点的输出弧
			for (int i = 0; LeavingTransitionInstances != null && i < LeavingTransitionInstances.Count; i++)
			{
				ITransitionInstance transInst = LeavingTransitionInstances[i];//开始节点的边的类型只能是transition
				String condition = transInst.Transition.Condition;
				//如果弧线的条件！=null 并且 =“default” ，那么弧线实例就是default的弧线了。
				if (condition != null && condition.Equals(ConditionConstant.DEFAULT))
				{
					defaultTransInst = transInst;//记录default转移线，其他条件都未false，才执行它
					continue;
				}

				Token token = new Token(); // 产生新的token
				token.IsAlive = true;
				token.ProcessInstance = processInstance;
				token.FromActivityId = tk.FromActivityId;
				token.StepNumber = tk.StepNumber + 1;//步骤号+1

				Boolean alive = transInst.take(token);//触发弧线的token
				if (alive)
				{
					activiateDefaultCondition = false;
				}

			}
			if (defaultTransInst != null)//如果defaultTransInst!=null ，走的是default值的弧线
			{
				Token token = new Token();
				token.IsAlive = activiateDefaultCondition;//设置token为dead
				token.ProcessInstance = processInstance;
				token.FromActivityId = token.FromActivityId;
				token.StepNumber = tk.StepNumber + 1;
				defaultTransInst.take(token);
			}


			//触发completed事件
			NodeInstanceEvent event3 = new NodeInstanceEvent(this);
			event3.Token=tk;
			event3.EventType=NodeInstanceEventEnum.NODEINSTANCE_COMPLETED;
			this.fireNodeEvent(event3);
		}

		public override String ExtensionTargetName { get { return Extension_Target_Name; } }

		public override List<String> ExtensionPointNames { get { return Extension_Point_Names; } }


		// TODO extesion是单态还是多实例？单态应该效率高一些。
		public override void registExtension(IKernelExtension extension)
		{
			// System.out.println("====extension class is
			// "+extension.getClass().Name);
			if (!Extension_Target_Name.Equals(extension.ExtentionTargetName))
			{
				throw new Exception("Error:When construct the StartNodeInstance,the Extension_Target_Name is mismatching");
			}
			if (Extension_Point_NodeInstanceEventListener.Equals(extension.ExtentionPointName))
			{
				if (extension is INodeInstanceEventListener)
				{
					this.EventListeners.Add((INodeInstanceEventListener)extension);
				}
				else
				{
					throw new Exception("Error:When construct the StartNodeInstance,the extension MUST be a instance of INodeInstanceEventListener");
				}
			}
		}

		public override String ToString()
		{
			return "StartNodeInstance_4_[" + startNode.Id + "]";
		}

		public Synchronizer Synchronizer { get { return this.startNode; } }

	}
}
