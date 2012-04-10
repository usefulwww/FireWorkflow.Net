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
using System.Text;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Event;
using FireWorkflow.Net.Kernel.Plugin;
using FireWorkflow.Net.Model.Net;

namespace FireWorkflow.Net.Kernel.Impl
{
	public class TransitionInstance : EdgeInstance, ITransitionInstance, IPlugable
	{
		public const String Extension_Target_Name = "FireWorkflow.Net.Kernel.TransitionInstance";
		public const String Extension_Point_TransitionInstanceEventListener = "TransitionInstanceEventListener";
		public static List<String> Extension_Point_Names = new List<String>();

		static TransitionInstance()
		{
			Extension_Point_Names.Add(Extension_Point_TransitionInstanceEventListener);
		}

		private Transition transition = null;

		public TransitionInstance(Transition t)
		{
			transition = t;
		}

		public override String Id { get { return this.transition.Id; } }

		//	private int weight = 0;
		public override int Weight
		{
			get
			{
				if (weight == 0)
				{
					if (EnteringNodeInstance is StartNodeInstance)
					{
						weight = 1;
						return weight;
						//如果前驱结点是开始节点，那么权值规定为1
					}
					else if (LeavingNodeInstance is EndNodeInstance)
					{
						weight = 1;
						return weight;
						//如果后继结点为结束节点，那么权值规定为1
					}
					else if (LeavingNodeInstance is ActivityInstance)
					{
						SynchronizerInstance synchronizerInstance = (SynchronizerInstance)EnteringNodeInstance;
						weight = synchronizerInstance.Volume / EnteringNodeInstance.LeavingTransitionInstances.Count;
						return weight;
						//如果弧线的后继结点 是 task结点，那么弧线的权值=前驱同步器结点的容量/输出弧线的数量

					}
					else if (LeavingNodeInstance is SynchronizerInstance)
					{
						SynchronizerInstance synchronizerInstance = (SynchronizerInstance)LeavingNodeInstance;
						weight = synchronizerInstance.Volume / LeavingNodeInstance.EnteringTransitionInstances.Count;
						return weight;
						//如果后继结点是同步器节点，那么权值=同步器的容量/同步器的输入弧线的数量
					}
				}
				return weight;
			}
		}

		public override Boolean take(IToken token)
		{
			EdgeInstanceEvent e = new EdgeInstanceEvent(this);
			e.Token=token;
			e.EventType = EdgeInstanceEventEnum.ON_TAKING_THE_TOKEN;

			for (int i = 0; this.eventListeners != null && i < this.eventListeners.Count; i++)
			{
				IEdgeInstanceEventListener listener = this.eventListeners[i];
				listener.onEdgeInstanceEventFired(e);//调用TransitionInstanceExtension 来计算弧线上的条件表达式
			}

			INodeInstance nodeInst = this.LeavingNodeInstance;//获取到流向哪个节点
			token.Value = this.Weight;//获取到弧线上的权值
			Boolean alive = token.IsAlive;

			nodeInst.fire(token);//节点触发

			return alive;
		}

		public Transition Transition { get { return this.transition; } }

		public String ExtensionTargetName { get { return TransitionInstance.Extension_Target_Name; } }

		public List<String> ExtensionPointNames { get { return TransitionInstance.Extension_Point_Names; } }

		public void registExtension(IKernelExtension extension)
		{
			if (!Extension_Target_Name.Equals(extension.ExtentionTargetName))
			{
				return;
			}
			if (Extension_Point_TransitionInstanceEventListener.Equals(extension.ExtentionPointName))
			{
				if (extension is IEdgeInstanceEventListener)
				{
					this.eventListeners.Add((IEdgeInstanceEventListener)extension);
				}
				else
				{
					throw new Exception("Error:When construct the TransitionInstance,the extension MUST be a instance of ITransitionInstanceEventListener");
				}
			}
		}
	}

}
