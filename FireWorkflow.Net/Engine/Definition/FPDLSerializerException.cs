using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireWorkflow.Net.Engine.Definition
{
    public class FPDLSerializerException : Exception
    {

        /** Construct a new FPDLSerializerException. */

        public FPDLSerializerException()
            : base()
        {
        }

        /** Construct a new FPDLSerializerException with the given error message.

            @param message The error message
        */

        public FPDLSerializerException(String message)
            : base(message)
        {
        }

        /** Construct a new FPDLSerializerException with the given nested error.

            @param t The nested error
        */

        public FPDLSerializerException(Exception t)
            : base(t.Message, t)
        {
        }

        /** Construct a new FPDLSerializerException with the given error message
            and nested error.

            @param message The error message
            @param t The error
        */

        public FPDLSerializerException(String message, Exception t)
            : base(message, t)
        {
        }

    }
}
