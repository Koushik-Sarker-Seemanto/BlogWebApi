using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CacheProcessorService
{
    public class SerializeDeserialize
    {
        // Serialize collection of any type to a byte stream

        public static byte[] Serialize<T>(T obj)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binSerializer = new BinaryFormatter();
                binSerializer.Serialize(memStream, obj);
                return memStream.ToArray();
            }
        }

        // DSerialize collection of any type to a byte stream

        public static T Deserialize<T>(byte[] serializedObj)
        {
            T obj = default(T);
            using (MemoryStream memStream = new MemoryStream(serializedObj))
            {
                BinaryFormatter binSerializer = new BinaryFormatter();
                obj = (T)binSerializer.Deserialize(memStream);
            }
            return obj;
        }

        
    }
}