using System;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;



namespace  Netron.Neon
{
	/// <summary>
	/// Encapsulates a single task item
	/// Task = description + filename + linenumber + status
	/// </summary>
	public class NTask
	{
		#region Fields
		protected string description = "";
		protected string fileName = "";
		protected string lineNumber="";
		protected string status = "";
		protected bool isChecked = false;
		protected Color color = Color.Black;
		protected bool strikeout = false;
		#endregion

		#region Properties

		public bool Strikeout
		{
			get{return strikeout;}
			set{strikeout = value;}
		}

		public Color Color
		{
			get{return color;}
			set{color = value;}
		}

		public string Description
		{
			get{return description;}
			set{description =value;}
		}

		public string FileName
		{
			get{return fileName;}
			set{fileName=value;}
		}

		public bool IsChecked
		{
			get{return isChecked;}
			set{isChecked =value;}
		}

		public string LineNumber
		{
			get{return lineNumber;}
			set{lineNumber = value;}
		}

		public string Status
		{
			get{return status;}
			set{status = value;}
		}



		#endregion


		public NTask()
		{
			
		}

		public NTask(string description)
		{
			this.description=description;
		}

		public NTask(string description, bool isChecked)
		{
			this.description=description;
			this.isChecked=isChecked;
		}

		public NTask(string description, bool isChecked, string fileName,string lineNumber )
		{
			this.description=description;
			this.isChecked=isChecked;
			this.fileName=fileName;
			this.lineNumber=lineNumber;
		}
		public NTask(string description, bool isChecked, string fileName,string lineNumber, string status)
		{
			this.description=description;
			this.isChecked=isChecked;
			this.fileName=fileName;
			this.lineNumber=lineNumber;
			this.status = status;
		}
		public NTask(string description, bool isChecked, string fileName,string lineNumber , Color color)
		{
			this.description=description;
			this.isChecked=isChecked;
			this.fileName=fileName;
			this.lineNumber=lineNumber;
			this.color = color;
		}
		public NTask(string description, bool isChecked, string fileName,string lineNumber , string status, Color color)
		{
			this.description=description;
			this.isChecked=isChecked;
			this.fileName=fileName;
			this.lineNumber=lineNumber;
			this.color = color;
			this.status = status;
		}
		public NTask(string description, bool isChecked, string fileName,string lineNumber , Color color, bool strikeout)
		{
			this.description=description;
			this.isChecked=isChecked;
			this.fileName=fileName;
			this.lineNumber=lineNumber;
			this.color = color;
			this.strikeout = strikeout;
		}
		public NTask(string description, bool isChecked, string fileName,string lineNumber, string status, Color color, bool strikeout)
		{
			this.description=description;
			this.isChecked=isChecked;
			this.fileName=fileName;
			this.lineNumber=lineNumber;
			this.color = color;
			this.status = status;
			this.strikeout = strikeout;
		}
	}
}
