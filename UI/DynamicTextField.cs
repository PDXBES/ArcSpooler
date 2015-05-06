// Project: UI, File: DynamicTextField.cs
// Namespace: ArcSpooler.UI, Class: DynamicTextField
// Path: D:\Development\ArcSpooler\UI, Author: arnelm
// Code lines: 15, Size of file: 284 Bytes
// Creation date: 10/31/2008 7:38 PM
// Last modified: 9/2/2010 5:09 PM

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace ArcSpooler.UI
{
	public class DynamicTextField
	{
    private Dictionary<string, string> fieldSpecs = new Dictionary<string, string>();
    private Dictionary<string, string> fieldValues = new Dictionary<string, string>();

		#region Constructors
		/// <summary>
		/// Create dynamic text field
		/// </summary>
		/// <param name="baseName">Base name</param>
		/// <param name="modifyToField">Modify to field</param>
		public DynamicTextField(string baseName, string modifyToField, string replaceFormat)
		{
			BaseName = baseName;
			ModifyToField = modifyToField;
			ReplaceFormat = replaceFormat;
		} // DynamicTextField(baseName, modifyToField)
		#endregion

		#region Properties
		/// <summary>
		/// Base name
		/// </summary>
		/// <returns>String</returns>
		public string BaseName { get; set; } // BaseName

		/// <summary>
		/// Replace format
		/// </summary>
		/// <returns>String</returns>
		public string ReplaceFormat { get; set; } // ReplaceFormat
		/// <summary>
		/// Modify to field
		/// </summary>
		/// <returns>String</returns>
		public string ModifyToField { get; set; } // ModifyToField

		/// <summary>
		/// Replace string
		/// </summary>
		/// <returns>String</returns>
		public string ReplaceString { get; set; } // ReplaceString

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
        object[] formatArgs;
        if (fieldSpecs.Count > 0)
        {
          formatArgs = new object[fieldSpecs.Count];
          List<string> values = new List<string>(fieldValues.Values);
          List<string> types = new List<string>(fieldSpecs.Values);
          for (int i = 0; i < (fieldSpecs.Count); i++)
          {
            switch (types[i])
            {
              case "int":
                formatArgs[i] = Convert.ToInt16(values[i]);
                break;
              case "float" :
              case "double":
                formatArgs[i] = Convert.ToDouble(values[i]);
                break;
              case "string":
                formatArgs[i] = values[i];
                break;
              case "datetime":
                formatArgs[i] = Convert.ToDateTime(values[i]);
                break;
            }
          }
          return string.Format(ReplaceFormat, formatArgs);
        } // if
        else
        {
          return string.Format(ReplaceFormat, ReplaceString);
        } // else
			} // get
		} // ModifyTo

		/// <summary>
		/// Boundary frame
		/// </summary>
		/// <returns>String</returns>
		public string BoundaryFrame { get; set; } // BoundaryFrame

		/// <summary>
		/// Border x from boundary
		/// </summary>
		/// <returns>Double</returns>
		public double BorderXFromBoundary { get; set; } // BorderXFromBoundar

		/// <summary>
		/// Border y from boundary
		/// </summary>
		/// <returns>Double</returns>
		public double BorderYFromBoundary { get; set; } // BorderYFromBoundary

    /// <summary>
    /// Add field spec
    /// </summary>
    /// <param name="fieldName">Field name</param>
    /// <param name="fieldType">Field type</param>
    public void AddFieldSpec(string fieldName, string fieldType)
    {
      fieldSpecs.Add(fieldName, fieldType);
    } // AddFieldSpec(fieldName, fieldType)

    /// <summary>
    /// Clear field specs
    /// </summary>
    public void ClearFieldSpecs()
    {
      fieldSpecs.Clear();
    } // ClearFieldSpecs()

    /// <summary>
    /// Field spec count
    /// </summary>
    /// <returns>Int</returns>
    public int FieldSpecCount
    {
      get
      {
        return fieldSpecs.Count;
      } // get
     } // FieldSpecCount

    /// <summary>
    /// Get field spec field name
    /// </summary>
    /// <param name="i">I</param>
    /// <returns>String</returns>
    public string GetFieldSpecFieldName(int i)
    {
      List<string> fieldNames = new List<string>(fieldSpecs.Keys);
      return fieldNames[i];
    } // GetFieldSpecFieldName()

    /// <summary>
    /// Add field value
    /// </summary>
    /// <param name="fieldName">Field name</param>
    /// <param name="fieldValue">Field value</param>
    public void AddFieldValue(string fieldName, object fieldValue)
    {
      fieldValues.Add(fieldName, fieldValue.ToString());
    } // AddFieldValue(fieldName, fieldValue)

    /// <summary>
    /// Clear field values
    /// </summary>
    public void ClearFieldValues()
    {
      fieldValues.Clear();
    } // ClearFieldValues()
		#endregion
	}
}
