// Project: UI, File: DataFrame.cs
// Namespace: ArcSpooler.UI, Class: DataFrame
// Path: D:\Development\ArcSpooler\UI, Author: arnelm
// Code lines: 35, Size of file: 712 Bytes
// Creation date: 11/4/2008 9:04 AM
// Last modified: 12/12/2008 1:13 PM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcSpooler.UI
{
	/// <summary>
	/// Slave map data frame services
	/// </summary>
	public class DataFrame
	{
		private List<HighlightLayer> _HighlightLayers = new List<HighlightLayer>();

		/// <summary>
		/// Name
		/// </summary>
		/// <returns>String</returns>
		public string Name { get; set; } // Name

		/// <summary>
		/// Match master zoom
		/// </summary>
		/// <returns>Bool</returns>
		public bool MatchMasterZoom { get; set; } // MatchMasterZoom

		/// <summary>
		/// Highlight layers
		/// </summary>
		/// <returns>List</returns>
		public List<HighlightLayer> HighlightLayers
		{
			get
			{
				return _HighlightLayers;
			}
		} // HighlightLayers
	}
}
