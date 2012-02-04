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

namespace FireWorkflow.Net.Model
{
    /// <summary>Fire workflow保留的扩展属性的名称，工作流自定义的扩展属性不要使用这些名字</summary>
    public class ExtendedAttributeNames
    {
        public const String BOUNDS_X = "FIRE_FLOW.bounds.x";
        public const String BOUNDS_Y = "FIRE_FLOW.bounds.y";
        public const String BOUNDS_WIDTH = "FIRE_FLOW.bounds.width";
        public const String BOUNDS_HEIGHT = "FIRE_FLOW.bounds.height";

        public const String EDGE_POINT_LIST = "FIRE_FLOW.edgePointList";
        public const String LABEL_POSITION = "FIRE_FLOW.labelPosition";

        public const String ACTIVITY_LOCATION = "FIRE_FLOW.activityLocation";
    }
}
