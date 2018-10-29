using System;
using System.Runtime.Serialization;
using Netron.GraphLib.Attributes;
using System.Xml.Serialization;
using System.Reflection;
namespace Netron.GraphLib.Entitology
{
	/// <summary>
	/// Defines a method for the ClassShape class <see cref="ClassShape"/>
	/// </summary>
	[Serializable] public class ClassMethod : ISerializable, IXmlSerializable
	{
		private string mName;
		private string mDataType;
		[GraphMLData]public string Name
		{
			get{return mName;}
			set{mName = value;}
		}

		[GraphMLData]public string DataType
		{
			get{return mDataType;}
			set{mDataType = value;}
		}

		public ClassMethod(){}

		protected ClassMethod(SerializationInfo info, StreamingContext context) 
		{
			this.mName = info.GetString("mName");

			this.mDataType = info.GetString("mDataType");
		}

		public ClassMethod(string name)
		{
			mName = name;
		}
		public ClassMethod(string name, string dataType)
		{
			mName = name;
			mDataType = dataType;
		}

		public ClassMethod(MethodInfo info)
		{
			this.mName = info.Name;
			this.DataType = info.ReturnType.Name;
		}

		#region ISerializable Members

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("mName", mName);

			info.AddValue("mDataType", mDataType);
		}

		#endregion

		#region IXmlSerializable Members

		public void WriteXml(System.Xml.XmlWriter writer)
		{
			writer.WriteStartElement("ClassMethod");
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
