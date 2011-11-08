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
using System.Runtime.Serialization;

namespace FireWorkflow.Net.Model
{
    /// <summary>
    /// 流程元素抽象类
    /// </summary>
    [Serializable]
    public abstract class AbstractWFElement : IWFElement
    {
        #region 属性

        /// <summary>获取或设置元素序列号，请不要在业务代码里面使用该属性的信息。因为这个属性的值是变化的。</summary>
        public String Sn { get; set; }

        /// <summary>
        /// <para>获取工作流元素的Id</para>
        /// 工作流元素的Id采用“父Id.自身Name”的方式组织。
        /// </summary>
        public String Id
        {
            get
            {
                if (this.ParentElement == null) return this.Name;
                else return this.ParentElement.Id + "." + this.Name;
            }
        }

        /// <summary>获取或设置父元素</summary>
        public IWFElement ParentElement { get; set; }

        /// <summary>获取或设置名称，不为空</summary>
        public virtual String Name { get; set; }

        /// <summary>获取或设置显示名称</summary>
        public virtual String DisplayName { get; set; }

        /// <summary>获取或设置描述</summary>
        public virtual String Description { get; set; }

        /// <summary>获取或设置事件监听器</summary>
        public List<EventListener> EventListeners { get; set; }

        /// <summary>获取或设置扩展属性</summary>
        public Dictionary<String, String> ExtendedAttributes { get; set; }

        #endregion

        public AbstractWFElement()
        {
            this.EventListeners = new List<EventListener>();
            this.ExtendedAttributes = new Dictionary<String, String>();
        }

        /// <summary></summary>
        /// <param name="parentElement">父流程元素</param>
        /// <param name="name">本流程元素的名称</param>
        public AbstractWFElement(IWFElement parentElement, String name)
            : this()
        {
            this.ParentElement = parentElement;
            this.Name = name;
        }

        public override String ToString()
        {
            return (String.IsNullOrEmpty(DisplayName)) ? this.Name : this.DisplayName;
        }
        public override bool Equals(object obj)
        {
            return ((obj is IWFElement) &&
                    this.Id.Equals(((AbstractWFElement)obj).Id));
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
