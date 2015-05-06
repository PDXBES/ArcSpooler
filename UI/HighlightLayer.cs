// Project: UI, File: HighlightLayer.cs
// Namespace: ArcSpooler.UI, Class: HighlightLayer
// Path: D:\Development\ArcSpooler\UI, Author: arnelm
// Code lines: 13, Size of file: 203 Bytes
// Creation date: 12/1/2008 2:49 PM
// Last modified: 12/1/2008 2:56 PM

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace ArcSpooler.UI
{
  public class HighlightLayer
  {
    /// <summary>
    /// Create highlight layer
    /// </summary>
    /// <param name="Hide">Hide</param>
    /// <param name="layerName">Layer name</param>
    /// <param name="layerField">Layer field</param>
    public HighlightLayer(bool hide, string layerName, string layerField)
    {
      Hide = hide;
      LayerName = layerName;
      LayerField = layerField;
    } // HighlightLayer(Hide, layerName, layerField)

    /// <summary>
    /// Hide
    /// </summary>
    /// <returns>Bool</returns>
    public bool Hide { get; set; } // Hide

    /// <summary>
    /// Layer name
    /// </summary>
    /// <returns>String</returns>
    public string LayerName { get; set; } // LayerName

    /// <summary>
    /// Layer field
    /// </summary>
    /// <returns>String</returns>
    public string LayerField { get; set; } // LayerField

    /// <summary>
    /// Value
    /// </summary>
    /// <returns>String</returns>
    public string Value { get; set; } // Value

    /// <summary>
    /// Def query
    /// </summary>
    /// <returns>String</returns>
    public string DefQuery
    {
      get
      {
        string beginQuoteDelim = "[";
        string endQuoteDelim = "]";
        if (Hide)
          return string.Format("{2}{0}{3} NOT LIKE '{1}'", LayerField, Value,
            beginQuoteDelim, endQuoteDelim);
        else
          return string.Format("{2}{0}{3} = '{1}'", LayerField, Value,
            beginQuoteDelim, endQuoteDelim);
      } // get
    } // DefQuery
  }
}
