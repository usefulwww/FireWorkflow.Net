/* Copyright 2009 无忧lwz0721@gmail.com
 * @author 无忧lwz0721@gmail.com
 */
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.Text;
using System.Reflection;

namespace FireWorkflow.Net.Base
{
    /// <summary>
    /// 表达式解析
    /// </summary>
    public class Expressions
    {
        #region Construction

        /// <summary>生成表达式类</summary>
        /// <param name="returnType">返回类型</param>
        /// <param name="expression">表达式</param>
        /// <param name="name">方法名称</param>
        /// <param name="Keys">参数传递</param>
        public Expressions(Type returnType, string expression, string name, Dictionary<String, Object> Keys)
        {
            Dictionary<string, string> providerOptions = new Dictionary<string, string>();
            providerOptions.Add("CompilerVersion", "v3.5");
            CSharpCodeProvider csp = new CSharpCodeProvider(providerOptions);

            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("system.dll");
            cp.ReferencedAssemblies.Add("system.data.dll");
            cp.ReferencedAssemblies.Add("system.xml.dll");
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;

            StringBuilder code = new StringBuilder();
            code.Append("using System; \n");
            code.Append("using System.Data; \n");
            code.Append("using System.Data.SqlClient; \n");
            code.Append("using System.Data.OleDb; \n");
            code.Append("using System.Xml; \n");
            code.Append("namespace ISM.DynamicallyGenerated { \n");
            code.Append("  public class _DG { \n");
            int i = 0;

            if (Keys != null && Keys.Count > 0)
            {
                code.AppendFormat("    public {0} {1}(", returnType.Name, name);
                i = 0;
                foreach (String key in Keys.Keys)
                {
                    if (i > 0) code.Append(" ,");
                    i++;
                    code.AppendFormat("{0} {1}", Keys[key].GetType().Name, key);
                }
                code.Append(") \n");
                code.Append("{ \n");
                code.AppendFormat("      return ({0});\n ", expression);
                code.Append("}\n");
            }
            else
            {

                code.AppendFormat("    public {0} {1}() ", returnType.Name, name);
                code.Append("{ ");
                code.AppendFormat("      return ({0}); ", expression);
                code.Append("}\n");
            }

            code.Append("} }");

            //ICodeCompiler comp = csp.CreateCompiler();
            CompilerResults cr = csp.CompileAssemblyFromSource(cp, code.ToString());
            if (cr.Errors.HasErrors)
            {
                StringBuilder error = new StringBuilder();
                error.Append("Error Compiling Expression: ");
                foreach (CompilerError err in cr.Errors)
                {
                    error.AppendFormat("{0}\n", err.ErrorText);
                }
                throw new Exception("Error Compiling Expression: " + error.ToString());
            }
            Assembly a = cr.CompiledAssembly;
            _Compiled = a.CreateInstance("ISM.DynamicallyGenerated._DG");
        }
        #endregion

        #region Public Members

        /// <summary>
        /// 执行
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="name">执行方法名</param>
        /// <param name="keys">参数</param>
        public T Evaluate<T>(string name, Dictionary<String, Object> keys)
        {
            if (keys == null && keys.Keys == null) return default(T);
            MethodInfo mi = _Compiled.GetType().GetMethod(name);
            List<object> os = new List<object>();
            foreach (object o in keys.Values)
            {
                os.Add(o);
            }
            return (T)mi.Invoke(_Compiled, os.ToArray());
        }
        #endregion

        #region Private
        object _Compiled = null;
        #endregion
    }
}
