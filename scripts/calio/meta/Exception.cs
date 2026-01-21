using System;

namespace Calio
{
    public class CalioException : Exception
    {
        public CalioException(string msg) : base(msg) { }
        public CalioException() {}
    }
    public class InvalidParserException : CalioException
    {
        public InvalidParserException(string msg) : base(msg) { }
    }
    public class InvalidParseParamException : CalioException
    {
        public InvalidParseParamException(string msg) : base(msg) { }
    }
    public class JSONKeyNotFoundException : CalioException
    {
        public JSONKeyNotFoundException(string msg) : base(msg) { }
    }
}
