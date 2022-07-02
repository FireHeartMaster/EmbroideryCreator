using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    public static class SerializerHelper
    {
        public static void WriteToFile<T>(string filePath, T objectToSave)
        {
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Create))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binaryFormatter.Serialize(stream, objectToSave);
                }
            }
            catch (IOException)
            {

            }
        }

        public static T ReadFromFile<T>(string filePath) where T: class
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (Stream stream = File.Open(filePath, FileMode.Open)){
                        var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        return (T)binaryFormatter.Deserialize(stream);
                    }
                }
            }
            catch (IOException)
            {

            }

            return null;
        }
    }
}
