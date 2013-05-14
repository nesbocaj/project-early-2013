using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Shared
{
    public class Response<T>
    {
        private T _value;
        private string _code, _message;

        public Response() { }

        public Response(T value, string code, string message)
        {
            _value = value;
            _code = code;
            _message = message;
        }

        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
