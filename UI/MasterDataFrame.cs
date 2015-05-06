// Project: UI, File: MasterDataFrame.cs
// Namespace: ArcSpooler.UI, Class: MasterDataFrame
// Path: D:\Development\ArcSpooler\UI, Author: arnelm
// Code lines: 13, Size of file: 198 Bytes
// Creation date: 10/31/2008 5:13 PM
// Last modified: 12/2/2008 10:50 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcSpooler.UI
{
	public class MasterDataFrame
	{
		private List<HighlightLayer> _HighlightLayers = new List<HighlightLayer>();

		/// <summary>
		/// Name
		/// </summary>
		/// <returns>String</returns>
		public string Name { get; set; } // Name

		/// <summary>
		/// Layer to shift
		/// </summary>
		/// <returns>String</returns>
		public string LayerToShift { get; set; } // LayerToShift

		/// <summary>
		/// Layer key field
		/// </summary>
		/// <returns>String</returns>
		public string LayerKeyField { get; set; } // LayerKeyField

		/// <summary>
		/// Zoom to object
		/// </summary>
		/// <returns>Bool</returns>
		public bool ZoomToObject { get; set; } // ZoomToObject

		/// <summary>
		/// Rotate map
		/// </summary>
		/// <returns>Bool</returns>
		public bool RotateMap { get; set; } // RotateMap

		/// <summary>
		/// Scale interval
		/// </summary>
		/// <returns>Double</returns>
		public double ScaleInterval { get; set; } // ScaleInterval

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
