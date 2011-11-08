/*
 * 
 * @author chennieyun
 * @Revision to .NET 无忧 lwz0721@gmail.com 2010-02
 *
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FireWorkflow.Net.Model.Io
{
    /// <summary>
    /// FPDL解析器，将一个xml格式的fpdl流程定义文件解析成WorkflowProcess对象。
    /// </summary>
    public abstract class IFPDLParser : FPDLNames
    {

        /// <summary>
        /// 将输入流解析成为一个WorkflowProcess对象。
        /// </summary>
        /// <param name="srin">输入流</param>
        /// <returns>返回WorkflowProcess对象</returns>
        public abstract WorkflowProcess parse(Stream srin);

        /// <summary>
        /// 将字符串解析成为一个WorkflowProcess对象。
        /// </summary>
        /// <param name="srin">字符串</param>
        /// <returns>返回WorkflowProcess对象</returns>
        public abstract WorkflowProcess parse(string srin);
    }
}
