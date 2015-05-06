// Project: UI, File: MapDocumentHelper.cs
// Namespace: ArcSpooler.UI, Class: MapDocumentHelper
// Path: D:\Development\ArcSpooler\UI, Author: arnelm
// Code lines: 16, Size of file: 288 Bytes
// Creation date: 10/31/2008 6:11 PM
// Last modified: 9/2/2010 4:22 PM

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.EditorExt;
using ESRI.ArcGIS.GeoDatabaseUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using System.Runtime.InteropServices;
using System.Net;
using log4net;
#endregion

namespace ArcSpooler.UI
{
  /// <summary>
  /// Map document helper
  /// </summary>
  public class MapDocumentHelper
  {
    #region Constants
    private const double TABLE_ROW_SPACING_INCHES = 0.20;
    private const double TABLE_MARGIN_INCHES = 0.10;
    #endregion

    #region Fields
    private static readonly log4net.ILog log = LogManager.GetLogger(
      System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    #region Properties
    /// <summary>
    /// Map document
    /// </summary>
    /// <returns>IMap document</returns>
    public IMapDocument MapDocument { get; private set; } // MapDocument
    #endregion

    #region Constructors
    /// <summary>
    /// Create map document helper
    /// </summary>
    /// <param name="mapDocument">Map document</param>
    public MapDocumentHelper(IMapDocument mapDocument)
    {
      if (log.IsDebugEnabled)
        log.DebugFormat("Initialized MapDocumentHelper to {0}",
          mapDocument.DocumentFilename);

      MapDocument = mapDocument;
    } // MapDocumentHelper(mapDocument)
    #endregion

    #region Properties
    /// <summary>
    /// Map frames
    /// </summary>
    /// <returns>List</returns>
    public IEnumerable<IMapFrame> MapFrames
    {
      get
      {
        IGraphicsContainer layout = MapDocument.PageLayout as IGraphicsContainer;
        layout.Reset();
        IElement currentElement = null;
        while ((currentElement = layout.Next()) != null)
        {
          if (currentElement is IMapFrame)
            yield return currentElement as IMapFrame;
        } // while
      } // get
    } // MapFrames

    /// <summary>
    /// Num map frames
    /// </summary>
    /// <returns>Int</returns>
    public int NumMapFrames
    {
      get
      {
        return MapFrames.Count();
      } // get
    } // NumMapFrames

    /// <summary>
    /// Maps
    /// </summary>
    /// <returns>IEnumerable</returns>
    public IEnumerable<IMap> Maps
    {
      get
      {
        for (int i = 0; i < MapDocument.MapCount; i++)
        {
          yield return MapDocument.get_Map(i);
        } // for
      } // get
    } // Maps

    /// <summary>
    /// Num maps
    /// </summary>
    /// <returns>Int</returns>
    public int NumMaps
    {
      get
      {
        return MapDocument.MapCount;
      } // get
    } // NumMaps

    /// <summary>
    /// Text elements
    /// </summary>
    /// <returns>IEnumerable</returns>
    public IEnumerable<ITextElement> TextElements
    {
      get
      {
        IGraphicsContainer layout = MapDocument.PageLayout as IGraphicsContainer;
        layout.Reset();
        IElement currentElement = null;
        while ((currentElement = layout.Next()) != null)
        {
          if (currentElement is ITextElement)
            yield return currentElement as ITextElement;
        } // while
      } // get
    } // TextElements

    /// <summary>
    /// Num text elements
    /// </summary>
    /// <returns>Int</returns>
    public int NumTextElements
    {
      get
      {
        return TextElements.Count();
      } // get
    } // NumTextElements

    /// <summary>
    /// Rectangle elements
    /// </summary>
    /// <returns>IEnumerable</returns>
    public IEnumerable<IRectangleElement> RectangleElements
    {
      get
      {
        IGraphicsContainer layout = MapDocument.PageLayout as IGraphicsContainer;
        layout.Reset();
        IElement currentElement = null;
        while ((currentElement = layout.Next()) != null)
        {
          if (currentElement is IRectangleElement)
            yield return currentElement as IRectangleElement;
        } // while
      } // get
    } // RectangleElements

    /// <summary>
    /// Num rectangle elements
    /// </summary>
    /// <returns>Int</returns>
    public int NumRectangleElements
    {
      get
      {
        return RectangleElements.Count();
      } // get
    } // NumRectangleElements

    /// <summary>
    /// Named elements
    /// </summary>
    /// <returns>IEnumerable</returns>
    public IEnumerable<IElement> NamedElements
    {
      get
      {
        IGraphicsContainer layout = MapDocument.PageLayout as IGraphicsContainer;
        layout.Reset();
        IElement currentElement = null;
        while ((currentElement = layout.Next()) != null)
        {
          if ((currentElement as IElementProperties).Name != string.Empty)
            yield return currentElement;
        } // while
      } // get
    } // NamedElements

    /// <summary>
    /// Num named elements
    /// </summary>
    /// <returns>Int</returns>
    public int NumNamedElements
    {
      get
      {
        return NamedElements.Count();
      } // get
    } // NumNamedElements
    #endregion

    #region Methods
    /// <summary>
    /// Map frame
    /// </summary>
    /// <param name="mapFrameName">Map frame name</param>
    /// <returns>IMap frame</returns>
    public IMapFrame MapFrame(string mapFrameName)
    {
      var mapFrame = from item in MapFrames
                     where (item as IElementProperties).Name == mapFrameName
                     select item;
      return mapFrame.First();
    } // MapFrame(mapFrameName)

    /// <summary>
    /// Map
    /// </summary>
    /// <param name="mapName">Map name</param>
    /// <returns>IMap</returns>
    public IMap Map(string mapName)
    {
      var map = from item in Maps
                where item.Name == mapName
                select item;
      return map.First();
    } // Map(mapName)

    /// <summary>
    /// Text element
    /// </summary>
    /// <param name="name">Name</param>
    /// <returns>IText element</returns>
    public ITextElement TextElement(string textElementName)
    {
      var textElement = from item in TextElements
                        where (item as IElementProperties).Name == textElementName
                        select item;
      return textElement.First();
    } // TextElement(textElementName)

    /// <summary>
    /// Rectangle element
    /// </summary>
    /// <param name="rectangleElementName">Rectangle element name</param>
    /// <returns>IRectangle element</returns>
    public IRectangleElement RectangleElement(string rectangleElementName)
    {
      var rectangleElement = from item in RectangleElements
                             where (item as IElementProperties).Name == rectangleElementName
                             select item;
      return rectangleElement.First();
    } // RectangleElement(rectangleElementName)

    /// <summary>
    /// Layers
    /// </summary>
    /// <returns>IEnumerable</returns>
    public IEnumerable<IFeatureLayer> FeatureLayers(IMap map)
    {
      for (int i = 0; i < map.LayerCount; i++)
      {
        ILayer aLayer = map.get_Layer(i);
        if (aLayer is IFeatureLayer)
          yield return map.get_Layer(i) as IFeatureLayer;
      } // for
    } // FeatureLayers(map)

    /// <summary>
    /// Feature layer
    /// </summary>
    /// <param name="map">Map</param>
    /// <param name="layerName">Layer name</param>
    /// <returns>IFeature layer</returns>
    public IFeatureLayer FeatureLayer(IMap map, string layerName)
    {
      var featureLayer = from item in FeatureLayers(map)
                         where (item.Name == layerName)
                         select item;
      return featureLayer.First();
    } // FeatureLayer(map, layerName)

    /// <summary>
    /// Features
    /// </summary>
    /// <param name="layer">Layer</param>
    /// <returns>IEnumerable</returns>
    public IEnumerable<IFeature> Features(IFeatureLayer layer)
    {
      IFeatureCursor fc = layer.FeatureClass.Search(null, false);
      IFeature currentFeature;
      while ((currentFeature = fc.NextFeature()) != null)
      {
        yield return currentFeature;
      } // while
    } // Features(layer)

    /// <summary>
    /// Feature
    /// </summary>
    /// <param name="layer">Layer</param>
    /// <param name="key">Key</param>
    /// <returns>IFeature</returns>
    public IFeature Feature(IFeatureLayer layer, string keyFieldName, string key)
    {
      int keyFieldIndex = layer.FeatureClass.FindField(keyFieldName);
      var feature = from item in Features(layer)
                    where item.get_Value(keyFieldIndex).ToString() == key
                    select item;
      if (feature.Count() == 0)
      {
        log.DebugFormat("Feature {1}={2}, Layer {0}, does not exist", layer.Name, keyFieldName, key);
        return null;
      } // if
      else
        return feature.First();
    } // Feature(layer, keyFieldName, key)

    /// <summary>
    /// Fields
    /// </summary>
    /// <param name="Features">Features</param>
    /// <returns>IEnumerable</returns>
    public IEnumerable<IField> Fields(IFeatureClass featureClass)
    {
      IFields fields = featureClass.Fields;
      for (int i = 0; i < fields.FieldCount; i++)
      {
        yield return fields.get_Field(i);
      } // for
    } // Fields(featureClass)

    /// <summary>
    /// Field
    /// </summary>
    /// <param name="feature">Feature</param>
    /// <param name="fieldName">Field name</param>
    /// <returns>IField</returns>
    public IField Field(IFeatureClass featureClass, string fieldName)
    {
      var field = from item in Fields(featureClass)
                  where item.Name == fieldName
                  select item;
      if (field.Count() == 0)
      {
        log.DebugFormat("Field {1} for featureClass {0} does not exist",
          featureClass.AliasName, fieldName);
        return null;
      } // if
      else
        return field.First();
    } // Field(featureClass, fieldName)

    /// <summary>
    /// Shift a master map frame
    /// </summary>
    /// <param name="layerToShift">Layer to shift</param>
    /// <param name="layerKeyField">Layer key field</param>
    /// <param name="key">Key</param>
    public void ShiftMapFrame(string masterMapFrame, string layerToShift,
      string layerKeyField, string key, bool zoomToObject, bool rotateMap,
      double scaleInterval)
    {
      if (log.IsDebugEnabled)
        log.Debug(
          string.Format("Shifting master map frame {0} {1}:{2} {3} zoom={4}, rotateMap={5}, scaleInterval={6}",
            masterMapFrame, layerToShift, layerKeyField, key, zoomToObject, rotateMap,
            scaleInterval));

      IMapFrame mapFrame = MapFrame(masterMapFrame);
      IEnvelope mapBounds;
      IEnvelope mapFrameBounds = (mapFrame as IElement).Geometry.Envelope;
      IFeature selectedFeature = Feature(FeatureLayer(mapFrame.Map, layerToShift),
        layerKeyField, key);
      if (selectedFeature == null)
      {
        string exceptionMessage = string.Format("Object {0} for field {1}, layer {2} doesn't exist", key,
                  layerKeyField, layerToShift);

        if (log.IsDebugEnabled)
          log.Debug(exceptionMessage);
        throw new ArgumentException(exceptionMessage);
      } // if

      if (log.IsDebugEnabled)
        log.Debug("  selection successful");

      IPoint newCenter = new PointClass()
      {
        X = (selectedFeature.Extent.XMax + selectedFeature.Extent.XMin) / 2,
        Y = (selectedFeature.Extent.YMax + selectedFeature.Extent.YMin) / 2
      };

      bool featureForcesRotate = false;
      if (rotateMap)
      {
        if (log.IsDebugEnabled)
          log.Debug("  rotating map");

        double featureAspectRatio = selectedFeature.Extent.Height / selectedFeature.Extent.Width;
        double mapAspectRatio = mapFrameBounds.Height / mapFrameBounds.Width;
        if (mapAspectRatio < 1) // wider than taller
          featureForcesRotate = featureAspectRatio > (mapAspectRatio * 5 / 4);
        else
          featureForcesRotate = featureAspectRatio < (mapAspectRatio * 4 / 5);
      } // if

      IActiveView masterMapView = mapFrame.Map as IActiveView;

      if (zoomToObject)
      {
        if (log.IsDebugEnabled)
          log.Debug("  zooming object");

        double mapFrameWidth = mapFrameBounds.Width;
        double mapFrameHeight = mapFrameBounds.Height;
        double mapAspectRatio = mapFrameBounds.Height / mapFrameBounds.Width;
        double mapUnitsPerMapFrameUnit;
        mapBounds = selectedFeature.Extent;

        double selectedAspectRatio = mapBounds.Height / mapBounds.Width;
        if (mapAspectRatio < selectedAspectRatio) // frame wider aspect than selection
          mapUnitsPerMapFrameUnit = mapBounds.Height / mapFrameHeight;
        else
          mapUnitsPerMapFrameUnit = mapBounds.Width / mapFrameWidth;

        double newMapUnitsPerMapFrameUnit = Math.Ceiling(mapUnitsPerMapFrameUnit / scaleInterval) * scaleInterval;
        if (mapAspectRatio < selectedAspectRatio)
          mapBounds.Expand(0,
            (newMapUnitsPerMapFrameUnit * mapFrameHeight - mapUnitsPerMapFrameUnit * mapFrameHeight) / 2, false);
        else
          mapBounds.Expand((newMapUnitsPerMapFrameUnit * mapFrameWidth - mapUnitsPerMapFrameUnit * mapFrameWidth) / 2, 0, false);

        masterMapView.Extent = mapBounds;
        masterMapView.Refresh();

      } // if
      else
      {
        mapBounds = mapFrame.MapBounds;
        mapBounds.CenterAt(newCenter);
        masterMapView.Extent = mapBounds;
      } // else

      if (featureForcesRotate)
      {
        masterMapView.ScreenDisplay.DisplayTransformation.Rotation = -90;
        mapBounds = selectedFeature.Extent;
        mapBounds.Expand(1.1, 1.1, true);
      } // if
      masterMapView.Refresh();

      if (log.IsDebugEnabled)
        log.Debug("End shift master map frame");
    } // ShiftMapFrame(masterMapFrame, layerToShift, layerKeyField)

    /// <summary>
    /// Shift a slave map frame using a master map frame
    /// </summary>
    /// <param name="masterMapFrame">Master map frame</param>
    /// <param name="slaveMapFrame">Slave map frame</param>
    public void ShiftMapFrame(string masterMapFrame, string slaveMapFrame,
      bool zoomToObject, bool rotateMap)
    {
      if (log.IsDebugEnabled)
        log.Debug(String.Format("Shifting slave map frame {0}:{1} zoom={1}, rotateMap={2}",
          masterMapFrame, slaveMapFrame, zoomToObject, rotateMap));

      IMapFrame masterFrame = MapFrame(masterMapFrame);
      IEnvelope masterBounds = masterFrame.MapBounds;
      IMapFrame slaveFrame = MapFrame(slaveMapFrame);
      IEnvelope slaveBounds = slaveFrame.MapBounds;

      if (rotateMap)
      {
        (slaveFrame.Map as IActiveView).ScreenDisplay.DisplayTransformation.Rotation =
          (masterFrame.Map as IActiveView).ScreenDisplay.DisplayTransformation.Rotation;
      } // if

      if (zoomToObject)
      {
        slaveBounds = masterBounds;
      } // if
      else
      {
        IPoint newCenter = new PointClass()
        {
          X = (masterBounds.XMax + masterBounds.XMin) / 2,
          Y = (masterBounds.YMax + masterBounds.YMin) / 2
        };

        slaveBounds.CenterAt(newCenter);
      } // else

      IActiveView slaveMapView = slaveFrame.Map as IActiveView;
      slaveMapView.Extent = slaveBounds;
      slaveMapView.Refresh();
    } // ShiftMapFrame(masterMapFrame, slaveMapFrame, zoomToObject)

    /// <summary>
    /// Change definition query
    /// </summary>
    /// <param name="layerName">Layer name</param>
    /// <param name="defQuery">Def query</param>
    public void ChangeDefinitionQuery(IMap map, string layerName, string defQuery)
    {
      if (log.IsDebugEnabled)
        log.Debug(string.Format("Changing definition query {0} -> {1}",
          layerName, defQuery));

      IFeatureLayer layer = FeatureLayer(map, layerName);
      IFeatureLayerDefinition layerDef = layer as IFeatureLayerDefinition;

      bool isFileBased = !layer.DataSourceType.Equals("Personal Geodatabase Feature Class");
      string finalDefQuery = defQuery;

      if (isFileBased)
      {
        finalDefQuery = finalDefQuery.Replace('[', '\"');
        finalDefQuery = finalDefQuery.Replace(']', '\"');
      }

      layerDef.DefinitionExpression = finalDefQuery;
    } // ChangeDefinitionQuery(map, layerName, defQuery)

    /// <summary>
    /// Add table to layout
    /// </summary>
    /// <param name="map">Map</param>
    /// <param name="layerName">Layer name</param>
    public void AddTableToLayout(IMap map, string layerName, string boundingElementName,
      string orderByField)
    {
      if (log.IsDebugEnabled)
        log.Debug(string.Format("Adding table to layout {0}, boundingElementName={1}, orderBy={2}",
          layerName, boundingElementName, orderByField));

      IPageLayout pageLayout = MapDocument.PageLayout;
      IGraphicsContainer pageContainer = pageLayout as IGraphicsContainer;
      IElement boundingElementRect = RectangleElement(boundingElementName) as IElement;
      IEnvelope boundingElementEnvelope = boundingElementRect.Geometry.Envelope;
      boundingElementEnvelope.Expand(-TABLE_MARGIN_INCHES, -TABLE_MARGIN_INCHES, false);
      IFeatureLayer featureLayer = (from item in FeatureLayers(map)
                                    where item.Name == layerName
                                    select item).First();
      ITable featureTable = featureLayer as ITable;
      IFields featureFields = featureTable.Fields;
      ILayerFields layerFields = featureLayer as ILayerFields;
      string text = string.Empty;

      IQueryFilter qf = new QueryFilterClass();
      qf.WhereClause = orderByField + " = " + orderByField;
      IQueryFilterDefinition qfdef = qf as IQueryFilterDefinition;
      qfdef.PostfixClause = "ORDER BY " + orderByField;

      ICursor cursor = featureTable.Search(qf, false);
      IRow row;
      double currentRowCoord = boundingElementEnvelope.YMax;
      double currentColCoord = boundingElementEnvelope.XMin;

      int numCols = 0;
      for (int i = 0; i < layerFields.FieldCount; i++)
      {
        IFieldInfo fieldInfo = layerFields.get_FieldInfo(i);
        if (fieldInfo.Visible)
          numCols++;
      } // for

      for (int i = 0; i < layerFields.FieldCount; i++)
      {
        IFieldInfo fieldInfo = layerFields.get_FieldInfo(i);
        if (fieldInfo.Visible)
        {
          text = "<BOL>" + fieldInfo.Alias + "</BOL>";
          AddText(pageLayout, pageContainer, boundingElementEnvelope, text,
            ref currentRowCoord, ref currentColCoord, numCols);
        } // if
      } // for
      ILineElement lineElement = new LineElementClass();
      ILineSymbol lineSymbol = new SimpleLineSymbolClass();
      lineSymbol.Color = new CmykColorClass()
      {
        Cyan = 0,
        Magenta = 0,
        Yellow = 0,
        Black = 100
      };
      lineSymbol.Width = 2;
      lineElement.Symbol = lineSymbol;
      IPolyline polyline = new PolylineClass();
      polyline.FromPoint = new PointClass()
      {
        X = boundingElementEnvelope.XMin,
        Y = currentRowCoord - TABLE_ROW_SPACING_INCHES * 0.9
      };
      polyline.ToPoint = new PointClass()
      {
        X = boundingElementEnvelope.XMax,
        Y = currentRowCoord - TABLE_ROW_SPACING_INCHES * 0.9
      };
      IElement element = lineElement as IElement;
      element.Geometry = polyline;
      pageContainer.AddElement(element, 0);
      NextRowText(ref currentRowCoord, ref currentColCoord, boundingElementEnvelope.XMin);


      while ((row = cursor.NextRow()) != null)
      {
        text = string.Empty;
        for (int i = 0; i < row.Fields.FieldCount; i++)
        {
          IFieldInfo fieldInfo = layerFields.get_FieldInfo(i);
          if (fieldInfo.Visible)
          {
            text = row.get_Value(i).ToString();
            AddText(pageLayout, pageContainer, boundingElementEnvelope, text,
              ref currentRowCoord, ref currentColCoord, numCols);
          }
        }
        NextRowText(ref currentRowCoord, ref currentColCoord, boundingElementEnvelope.XMin);
      }
    }	// AddTableToLayout(map, layerName)

    /// <summary>
    /// Add text
    /// </summary>
    /// <param name="pageLayout">Page layout</param>
    /// <param name="pageContainer">Page container</param>
    /// <param name="boundingElementEnvelope">Bounding element envelope</param>
    /// <param name="text">Text</param>
    /// <param name="currentRowCoord">Current row coord</param>
    /// <param name="currentColCoord">Current col coord</param>
    private void AddText(IPageLayout pageLayout,
      IGraphicsContainer pageContainer,
      IEnvelope boundingElementEnvelope,
      string text,
      ref double currentRowCoord,
      ref double currentColCoord,
      int numCols)
    {
      if (log.IsDebugEnabled)
        log.Debug(string.Format("Adding text {0}", text));

      ITextElement textElement = new TextElementClass();
      ITextSymbol textSymbol = new TextSymbolClass()
      {
        Size = 10,
        VerticalAlignment = esriTextVerticalAlignment.esriTVATop,
        HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
      };
      textElement.Symbol = textSymbol;
      textElement.Text = text;
      IPoint point = new PointClass();
      point.PutCoords(currentColCoord, currentRowCoord);
      IElement element = textElement as IElement;
      element.Geometry = point;
      pageContainer.AddElement(element, 0);
      (pageLayout as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGraphics,
        null, null);
      currentColCoord = currentColCoord + boundingElementEnvelope.Width / numCols;
    } // AddText(pageLayout, pageContainer, boundingElementEnvelope)

    /// <summary>
    /// Next row text
    /// </summary>
    /// <param name="currentRowCoord">Current row coord</param>
    /// <param name="currentColCoord">Current col coord</param>
    private void NextRowText(ref double currentRowCoord, ref double currentColCoord,
      double leftColCoord)
    {
      currentRowCoord = currentRowCoord - TABLE_ROW_SPACING_INCHES;
      currentColCoord = leftColCoord;
    } // NextRowText(currentRowCoord, currentColCoord)
    #endregion
  }
}
