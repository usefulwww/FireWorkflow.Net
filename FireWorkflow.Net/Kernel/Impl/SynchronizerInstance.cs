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
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Condition;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Event;
using FireWorkflow.Net.Kernel.Plugin;
using FireWorkflow.Net.Model.Net;

namespace FireWorkflow.Net.Kernel.Impl
{
	public class SynchronizerInstance : AbstractNodeInstance, ISynchronizerInstance
	{
		public const String Extension_Target_Name = "FireWorkflow.Net.Kernel.SynchronizerInstance";
		public static List<String> Extension_Point_Names = new List<String>();
		public const String Extension_Point_NodeInstanceEventListener = "NodeInstanceEventListener";

		/// <summary>
		/// volume是同步器的容量
		/// </summary>
		public int Volume { get; set; }

		static SynchronizerInstance()
		{
			Extension_Point_Names.Add(Extension_Point_NodeInstanceEventListener);
		}
		private Synchronizer synchronizer = null;


		public override String Id
		{
			get { return this.synchronizer.Id; }
		}

		public SynchronizerInstance(Synchronizer s)
		{
			synchronizer = s;
			int a = synchronizer.EnteringTransitions.Count;
			int b = synchronizer.LeavingTransitions.Count;
			this.Volume = a * b;

			//		System.out.println("synchronizer "+synchronizer.Name+"'s volume is "+volume);
		}


		public IJoinPoint synchronized(IToken tk)
		{
			IJoinPoint joinPoint = null;
			tk.NodeId=this.Synchronizer.Id;
			//log.debug("The weight of the Entering TransitionInstance is " + tk.getValue());
			// 触发TokenEntered事件
			NodeInstanceEvent event1 = new NodeInstanceEvent(this);
			event1.Token=tk;
			event1.EventType=NodeInstanceEventEnum.NODEINSTANCE_TOKEN_ENTERED;//token 进入
			this.fireNodeEvent(event1);

			//汇聚检查
			joinPoint = ProcessInstanceHelper.createJoinPoint(tk.ProcessInstance,this, tk);// JoinPoint由谁生成比较好？
			int value = (int)joinPoint.Value;
			if (value > this.Volume)//如果value大于同步器容量，那说明出错了
			{
				KernelException exception = new KernelException(tk.ProcessInstance,
				                                                this.Synchronizer,
				                                                "Error:The token count of the synchronizer-instance can NOT be  greater than  it's volumn  ");
				throw exception;
			}
			if (value < this.Volume)
			{// 如果Value小于容量则继续等待其他弧的汇聚。 (哪些状态为dead的token到此结束，不再向下传递)
				return null;
			}
			return joinPoint;
		}

		public override void fire(IToken tk)
		{
			//TODO 此处性能需要改善一下,20090312
			IJoinPoint joinPoint = synchronized(tk);
			if (joinPoint == null) return;

			//如果汇聚点的容量和同步器节点的容量相同
			IProcessInstance processInstance = tk.ProcessInstance;
			// Synchronize的fire条件应该只与joinPoint的value有关（value==volume），与alive无关
			NodeInstanceEvent event2 = new NodeInstanceEvent(this);
			event2.Token=tk;
			event2.EventType=NodeInstanceEventEnum.NODEINSTANCE_FIRED;
			this.fireNodeEvent(event2);

			//在此事件监听器中，删除原有的token
			NodeInstanceEvent event4 = new NodeInstanceEvent(this);
			event4.Token=tk;
			event4.EventType=NodeInstanceEventEnum.NODEINSTANCE_LEAVING;
			
			this.fireNodeEvent(event4);

			//首先必须检查是否有满足条件的循环，loop比transition有更高的优先级，
			//（只能够有一个loop的条件为true，流程定义的时候需要注意）
			Boolean doLoop = false;//表示是否有满足条件的循环，false表示没有，true表示有。
			if (joinPoint.Alive)
			{
				IToken tokenForLoop = null;

				tokenForLoop = new Token(); // 产生新的token
				tokenForLoop.IsAlive=joinPoint.Alive;
				tokenForLoop.ProcessInstance=processInstance;
				tokenForLoop.StepNumber=joinPoint.StepNumber - 1;
				tokenForLoop.FromActivityId=joinPoint.FromActivityId;

				for (int i = 0; i < this.LeavingLoopInstances.Count; i++)
				{
					ILoopInstance loopInstance = this.LeavingLoopInstances[i];
					doLoop = loopInstance.take(tokenForLoop);
					if (doLoop)
					{
						break;
					}
				}
			}
			if (!doLoop)
			{//如果没有循环，则执行transitionInstance
				//非顺序流转的需要生成新的token，
				Boolean activiateDefaultCondition = true;
				ITransitionInstance defaultTransInst = null;
				for (int i = 0; LeavingTransitionInstances != null && i < LeavingTransitionInstances.Count; i++)
				{
					ITransitionInstance transInst = LeavingTransitionInstances[i];
					String condition = transInst.Transition.Condition;
					if (condition != null && condition.Equals(ConditionConstant.DEFAULT))
					{
						defaultTransInst = transInst;
						continue;
					}

					Token token = new Token(); // 产生新的token
					token.IsAlive=joinPoint.Alive;
					token.ProcessInstance=processInstance;
					token.StepNumber=joinPoint.StepNumber;
					token.FromActivityId=joinPoint.FromActivityId;
					Boolean alive = transInst.take(token);
					if (alive)
					{
						activiateDefaultCondition = false;
					}

				}
				if (defaultTransInst != null)
				{
					Token token = new Token();
					token.IsAlive=activiateDefaultCondition && joinPoint.Alive;
					token.ProcessInstance=processInstance;
					token.StepNumber=joinPoint.StepNumber;
					token.FromActivityId=joinPoint.FromActivityId;
					defaultTransInst.take(token);
				}

			}

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
			if (!Extension_Target_Name.Equals(extension.ExtentionTargetName))
			{
				throw new Exception(
					"Error:When construct the SynchronizerInstance,the Extension_Target_Name is mismatching");
			}
			if (Extension_Point_NodeInstanceEventListener.Equals(extension.ExtentionPointName))
			{
				if (extension is INodeInstanceEventListener)
				{
					this.EventListeners.Add((INodeInstanceEventListener)extension);
				}
				else
				{
					throw new Exception(
						"Error:When construct the SynchronizerInstance,the extension MUST be a instance of INodeInstanceEventListener");
				}
			}
		}

		public override String ToString()
		{
			return "SynchronizerInstance_4_[" + synchronizer.Id + "]";
		}

		public Synchronizer Synchronizer { get { return this.synchronizer; } }
		
	}

}
