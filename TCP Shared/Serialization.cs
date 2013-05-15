using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Shared
{
    public static class Serialization
    {
        public static T Deserialize<T>(this BinaryFormatter formatter, Stream stream)
        {
            return (T)formatter.Deserialize(stream);
        }
    }
}