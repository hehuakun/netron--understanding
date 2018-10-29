using System;
using System.Collections;
using System.Xml.Serialization;
namespace Netron.GraphLib.Entitology
{
	[Serializable] public class ClassMethodCollection : CollectionBase, IXmlSerializable
	{
		public int Add(ClassMethod method)
		{
			return InnerList.Add(method);
		}

		public ClassMethod this[int index]
		{
			get{return InnerList[index] as ClassMethod ;}
		}
		public ClassMethodCollection(){}

		/// <summary>
		/// Allows to fill the collection via reflection when deserializing from Xml
		/// </summary>
		/// <param name="list"></param>
		public ClassMethodCollection(ArrayList list)
		{
			for(int k=0; k<list.Count; k++)
			{
				try{
					string[] elements = list[k] as string[];
					this.Add(new ClassMethod(elements[0], elements[1]));
				}
				catch
				{}
			}
		}

		#region IXmlSerializable Members

		public void WriteXml(System.Xml.XmlWriter writer)
		{
			//System.Text.StringBuilder sb = new System.Text.StringBuilder();
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
