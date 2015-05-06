// Project: UI, File: Group.cs
// Namespace: ArcSpooler.UI, Class: Group
// Path: D:\Development\ArcSpooler\UI, Author: arnelm
// Code lines: 20, Size of file: 282 Bytes
// Creation date: 11/1/2008 9:44 PM
// Last modified: 11/3/2008 10:58 PM

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace ArcSpooler.UI
{
	/// <summary>
	/// Group
	/// </summary>
	public class Group
	{
		#region Fields
		private List<DynamicTextField> _DynamicTextFields = new List<DynamicTextField>();
		private List<DynamicPictureField> _DynamicPictureFields = new List<DynamicPictureField>();
		#endregion

		#region Constructors
		/// <summary>
		/// Create group
		/// </summary>
		/// <param name="name">Name</param>
		public Group(string name, string sql, string modifyToField)
		{
			Name = name;
			Sql = sql;
			ModifyToField = modifyToField;
		} // Group(name)
		#endregion

		#region Properties
		/// <summary>
		/// Name
		/// </summary>
		/// <returns>String</returns>
		public string Name { get; private set; } // Name

		/// <summary>
		/// Sql
		/// </summary>
		/// <returns>String</returns>
		public string Sql { get; private set; } // Sql

		/// <summary>
		/// Modify to field
		/// </summary>
		/// <returns>String</returns>
		public string ModifyToField { get; private set; } // ModifyToField

		/// <summary>
		/// Slot field
		/// </summary>
		/// <returns>String</returns>
		public string Slot { get; set; } // Slot

		/// <summary>
		/// Num dynamic text field
		/// </summary>
		/// <returns>Int</returns>
		public int NumDynamicTextFields
		{
			get
			{
				return _DynamicTextFields.Count;
			} // get
		} // NumDynamicTextField

		/// <summary>
		/// Num dynamic picture fields
		/// </summary>
		/// <returns>Int</returns>
		public int NumDynamicPictureFields
		{
			get
			{
				return _DynamicPictureFields.Count;
			} // get
		} // NumDynamicPictureFields
		#endregion

		#region Methods
		/// <summary>
		/// Add dynamic text field
		/// </summary>
		/// <param name="field">Field</param>
		public void AddDynamicTextField(DynamicTextField field)
		{
			_DynamicTextFields.Add(field);
		} // AddDynamicTextField(field)

		/// <summary>
		/// Add dynamic picture field
		/// </summary>
		/// <param name="field">Field</param>
		public void AddDynamicPictureField(DynamicPictureField field)
		{
			_DynamicPictureFields.Add(field);
		} // AddDynamicPictureField(field)

		/// <summary>
		/// Dynamic text field
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>Dynamic text field</returns>
		public DynamicTextField DynamicTextField(int index)
		{
			return _DynamicTextFields[index];
		} // DynamicTextField(index)

		/// <summary>
		/// Dynamic picture field
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>Dynamic picture field</returns>
		public DynamicPictureField DynamicPictureField(int index)
		{
			return _DynamicPictureFields[index];
		} // DynamicPictureField(index)
		#endregion
	}
}
