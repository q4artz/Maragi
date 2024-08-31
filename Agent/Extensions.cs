using System.IO;
using System.Runtime.Serialization.Json;

namespace Agent
{
    public static class Extensions
    {
        // Seralisation is garbage in .net without nuget pkgs, but not adding to keep file size small
        // Add references to System.Runtime.Serialization
        public static byte[] Serialise<T>(this T data)
        {
            var serialiser = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream()) 
            { 
                serialiser.WriteObject(ms, data);
                return ms.ToArray();
            }
        }

        public static T Deserialize<T>(this byte[] data) 
        {
            var serialiser = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream(data))
            {
                // return whatever output from the ms object as T
                return (T) serialiser.ReadObject(ms);
            }
        }
    }
}
