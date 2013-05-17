using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Shared
{
    [Serializable]
    public class Response<T>
    {
        private T _value;
        private string _code, _message;
        private bool _succeeded;
        private static BinaryFormatter _formatter;

        public Response() { }

        public Response(string code, string message, bool succeeded)
        {
            _code = code;
            _message = message;
            _succeeded = succeeded;
        }

        public Response(T value, string code, string message, bool succeeded = true)
            : this(code, message, succeeded)
        {
            _value = value;
        }

        public static Response<T> FromSerialized(string serialized)
        {
            if (_formatter == null)
                _formatter = new BinaryFormatter();

            using (var stream = new MemoryStream(Convert.FromBase64String(serialized)))
            {
                stream.Position = 0;
                return _formatter.Deserialize<Response<T>>(stream);
            }
        }

        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public bool Succeeded
        {
            get { return _succeeded; }
            set { _succeeded = value; }
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

        public string Serialize()
        {
            string serialized = "error";

            if (_formatter == null)
                _formatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                _formatter.Serialize(stream, this);
                stream.Position = 0;
                serialized = Convert.ToBase64String(stream.ToArray());
            }

            return serialized;
        }
    }
}
