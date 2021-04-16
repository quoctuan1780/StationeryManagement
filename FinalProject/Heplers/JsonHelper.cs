using Common;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace FinalProject.Heplers
{
    public class JsonHelper
    {
        public static string ObjectToJSONString(object serializableObject)
        {
            var memoryStream = new MemoryStream();
            var writer = JsonReaderWriterFactory.CreateJsonWriter(
                        memoryStream, Encoding.UTF8, true, true, Constant.DOUBLE_EMPTY);
            var ser = new DataContractJsonSerializer(serializableObject.GetType(), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
            ser.WriteObject(writer, serializableObject);
            memoryStream.Position = 0;
            var sr = new StreamReader(memoryStream);
            return sr.ReadToEnd();
        }
    }
}
