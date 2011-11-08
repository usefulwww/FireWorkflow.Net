/*
 Copyright (C) 2002-2003 Anthony Eden.
 All rights reserved.

 Redistribution and use in source and binary forms, with or without
 modification, are permitted provided that the following conditions
 are met:

 1. Redistributions of source code must retain the above copyright
    notice, this list of conditions, and the following disclaimer.

 2. Redistributions in binary form must reproduce the above copyright
    notice, this list of conditions, and the disclaimer that follows
    these conditions in the documentation and/or other materials
    provided with the distribution.

 3. The names "OBE" and "Open Business Engine" must not be used to
    endorse or promote products derived from this software without prior
    written permission.  For written permission, please contact
    me@anthonyeden.com.

 4. Products derived from this software may not be called "OBE" or
    "Open Business Engine", nor may "OBE" or "Open Business Engine"
    appear in their name, without prior written permission from
    Anthony Eden (me@anthonyeden.com).

 In addition, I request (but do not require) that you include in the
 end-user documentation provided with the redistribution and/or in the
 software itself an acknowledgement equivalent to the following:
     "This product includes software developed by
      Anthony Eden (http://www.anthonyeden.com/)."

 THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED
 WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR(S) BE LIABLE FOR ANY DIRECT,
 INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
 IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 POSSIBILITY OF SUCH DAMAGE.
 For more information on OBE, please see <http://www.openbusinessengine.org/>.
 
 @Revision to .NET 无忧 lwz0721@gmail.com 2010-02
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireWorkflow.Net.Model
{
    /// <summary>时间间隔</summary>
    [Serializable]
    public class Duration
    {
        #region 属性
        /// <summary>获取或设置时间值</summary>
        public int Value { get; set; }

        /// <summary>获取或设置时间单位</summary>
        public UnitEnum Unit { get; set; }

        /// <summary>设置或获取时间间隔的属性，isBusinessTime==true表示时间间隔指工作时间</summary>
        public Boolean IsBusinessTime { get; set; }
        #endregion

        #region 构造函数
        /// <summary>创建时间间隔对象</summary>
        /// <param name="value">时间值</param>
        /// <param name="unit">时间单位</param>
        public Duration(int value, UnitEnum unit)
        {
            this.Value = value;
            this.Unit = unit;
            this.IsBusinessTime = true;
        }
        #endregion

        #region 方法
        /// <summary>获取时间单位，如果时间单位为null，则返回defaultUnit</summary>
        /// <param name="defaultUnit"></param>
        /// <returns></returns>
        public UnitEnum getUnit(UnitEnum defaultUnit)
        {
            if (Unit == UnitEnum.Null)
            {
                return defaultUnit;
            }
            else
            {
                return Unit;
            }
        }

        /// <summary>获取换算成毫秒的时间间隔值</summary>
        /// <param name="defaultUnit"></param>
        /// <returns></returns>
        public long getDurationInMilliseconds(UnitEnum defaultUnit)
        {
            int value = Value;
            UnitEnum unit = getUnit(defaultUnit);
            if (value == 0)
            {
                return value;
            }
            else
            {
                long duration = value * toMilliseconds(unit);
                return duration;
            }
        }

        public long toMilliseconds(UnitEnum unit)
        {
            switch (unit)
            {
                case UnitEnum.Null: return 0L;
                case UnitEnum.YEAR: return 365 * 30 * 24 * 60 * 60 * 1000L;
                case UnitEnum.MONTH: return 30 * 24 * 60 * 60 * 1000L;
                case UnitEnum.WEEK: return 7 * 24 * 60 * 60 * 1000L;
                case UnitEnum.DAY: return 24 * 60 * 60 * 1000L;
                case UnitEnum.HOUR: return 60 * 60 * 1000L;
                case UnitEnum.MINUTE: return 60 * 1000L;
                case UnitEnum.SECOND: return 1 * 1000L;
                default: return 0L;
            }
        }

        public override String ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append(Value);
            if (Unit != UnitEnum.Null)
            {
                buffer.Append(Unit);
            }
            return buffer.ToString();
        }
        #endregion
    }
    #region 枚举
    /// <summary>
    /// 时间方式
    /// </summary>
    public enum UnitEnum
    {
        Null,
        /// <summary>年</summary>
        YEAR,
        /// <summary>月</summary>
        MONTH,
        /// <summary>周</summary>
        WEEK,
        /// <summary>天</summary>
        DAY,
        /// <summary>小时</summary>
        HOUR,
        /// <summary>一分钟</summary>
        MINUTE,
        /// <summary>秒</summary>
        SECOND
    }
    #endregion
}
