using System;
using System.Collections;
using System.Xml.Serialization;
namespace Netron.GraphLib.Entitology
{
	[Serializable] public class ClassPropertyCollection : CollectionBase, IXmlSerializable
	{
		public int Add(ClassProperty property)
		{
			return InnerList.Add(property);
		}

		public ClassProperty this[int index]
		{
			get{return InnerList[index] as ClassProperty ;}
		}
		public ClassPropertyCollection(){}

		public ClassPropertyCollection(ArrayList list)
		{
			for(int l=0; l<list.Count; l++)
			{
				string[] str = list[l] as string[];
				this.Add(new ClassProperty(str[0],str[3]));//the order is the same as the order the GrapjML attributes are applied
			}
		}

		public int Contains(string name)
		{
			for(int k=0; k<InnerList.Count; k++)
			{
				if(this[k].Name==name) return k;
			}
			return -1;
		}
		#region IXmlSerializable Members

		public void WriteXml(System.Xml.XmlWriter writer)
		{
			for(int k=0; k<InnerList.Count; k++)
			{
				this[k].WriteXml(writer);
			}
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
