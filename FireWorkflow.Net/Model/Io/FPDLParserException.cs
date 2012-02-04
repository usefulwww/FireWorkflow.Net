/*
 * 
 * @author chennieyun
 * @Revision to .NET 无忧 lwz0721@gmail.com 2010-02
 *
 */
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace FireWorkflow.Net.Model.Io
{
    /// <summary>
    ///  FPDLParser错误
    /// </summary>
    public class FPDLParserException : Exception
    {
        /// <summary>
        /// Construct a new FPDLParserException.
        /// </summary>
        public FPDLParserException()
            :base()
        {
        }

        /// <summary>
        /// Construct a new FPDLParserException with the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public FPDLParserException(String message)
            :base(message)
        {
        }

        /// <summary>
        /// Construct a new FPDLParserException with the specified nested error. <see cref="FPDLParserException"/> class.
        /// </summary>
        /// <param name="t">The nested error.</param>
        public FPDLParserException(Exception t)
            :base(t.Message,t)
        {
        }

        /// <summary>
        /// Construct a new FPDLParserException with the specified error message
        /// and nested exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="t">The nested error</param>
        public FPDLParserException(String message, Exception t)
            : base(message, t)
        {
        }
    }
}
