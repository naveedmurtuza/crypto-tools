using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CryptoTools.Utilities
{
    /// <summary>
    /// Generic Serialize class
    /// </summary>
    class SerializeUtils
    {
        public static void SerializeToXml<T>(T t, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, t);
            textWriter.Close();
        }

        public static T DeserializeFromXml<T>(string path)
        {
            if (!File.Exists(path))
                return default(T);
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            TextReader textReader = new StreamReader(path);
            T movies;
            movies = (T)deserializer.Deserialize(textReader);
            textReader.Close();

            return movies;
        }
    }
}
