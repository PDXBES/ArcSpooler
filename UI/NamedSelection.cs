// Project: UI, File: NamedSelection.cs
// Namespace: ArcSpooler.UI, Class: NamedSelection
// Path: D:\Development\ArcSpooler\UI, Author: arnelm
// Code lines: 13, Size of file: 205 Bytes
// Creation date: 12/17/2008 4:12 PM
// Last modified: 12/17/2008 4:14 PM

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace ArcSpooler.UI
{
	public class NamedSelection
	{
		/// <summary>
		/// Create named selection
		/// </summary>
		/// <param name="name">Name</param>
		/// <param name="selection">Selection</param>
		public NamedSelection(string name, List<string> selection)
		{
			Name = name;
			Selection = selection;
		} // NamedSelection(name, selection)

		/// <summary>
		/// Name
		/// </summary>
		/// <returns>String</returns>
		public string Name { get; set; } // Name

		/// <summary>
		/// Selection
		/// </summary>
		/// <returns>List</returns>
		public List<string> Selection { get; private set; } // Selection
	}
}
