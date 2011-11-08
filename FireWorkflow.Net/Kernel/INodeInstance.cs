using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FireWorkflow.Net.Kernel.Event;
using FireWorkflow.Net.Model;

namespace FireWorkflow.Net.Kernel
{

    /// <summary>
    /// NodeInstance应该是无状态的，不会随着ProcessInstance的增加而增加。??)
    /// </summary>
    public interface INodeInstance
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        String Id { get; }
        /// <summary>node 触发 (最核心的方法) </summary>
        /// <param name="token"></param>
        void fire(IToken token);// throws KernelException;

        /// <summary>获取输出弧的实例</summary>
        /// <returns></returns>
        List<ITransitionInstance> LeavingTransitionInstances { get; set; }
        /// <summary>
        /// Adds the leaving transition instance.
        /// </summary>
        /// <param name="transitionInstance">The transition instance.</param>
        void AddLeavingTransitionInstance(ITransitionInstance transitionInstance);

        /// <summary>
        /// Gets or sets the entering transition instances.
        /// </summary>
        /// <value>The entering transition instances.</value>
        List<ITransitionInstance> EnteringTransitionInstances { get; set; }
        /// <summary>
        /// Adds the entering transition instance.
        /// </summary>
        /// <param name="transitionInstance">The transition instance.</param>
        void AddEnteringTransitionInstance(ITransitionInstance transitionInstance);

        /// <summary>
        /// Gets or sets the leaving loop instances.
        /// </summary>
        /// <value>The leaving loop instances.</value>
        List<ILoopInstance> LeavingLoopInstances { get; set; }
        /// <summary>
        /// Adds the leaving loop instance.
        /// </summary>
        /// <param name="loopInstance">The loop instance.</param>
        void AddLeavingLoopInstance(ILoopInstance loopInstance);

        /// <summary>
        /// Gets or sets the entering loop instances.
        /// </summary>
        /// <value>The entering loop instances.</value>
        List<ILoopInstance> EnteringLoopInstances { get; set; }
        /// <summary>
        /// Adds the entering loop instance.
        /// </summary>
        /// <param name="loopInstance">The loop instance.</param>
        void AddEnteringLoopInstance(ILoopInstance loopInstance);

        /// <summary>
        /// Gets or sets the event listeners.
        /// </summary>
        /// <value>The event listeners.</value>
        List<INodeInstanceEventListener> EventListeners { get; set; }


    }
}
