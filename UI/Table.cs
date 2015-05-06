// Project: UI, File: Table.cs
// Namespace: ArcSpooler.UI, Class: Table
// Path: D:\Development\ArcSpooler\UI, Author: arnelm
// Code lines: 15, Size of file: 235 Bytes
// Creation date: 12/2/2008 11:13 AM
// Last modified: 12/16/2008 5:04 PM

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace ArcSpooler.UI
{
	public class Table
	{
		/// <summary>
		/// Create table
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="layerName">Layer name</param>
		public Table(string name, string layerName, string orderByField)
		{
			Name = name;
			LayerName = layerName;
			OrderByField = orderByField;
		} // Table(name, layerName)

		/// <summary>
		/// Name
		/// </summary>
		/// <returns>String</returns>
		public string Name { get; set; } // Name

		/// <summary>
		/// Layer name
		/// </summary>
		/// <returns>String</returns>
		public string LayerName { get; set; } // LayerName

		/// <summary>
		/// Order by field
		/// </summary>
		/// <returns>String</returns>
		public string OrderByField { get; set; } // OrderByField
	}
}
