using System;

namespace Apoli
{
    public class ApoliException : Exception
    {
        public ApoliException(string msg):base(msg){}
    }
    public class InvalidParameterException : ApoliException
    {
        public InvalidParameterException(string msg):base(msg){}
    }
    public class TypeUnsetException : ApoliException
    {
        public TypeUnsetException(string msg):base(msg){}
    }
}
