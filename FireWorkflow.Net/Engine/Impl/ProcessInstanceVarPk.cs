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

namespace FireWorkflow.Net.Engine.Impl
{
    public class ProcessInstanceVarPk
    {
	    private  const long serialVersionUID = 1441383685880577012L;
        public String ProcessInstanceId { get; set; }
        public String Name { get; set; }
        
        public override Boolean Equals(Object obj)
        {
            if (obj == this) return true;
            if (obj == null) return false;
            if (!(obj is ProcessInstanceVarPk)) return false;
            ProcessInstanceVarPk other = (ProcessInstanceVarPk)obj;

            if (Name == null)
            {
                if (other.Name != null)
                    return false;
            }
            else if (!Name.Equals(other.Name))
                return false;
            if (ProcessInstanceId == null)
            {
                if (other.ProcessInstanceId != null)
                    return false;
            }
            else if (!ProcessInstanceId.Equals(other.ProcessInstanceId))
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + ((Name == null) ? 0 : Name.GetHashCode());
            result = prime * result + ((ProcessInstanceId == null) ? 0 : ProcessInstanceId.GetHashCode());
            return result;
        }
    }
}
