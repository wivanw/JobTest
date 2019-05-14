using System;

namespace VBCM.Asserts
{
    [System.Diagnostics.DebuggerStepThrough]
    public class VbcmException : Exception
    {
        public VbcmException(string message)
            : base(message)
        {
        }

        public VbcmException(
            string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}