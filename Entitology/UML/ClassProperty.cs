using System;
using Netron.GraphLib.Attributes;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
namespace Netron.GraphLib.Entitology
{
	[Serializable] public class ClassProperty : ISerializable, IXmlSerializable
	{
		private string mName;
		private string mDataType;
		private bool mCanRead;
		private bool mCanWrite;
		
		[GraphMLData]public string Name
		{
			get{return mName;}
			set{mName = value;}
		}

		public string GetSetName
		{
			get{
				string addon = string.Empty;
				if(CanRead) 
				{
					addon+= "get";
					if(CanWrite) addon += "/set";
				}
				else if(CanWrite)
				{
					addon +="set";
				}
				if(addon!=string.Empty)
					addon = " [" + addon + "]";
				return mName + addon ;
			
			}
			
		}

		[GraphMLData]public bool CanRead
		{
			get{return mCanRead;}
			set{mCanRead = value;}
		}
		[GraphMLData]public bool CanWrite
		{
			get{return mCanWrite;}
			set{mCanWrite = value;}
		}


		[GraphMLData]public string DataType
		{
			get{return mDataType;}
			set{mDataType = value;}
		}

		public ClassProperty(){}

		public ClassProperty(string name)
		{
			mName = name;
		}

		public ClassProperty(string name, string dataType)
		{
			mName = name;
			mDataType = dataType;
		}
		public ClassProperty(string name, string dataType, bool canRead, bool canWrite): this(name, dataType)
		{
			mCanRead = canRead;
			mCanWrite = canWrite;
		}

		public ClassProperty(PropertyInfo info)
		{
			mCanRead = info.CanRead;
			mCanWrite = info.CanWrite;
			mName = info.Name;
			mDataType = info.PropertyType.Name;
		}

		protected ClassProperty(SerializationInfo info, StreamingContext context)
		{
			this.mCanRead = info.GetBoolean("mCanRead");
			
			this.CanWrite = info.GetBoolean("mCanWrite");

			this.mName = info.GetString("mName");

			this.mDataType = info.GetString("mDataType");
		}

		#region ISerializable Members

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("mCanRead", mCanRead);
			
			info.AddValue("mCanWrite", mCanWrite);

			info.AddValue("mName", mName);

			info.AddValue("mDataType", mDataType);

		}

		#endregion

		#region IXmlSerializable Members

		public void WriteXml(System.Xml.XmlWriter writer)
		{
			writer.WriteStartElement("ClassProperty");
			writer.WriteElementString("Name",mName);
			writer.WriteElementString("DataType",mDataType);
			writer.WriteEndElement();
		}

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			// TODO:  Add ClassPropertyCollection.GetSchema implementation
			return null;
		}

		public void ReadXml(System.Xml.XmlReader reader)
		{
			// TODO:  Add ClassPropertyCollection.ReadXml implementation
		}

		#endregion
	}

	
}
