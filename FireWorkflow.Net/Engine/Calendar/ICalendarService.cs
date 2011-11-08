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
using System.Text;
using FireWorkflow.Net.Model;

namespace FireWorkflow.Net.Engine.Calendar
{
    /// <summary>
    /// 日历服务
    /// </summary>
    public interface ICalendarService : IRuntimeContextAware
    {

        /// <summary>
        /// 获得fromDate后相隔duration的某个日期
        /// Get the date after the duration
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        DateTime dateAfter(DateTime fromDate, Duration duration);

        /// <summary>
        /// <para>缺省实现，周六周日都是非工作日，其他的都为工作日。</para>
        /// <para>实际应用中，可以在数据库中建立一张非工作日表，将周末以及法定节假日录入其中，</para>
        /// <para>然后在该方法中读该表的数据来判断工作日和非工作日。</para>
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        Boolean IsBusinessDay(DateTime d);

        /// <summary>
        /// 获得系统时间
        /// </summary>
        /// <returns></returns>
        DateTime getSysDate();
    }
}
