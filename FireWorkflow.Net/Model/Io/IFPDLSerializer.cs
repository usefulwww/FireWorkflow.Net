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
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FireWorkflow.Net.Model.Io
{
    /// <summary>
    /// FPDL序列化器。将WorkflowProcess对象序列化到一个输出流。
    /// </summary>
    public abstract class IFPDLSerializer : FPDLNames
    {

        /// <summary>
        /// 将WorkflowProcess对象序列化到一个输出流。
        /// </summary>
        /// <param name="workflowProcess">工作流定义</param>
        /// <param name="swout">输出流</param>
        public abstract void serialize(WorkflowProcess workflowProcess, Stream swout);
        /*
	public void serialize(WorkflowProcess workflowProcess, Writer out)
			throws IOException, FPDLSerializerException;
         */

    }
}
