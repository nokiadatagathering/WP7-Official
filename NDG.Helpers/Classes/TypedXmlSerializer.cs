using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace NDG.Helpers.Classes
{

    public class TypedXmlSerializer<T>
    {
        private XmlSerializer _serializer;

        public TypedXmlSerializer()
        {
            _serializer = new XmlSerializer(typeof(T));
        }

        public T Deserialize(Stream objectStream)
        {
            return (T)_serializer.Deserialize(objectStream);
        }

        public void Serialize(Stream stream, T objectToSerialize)
        {
            _serializer.Serialize(stream, objectToSerialize);
        }

        public T DeserializeFromXmlString(string objectXml)
        {
            using (var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(objectXml)))
            {
                return Deserialize(xmlStream);
            }
        }

        public string SerializeToXmlString(T objectToSerialize)
        {
            using (var xmlStream = new MemoryStream())
            {
                Serialize(xmlStream, objectToSerialize);
                xmlStream.Position = 0;
                using (var reader = new StreamReader(xmlStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
