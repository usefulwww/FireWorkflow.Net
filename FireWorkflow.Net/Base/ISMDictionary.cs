using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Text;

namespace FireWorkflow.Net.Base
{
    public class ISMDictionary : Dictionary<String, String>, IXmlSerializable
    {
        
        #region IXmlSerializable 成员

        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(String));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(String));
            if (reader.IsEmptyElement || !reader.Read())
            {
                return;
            }
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("fpdl:ExtendedAttribute");

                reader.ReadStartElement("Name");
                String key = reader.Value;// (String)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("Value");
                String value = (String)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadEndElement();
                reader.MoveToContent();
                this.Add(key, value);
            }
            reader.ReadEndElement();
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        
        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(String));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(String));

            foreach (String key in this.Keys)
            {
                //writer.WriteStartElement("fpdl", "ExtendedAttribute");
                //writer.WriteElementString
                writer.WriteStartElement("fpdl:ExtendedAttribute");
                writer.WriteAttributeString("Name", key);
                writer.WriteAttributeString("Value", this[key]);
                writer.WriteEndElement();
//XmlAttribute
                //writer.WriteStartElement("key");
                //keySerializer.Serialize(writer, key);
                //writer.WriteEndElement();
                //writer.WriteStartElement("value");
                //valueSerializer.Serialize(writer, this[key]);
                //writer.WriteEndElement();

            }
        }

        #endregion
    }
}
