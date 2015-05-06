// Project: UI, File: DynamicPictureField.cs
// Namespace: ArcSpooler.UI, Class: DynamicPictureField
// Path: D:\Development\ArcSpooler\UI, Author: arnelm
// Code lines: 18, Size of file: 294 Bytes
// Creation date: 10/31/2008 7:43 PM
// Last modified: 11/1/2008 9:34 PM

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace ArcSpooler.UI
{
	public class DynamicPictureField
	{
		#region Constructors
		/// <summary>
		/// Create dynamic picture field
		/// </summary>
		/// <param name="baseName">Base name</param>
		/// <param name="modifyToField">Modify to field</param>
		public DynamicPictureField(string baseName, string modifyToField)
		{
			BaseName = baseName;
			ModifyToField = modifyToField;
		} // DynamicPictureField(baseName, modifyToField)
		#endregion

		#region Properties
		/// <summary>
		/// Base name
		/// </summary>
		/// <returns>String</returns>
		public string BaseName { get; set; } // BaseName

		/// <summary>
		/// Modify to field
		/// </summary>
		/// <returns>String</returns>
		public string ModifyToField { get; set; } // ModifyToField

		/// <summary>
		/// Slot
		/// </summary>
		/// <returns>Int</returns>
		public int Slot { get; set; } // Slot

		/// <summary>
		/// Name
		/// </summary>
		/// <returns>String</returns>
		public string Name
		{
			get
			{
				return string.Format(BaseName, Slot);
			} // get
		} // Name

		/// <summary>
		/// Modify to
		/// </summary>
		/// <returns>String</returns>
		public string ModifyTo
		{
			get
			{
				return "DynamicPictureField";
			} // get
		} // ModifyTo
		#endregion
	}
}
