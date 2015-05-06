// Project: UI, File: TextField.cs
// Namespace: ArcSpooler.UI, Class: TextField
// Path: D:\Development\ArcSpooler\UI, Author: arnelm
// Code lines: 21, Size of file: 281 Bytes
// Creation date: 10/31/2008 7:25 PM
// Last modified: 11/4/2008 11:07 AM

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace ArcSpooler.UI
{
	public class TextField
	{
		#region Constructors
		/// <summary>
		/// Create text field
		/// </summary>
		public TextField(string name)
		{
			Name = name;
		} // TextField()

		/// <summary>
		/// Create text field
		/// </summary>
		/// <param name="modifyTo">Modify to</param>
		public TextField(string name, string modifyTo)
			: this(name)
		{
			ModifyTo = modifyTo;
		} // TextField(modifyTo)
		#endregion

		#region Properties
		/// <summary>
		/// Name
		/// </summary>
		/// <returns>String</returns>
		public string Name { get; private set; } // Name

		/// <summary>
		/// Modify to
		/// </summary>
		/// <returns>String</returns>
		public string ModifyTo { get; private set; } // ModifyTo

		/// <summary>
		/// Boundary frame
		/// </summary>
		/// <returns>String</returns>
		public string BoundaryFrame { get; set; } // BoundaryFrame

		/// <summary>
		/// Border x from boundar
		/// </summary>
		/// <returns>Double</returns>
		public double BorderXFromBoundary { get; set; } // BorderXFromBoundar

		/// <summary>
		/// Border y from boundary
		/// </summary>
		/// <returns>Double</returns>
		public double BorderYFromBoundary { get; set; } // BorderYFromBoundary
		#endregion
	}
}
