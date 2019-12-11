using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace HelpDesk.API.GenericHelpers
{
    public class CustomXmlSerializer<T> where T : class
    {
        // to use var xml = CustomXmlSerializer<ClassName>.Serialize(ClassObject);
        public static string Serialize(T source)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            return Serialize(source, ns, GetIndentedSettings());
        }

        public static string Serialize(T source, XmlSerializerNamespaces namespaces, XmlWriterSettings settings)
        {
            if (source == null)
                throw new ArgumentNullException("source", "Object to serialize cannot be null");
            string xml = null;
            XmlSerializer serializer = new XmlSerializer(source.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlWriter xmlWriter = XmlWriter.Create(memoryStream, settings);

                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(T));
                x.Serialize(xmlWriter, source, namespaces);
                memoryStream.Position = 0;

                using (StreamReader sr = new StreamReader(memoryStream))
                {
                    xml = sr.ReadToEnd();
                }

                xmlWriter = null;
            }
            return xml;
        }

        private static XmlWriterSettings GetIndentedSettings()
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.IndentChars = "\t";
            xmlWriterSettings.Encoding = Encoding.Unicode;
            return xmlWriterSettings;
        }
    }
}