using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireWorkflow.Net.Base
{
    /// <summary>
    /// 事件数据
    /// </summary>
    public class EventObject : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        protected Object source;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventObject"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public EventObject(Object source)
        {
            this.source = source;
        }
        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <returns></returns>
        public Object getSource()
        {
            return source;
        }
    }
}
