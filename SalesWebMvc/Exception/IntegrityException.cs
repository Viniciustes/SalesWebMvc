using System;

namespace SalesWebMvc.Exception
{
    public class IntegrityException : ApplicationException
    {
        public IntegrityException(string message) : base(message) { }
    }
}
