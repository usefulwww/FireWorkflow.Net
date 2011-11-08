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

namespace FireWorkflow.Net.Kernel
{
    public interface IEdgeInstance
    {
        String Id { get; }

        /// <summary>
        /// 弧的权
        /// </summary>
        int Weight { get; }

        /// <summary>获取输出到达的节点实例</summary>
        INodeInstance LeavingNodeInstance { get; set; }

        //void setLeavingNodeInstance(INodeInstance nodeInst);

        INodeInstance EnteringNodeInstance { get; set; }

        //void setEnteringNodeInstance(INodeInstance nodeInst);

        /// <summary>
        /// 接受一个token，并移交给下一个节点
        /// </summary>
        /// <param name="token"></param>
        /// <returns>返回值是该transition计算出的token的alive值</returns>
        Boolean take(IToken token);// throws KernelException;
    }
}
