using System;
using System.Runtime.Serialization;

namespace Wof.Controller
{
    class RootInitializationException : Exception
    {
        public RootInitializationException() : base()
        {
             
        }
        public RootInitializationException(string message)
            : base(message)
        {

        }

        public RootInitializationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
        public RootInitializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }


    }
}