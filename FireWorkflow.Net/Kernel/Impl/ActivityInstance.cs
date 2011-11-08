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
using System.Linq;
using System.Text;
using FireWorkflow.Net.Model;
using FireWorkflow.Net.Model.Net;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Plugin;
using FireWorkflow.Net.Kernel.Event;

namespace FireWorkflow.Net.Kernel.Impl
{
    public class ActivityInstance : AbstractNodeInstance, IActivityInstance
    {
        public const String Extension_Target_Name = "FireWorkflow.Net.Kernel.ActivityInstance";
        public static List<String> Extension_Point_Names = new List<String>();
        public const String Extension_Point_NodeInstanceEventListener = "NodeInstanceEventListener";

        static ActivityInstance()
        {
            Extension_Point_Names.Add(Extension_Point_NodeInstanceEventListener);
        }

        public ActivityInstance(Activity a)
        {
            Activity = a;
        }

        public override String Id
        {
            get { return Activity.Id; }
        }

        public override void fire(IToken tk)
        {
            //log.debug("The weight of the Entering TransitionInstance is " + tk.getValue());
            IToken token = tk;
            token.NodeId=this.Activity.Id;

            //触发TokenEntered事件
            NodeInstanceEvent event1 = new NodeInstanceEvent(this);
            event1.Token=tk;
            event1.EventType=NodeInstanceEventEnum.NODEINSTANCE_TOKEN_ENTERED;//token 来了
            
            this.fireNodeEvent(event1);
            if (token.IsAlive)//如果token是活动的，那么就保存token，并创建taskinstance
            {
                NodeInstanceEvent neevent = new NodeInstanceEvent(this);
                neevent.Token=token;
                neevent.EventType=NodeInstanceEventEnum.NODEINSTANCE_FIRED;//token 被触发,创建taskinstance，等待
                this.fireNodeEvent(neevent);
            }
            else//如果token是dead状态，那么就直接结束当前节点。
            {
                this.complete(token, null);
            }
        }

        /// <summary>结束活动</summary>
        /// <param name="token"></param>
        /// <param name="targetActivityInstance"></param>
        public void complete(IToken token, IActivityInstance targetActivityInstance)
        {
            NodeInstanceEvent event2 = new NodeInstanceEvent(this);
            event2.Token=token;
            event2.EventType=NodeInstanceEventEnum.NODEINSTANCE_LEAVING;//token leaving
            this.fireNodeEvent(event2);


            token.FromActivityId=this.Activity.Id;

            if (targetActivityInstance != null)
            {
                token.StepNumber=token.StepNumber + 1;
                targetActivityInstance.fire(token);
            }
            else
            {
                //按照定义，activity有且只有一个输出弧，所以此处只进行一次循环。
                for (int i = 0; LeavingTransitionInstances != null && i < LeavingTransitionInstances.Count; i++)
                {
                    ITransitionInstance transInst = LeavingTransitionInstances[i];
                    transInst.take(token);
                }
            }

            if (token.IsAlive)
            {
                NodeInstanceEvent neevent = new NodeInstanceEvent(this);
                neevent.Token=token;
                neevent.EventType=NodeInstanceEventEnum.NODEINSTANCE_COMPLETED;//token completed
                this.fireNodeEvent(neevent);
            }
        }

        public override String ExtensionTargetName { get { return Extension_Target_Name; } }

        public override List<String> ExtensionPointNames { get { return Extension_Point_Names; } }

        //TODO extesion是单态还是多实例？单态应该效率高一些。
        public override void registExtension(IKernelExtension extension)
        {
            if (!Extension_Target_Name.Equals(extension.ExtentionTargetName))
            {
                throw new Exception("Error:When construct the ActivityInstance,the Extension_Target_Name is mismatching");
            }
            if (Extension_Point_NodeInstanceEventListener.Equals(extension.ExtentionPointName))
            {
                if (extension is INodeInstanceEventListener)
                {
                    this.EventListeners.Add((INodeInstanceEventListener)extension);
                }
                else
                {
                    throw new Exception("Error:When construct the ActivityInstance,the extension MUST be a instance of INodeInstanceEventListener");
                }
            }
        }

        public override String ToString()
        {
            return "ActivityInstance_4_[" + Activity.Name + "]";
        }

        public Activity Activity { get; set; }
    }

}
