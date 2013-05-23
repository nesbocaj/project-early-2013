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
    /// <summary>
    /// Represents a container for a response to a TCP request
    /// </summary>
    /// <typeparam name="T">The type of the value returned by the request</typeparam>
    [Serializable] // makes serialization possible
    public class Response<T>
    {
        private T _value;
        private string _code, _message;
        private bool _succeeded;
        private static BinaryFormatter _formatter;

        /// <summary>
        /// Initializes a new instance of the TCP_Shared.Response&lt;T&gt; class
        /// </summary>
        public Response() { }

        /// <summary>
        /// Initializes a new instance of the TCP_Shared.Response&lt;T&gt; class
        /// with the parameters code, message and succeeded
        /// </summary>
        /// <param name="code">Defines the response code</param>
        /// <param name="message">Defines the complete message</param>
        /// <param name="succeeded">Defines whether or not the request was succesful</param>
        public Response(string code, string message, bool succeeded)
        {
            _code = code;
            _message = message;
            _succeeded = succeeded;
        }

        /// <summary>
        /// Initializes a new instance of the TCP_Shared.Response&lt;T&gt; class
        /// with the parameters value, code, message and succeeded
        /// </summary>
        /// <param name="value">The value returned by the request</param>
        /// <param name="code">The status code</param>
        /// <param name="message">The complete message</param>
        /// <param name="succeeded">Defines whether or not the request was succesful</param>
        public Response(T value, string code, string message, bool succeeded = true)
            : this(code, message, succeeded)
        {
            _value = value;
        }

        /// <summary>
        /// Creates a new TCP_Shared.Response&lt;T&gt; based on a serialized version
        /// where T is the type of the serialized response
        /// </summary>
        /// <param name="serialized">Represents a serialized TCP_Shared.Response&lt;T&gt;</param>
        /// <returns>A deserialized response instance based on the input</returns>
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

        /// <summary>
        /// Gets the value in the current TCP_Shared.Response&lt;T&gt; object
        /// </summary>
        public T Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets whether or not the request
        /// that generated the current TCP_Shared.Response&lt;T&gt; object succeeded
        /// </summary>
        public bool Succeeded
        {
            get { return _succeeded; }
        }

        /// <summary>
        /// Gets the status code of the current TCP_Shared.Response&lt;T&gt; object
        /// </summary>
        public string Code
        {
            get { return _code; }
        }

        /// <summary>
        /// Gets the message of the current TCP_Shared.Response&lt;T&gt; object
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Generates a serialized version of the current TCP_Shared.Response&lt;T&gt; object
        /// </summary>
        /// <returns>A serialized version of the current TCP_Shared.Response&lt;T&gt; object</returns>
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
