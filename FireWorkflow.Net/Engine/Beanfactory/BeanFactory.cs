using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace FireWorkflow.Net.Engine.Beanfactory
{
    /// <summary>
    /// 
    /// </summary>
    public class BeanFactory : IBeanFactory
    {
        #region IBeanFactory 成员

        /// <summary>
        /// 根据bean的名字返回bean的实例
        /// </summary>
        /// <param name="beanName">bean name具体含义是什么由IBeanFactory的实现类来决定</param>
        /// <returns></returns>
        public object GetBean(string beanName)
        {
            Type type = Type.GetType(beanName);
            if (type != null) return Activator.CreateInstance(type, null);
            else throw new Exception(String.Format("({0})初始化失败。", beanName));
        }

        /// <summary>
        /// 根据bean的名字返回bean的实例
        /// </summary>
        /// <param name="beanName">bean name具体含义是什么由IBeanFactory的实现类来决定</param>
        /// <param name="args">与要调用构造函数的参数数量、顺序和类型匹配的参数数组。如果 args 为空数组或 nullNothingnullptrnull 引用（在 Visual Basic 中为 Nothing），则调用不带任何参数的构造函数（默认构造函数）。</param>
        /// <returns></returns>
        public object GetBean(string beanName, params Object[] args)
        {
            Type type = Type.GetType(beanName);
            if (type != null) return Activator.CreateInstance(type, args);
            else throw new Exception(String.Format("({0})初始化失败。", beanName));
        }
        #endregion
    }
}
