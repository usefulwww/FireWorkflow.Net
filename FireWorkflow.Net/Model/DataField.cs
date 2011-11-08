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
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace FireWorkflow.Net.Model
{
    /// <summary>流程变量</summary>
    public class DataField : AbstractWFElement
    {
        #region 属性
        /// <summary>获取或设置返回流程变量的数据类型</summary>
        [XmlAttribute]
        public DataTypeEnum DataType { get; set; }

        /// <summary>获取或设置初始值</summary>
        [XmlAttribute]
        public String InitialValue { get; set; }

        /// <summary>获取或设置数据的pattern，目前主要用于日期类型。如 yyyyMMdd 等等。</summary>
        [XmlAttribute]
        public String DataPattern { get; set; }
        #endregion

        #region 构造函数
        public DataField()
        {
            this.DataType=DataTypeEnum.STRING;
        }

        public DataField(WorkflowProcess workflowProcess, String name, DataTypeEnum dataType)
            : base(workflowProcess, name)
        {
            this.DataType = dataType;
        }
        #endregion
    }

    #region
    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataTypeEnum
    {
        /// <summary>字符串类型</summary>
        STRING,
        /// <summary>浮点类型</summary>
        FLOAT,
        /// <summary>双精度类型</summary>
        DOUBLE,
        /// <summary>整数类型</summary>
        INTEGER,
        /// <summary>长整型</summary>
        LONG,
        /// <summary>日期时间类型</summary>
        DATETIME,
        /// <summary>布尔类型</summary>
        BOOLEAN
    }
    #endregion
}
