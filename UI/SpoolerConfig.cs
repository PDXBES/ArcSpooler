// Project: UI, File: SpoolerConfig.cs
// Namespace: ArcSpooler.UI, Class: SpoolerConfig
// Path: D:\Development\ArcSpooler\UI, Author: arnelm
// Code lines: 31, Size of file: 431 Bytes
// Creation date: 10/31/2008 3:50 PM
// Last modified: 9/2/2010 11:15 AM

#region Using directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using log4net;
#endregion

namespace ArcSpooler.UI
{
  public class SpoolerConfig
  {
    #region Fields
    private XElement _ConfigFile = null;
    private XElement _TemplateNode = null;
    private XElement _SourceDatabaseNode = null;
    private XElement _OutputNode = null;
    private ArcLicenser _ArcLicenser = new ArcLicenser();
    private List<NamedSelection> _NamedSelections = new List<NamedSelection>();

    private static readonly log4net.ILog log = LogManager.GetLogger(
      System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    #region Constructor
    /// <summary>
    /// Create spooler config
    /// </summary>
    /// <param name="filename">Filename</param>
    public SpoolerConfig(string fileName)
    {
      MasterDataFrames = new List<MasterDataFrame>();

      if (log.IsDebugEnabled)
        log.DebugFormat("Loading spooler configuration: {0}", fileName);

      ReadConfig(fileName);

      if (log.IsDebugEnabled)
        log.Debug("Finished loading configuration");

    } // SpoolerConfig(filename)
    #endregion

    #region Properties
    /// <summary>
    /// Config file name
    /// </summary>
    /// <returns>String</returns>
    public string ConfigFileName { get; private set; } // ConfigFileName

    /// <summary>
    /// Template file name
    /// </summary>
    /// <returns>String</returns>
    public string TemplateFileName { get; private set; } // TemplateFileName

    /// <summary>
    /// Template document
    /// </summary>
    /// <returns>IMap document</returns>
    public IMapDocument TemplateDocument { get; private set; } // TemplateDocument

    /// <summary>
    /// Master data frame
    /// </summary>
    /// <returns>Master data frame</returns>
    public MasterDataFrame MasterDataFrame
    {
      get
      {
        return MasterDataFrames.First();
      }
    } // MasterDataFrame

    /// <summary>
    /// Master data frames list
    /// </summary>
    /// <returns>List</returns>
    public List<MasterDataFrame> MasterDataFrames { get; private set; } // MasterDataFrames
    /// <summary>
    /// Data frames
    /// </summary>
    /// <returns>List</returns>
    public List<DataFrame> DataFrames { get; private set; } // DataFrames

    /// <summary>
    /// Text fields
    /// </summary>
    /// <returns>List</returns>
    public List<TextField> TextFields { get; private set; } // TextFields

    /// <summary>
    /// Dynamic text fields
    /// </summary>
    /// <returns>Lst</returns>
    public List<DynamicTextField> DynamicTextFields { get; private set; } // Dynam

    /// <summary>
    /// Tables
    /// </summary>
    /// <returns>List</returns>
    public List<Table> Tables { get; private set; } // Tables

    /// <summary>
    /// Group fields
    /// </summary>
    /// <returns>List</returns>
    public List<Group> Groups { get; private set; } // GroupFields

    /// <summary>
    /// Source connection string
    /// </summary>
    /// <returns>String</returns>
    public string SourceConnectionString { get; private set; } // SourceConnectionString

    /// <summary>
    /// Source table
    /// </summary>
    /// <returns>String</returns>
    public string SourceTable { get; private set; } // SourceTable

    /// <summary>
    /// Source field
    /// </summary>
    /// <returns>String</returns>
    public string SourceField { get; private set; } // SourceField

    /// <summary>
    /// Create MXD
    /// </summary>
    /// <returns>Bool</returns>
    public bool CreateMXD { get; private set; } // CreateMXD

    /// <summary>
    /// Create PDF
    /// </summary>
    /// <returns>Bool</returns>
    public bool CreatePDF { get; private set; } // CreatePDF

    /// <summary>
    /// Create GeoPDF
    /// </summary>
    /// <returns>Bool</returns>
    public bool CreateGeoPDF { get; private set; } // CreateGeoPDF

    /// <summary>
    /// PDF resolution
    /// </summary>
    /// <returns>Int</returns>
    public int PDFResolution { get; private set; } // PDFResolution
    /// <summary>
    /// Output path
    /// </summary>
    /// <returns>String</returns>
    public string OutputPath { get; private set; } // OutputPath

    /// <summary>
    /// Output base name
    /// </summary>
    /// <returns>String</returns>
    public string OutputBasename { get; private set; } // OutputBasename

    /// <summary>
    /// Create job log
    /// </summary>
    /// <returns>Bool</returns>
    public bool CreateJobLog { get; private set; } // CreateJobLog
    /// <summary>
    /// Template document loaded
    /// </summary>
    /// <returns>Bool</returns>
    public bool TemplateDocumentLoaded
    {
      get
      {
        try
        {
          return TemplateDocument.DocumentFilename != string.Empty;
        } // try
        catch
        {
          return false;
        } // catch
      } // get
    } // TemplateDocumentLoaded
    #endregion

    #region Methods

    #region Configuration reading
    /// <summary>
    /// Read config
    /// </summary>
    private void ReadConfig(string fileName)
    {
      _ConfigFile = XElement.Load(fileName);
      ConfigFileName = fileName;

      _TemplateNode = _ConfigFile.Element("template");
      XAttribute templateFile = _TemplateNode.Attribute("filename");
      TemplateFileName = templateFile.Value;
      ReadTemplate();
      ReadSourceDatabase();
      ReadOutput();
      ReadSelection();
    } // ReadConfig()

    /// <summary>
    /// Read template
    /// </summary>
    private void ReadTemplate()
    {
      if (log.IsDebugEnabled)
        log.Debug("Reading template section");

      if (_TemplateNode == null)
        return;

      TemplateDocument = new MapDocumentClass();

      ReadMasterDataFrameConfig();
      ReadDataFrameConfig();
      ReadTextFieldConfig();
      ReadDynamicTextFieldConfig();
      ReadTableConfig();
      ReadGroupConfig();

      if (log.IsDebugEnabled)
        log.Debug("Finished reading template section");
    } // ReadTemplate()

    /// <summary>
    /// Reset template
    /// </summary>
    public void ReloadTemplateFile()
    {
      if (log.IsDebugEnabled)
        log.Debug("Reloading template file");

      try
      {
        if (TemplateDocument.DocumentFilename != string.Empty)
        {
          TemplateDocument.Close();
        }
      }
      catch (Exception e)
      {
        if (log.IsDebugEnabled)
          log.Debug("Exception detected: " + e.Message);
      } // catch (Exception)
      TemplateDocument.Open(TemplateFileName, "");

      if (log.IsDebugEnabled)
        log.Debug("Finished reloading template file");

    } // ResetTemplate()

    /// <summary>
    /// Read source database
    /// </summary>
    private void ReadSourceDatabase()
    {
      if (log.IsDebugEnabled)
        log.Debug("Reading source database section");

      _SourceDatabaseNode = _ConfigFile.Element("sourceDatabase");

      XElement connectionStringNode = _SourceDatabaseNode.Descendants("connectionString").First();
      SourceConnectionString = connectionStringNode.Value;

      XElement sourcetTableNode = _SourceDatabaseNode.Descendants("sourceTable").First();
      SourceTable = sourcetTableNode.Value;

      XElement sourceFieldNode = _SourceDatabaseNode.Descendants("sourceField").First();
      SourceField = sourceFieldNode.Value;

      if (log.IsDebugEnabled)
      {
        log.DebugFormat("Source connection string: {0}", SourceConnectionString);
        log.DebugFormat("Source table: {0}", SourceTable);
        log.DebugFormat("Source field: {0}", SourceField);
        log.Debug("Finished reading source database section");
      } // if
    } // ReadSourceDatabase()

    /// <summary>
    /// Read output
    /// </summary>
    private void ReadOutput()
    {
      if (log.IsDebugEnabled)
        log.Debug("Reading output section");

      _OutputNode = _ConfigFile.Element("output");
      CreateMXD = Convert.ToBoolean(_OutputNode.Attribute("createMXD").Value);
      CreatePDF = Convert.ToBoolean(_OutputNode.Attribute("createPDF").Value);
      CreateGeoPDF = _OutputNode.Attribute("geoPDF") != null ?
        Convert.ToBoolean(_OutputNode.Attribute("geoPDF").Value) : false;
      OutputPath = _OutputNode.Descendants("path").First().Value;
      if (_OutputNode.Descendants("baseName").Count() > 0)
      {
        string outputNodeBaseNameValue = _OutputNode.Descendants("baseName").First().Value;
        OutputBasename = outputNodeBaseNameValue == null ? string.Empty :
          outputNodeBaseNameValue;
      } // if
      else
        OutputBasename = string.Empty;
      PDFResolution = _OutputNode.Attribute("pdfResolution") != null ?
        Convert.ToInt32(_OutputNode.Attribute("pdfResolution").Value) : 300;
      CreateJobLog = _OutputNode.Attribute("createJobLog") != null ?
        Convert.ToBoolean(_OutputNode.Attribute("createJobLog").Value) : false;

      if (log.IsDebugEnabled)
      {
        log.DebugFormat("Create MXD: {0}", CreateMXD);
        log.DebugFormat("Create PDF: {0}", CreatePDF);
        log.DebugFormat("Create GeoPDF: {0}", CreateGeoPDF);
        log.DebugFormat("Output path: {0}", OutputPath);
        log.DebugFormat("PDF resolution: {0}", PDFResolution);
        log.Debug("Finished reading output section");
      } // if
    } // ReadOutput()

    /// <summary>
    /// Read master data frame config
    /// </summary>
    private void ReadMasterDataFrameConfig()
    {
      if (log.IsDebugEnabled)
        log.Debug("Reading master data frame section");

      var masterDataFrames = _TemplateNode.Descendants("masterDataFrame");
      foreach (XElement master in masterDataFrames)
      {
        MasterDataFrame aMasterDataFrame = new MasterDataFrame()
        {
          Name = master.Attribute("name").Value,
          LayerToShift = master.Attribute("layerToShift").Value,
          LayerKeyField = master.Attribute("layerKeyField").Value,
          ZoomToObject = Convert.ToBoolean(master.Attribute("zoomToObject").Value),
          RotateMap = Convert.ToBoolean(master.Attribute("rotateMap").Value),
          ScaleInterval = Convert.ToDouble(master.Attribute("scaleInterval").Value)
        };
        MasterDataFrames.Add(aMasterDataFrame);

        IEnumerable<XElement> highlightLayerNodes =
          master.Descendants("highlightLayer");
        foreach (XElement element in highlightLayerNodes)
        {
          aMasterDataFrame.HighlightLayers.Add(new HighlightLayer(
            Convert.ToBoolean(element.Attribute("hide").Value),
            element.Attribute("layerName").Value,
            element.Attribute("layerField").Value));
        } // foreach  (element)

        if (log.IsDebugEnabled)
        {
          log.DebugFormat("{0} highlight layers read", highlightLayerNodes.Count());
          log.Debug("Finished reading master data frame section");
        }
      }
    } // ReadMasterDataFrameConfig()

    /// <summary>
    /// Read data frame config
    /// </summary>
    private void ReadDataFrameConfig()
    {
      if (log.IsDebugEnabled)
        log.Debug("Reading data frame section");

      IEnumerable<XElement> dataFrameNodes = _TemplateNode.Descendants("dataFrame");
      List<DataFrame> dataFrames = new List<DataFrame>();
      foreach (XElement dataFrameNode in dataFrameNodes)
      {
        DataFrame newDataFrame = new DataFrame()
        {
          Name = dataFrameNode.Attribute("name").Value,
          MatchMasterZoom = Convert.ToBoolean(dataFrameNode.Attribute("matchMasterZoom").Value)
        };
        dataFrames.Add(newDataFrame);

        IEnumerable<XElement> highlightLayerNodes =
          dataFrameNode.Descendants("highlightLayer");
        foreach (XElement element in highlightLayerNodes)
        {
          newDataFrame.HighlightLayers.Add(new HighlightLayer(
            Convert.ToBoolean(element.Attribute("hide").Value),
            element.Attribute("layerName").Value,
            element.Attribute("layerField").Value));
        } // foreach  (element)

      } // foreach  (dataFrameNode)
      DataFrames = dataFrames;

      if (log.IsDebugEnabled)
      {
        log.Debug("Finished reading data frame section");
      }

    } // ReadDataFrameConfig()

    /// <summary>
    /// Read text field config
    /// </summary>
    private void ReadTextFieldConfig()
    {
      if (log.IsDebugEnabled)
        log.Debug("Reading text field section");

      IEnumerable<XElement> textFieldNodes = _TemplateNode.Descendants("textField");
      List<TextField> textFields = new List<TextField>();
      foreach (XElement textFieldNode in textFieldNodes)
      {
        textFields.Add(new TextField(textFieldNode.Attribute("name").Value,
          textFieldNode.Attribute("modifyTo").Value)
          {
            BoundaryFrame = textFieldNode.Attribute("boundaryFrame") != null ?
              textFieldNode.Attribute("boundaryFrame").Value : string.Empty,
            BorderXFromBoundary = textFieldNode.Attribute("borderXFromBoundary") != null ?
              Convert.ToDouble(textFieldNode.Attribute("borderXFromBoundary").Value) : 0,
            BorderYFromBoundary = textFieldNode.Attribute("borderYFromBoundary") != null ?
              Convert.ToDouble(textFieldNode.Attribute("borderYFromBoundary").Value) : 0
          });
      } // foreach  (textFieldNode)
      TextFields = textFields;

      if (log.IsDebugEnabled)
        log.Debug("Finished reading text field section");
    } // ReadTextFieldConfig()

    /// <summary>
    /// Read dynamic text field config
    /// </summary>
    private void ReadDynamicTextFieldConfig()
    {
      if (log.IsDebugEnabled)
        log.Debug("Reading dynamic text field section");

      IEnumerable<XElement> dynamicTextFieldNodes = _TemplateNode.Descendants("dynamicTextField");
      List<DynamicTextField> dynamicTextFields = new List<DynamicTextField>();
      foreach (XElement dynamicTextFieldNode in dynamicTextFieldNodes)
      {
        dynamicTextFields.Add(new DynamicTextField(dynamicTextFieldNode.Attribute("baseName").Value,
          dynamicTextFieldNode.Attribute("modifyToField") != null ?
            dynamicTextFieldNode.Attribute("modifyToField").Value : string.Empty,
          dynamicTextFieldNode.Attribute("replaceFormat").Value)
          {
            BoundaryFrame = dynamicTextFieldNode.Attribute("boundaryFrame") != null ?
              dynamicTextFieldNode.Attribute("boundaryFrame").Value : string.Empty,
            BorderXFromBoundary = dynamicTextFieldNode.Attribute("borderXFromBoundary") != null ?
              Convert.ToDouble(dynamicTextFieldNode.Attribute("borderXFromBoundary").Value) : 0,
            BorderYFromBoundary = dynamicTextFieldNode.Attribute("borderYFromBoundary") != null ?
              Convert.ToDouble(dynamicTextFieldNode.Attribute("borderYFromBoundary").Value) : 0
          });
        DynamicTextField newDynamicTextField = dynamicTextFields.Last();

        IEnumerable<XElement> fieldSpecNodes = dynamicTextFieldNode.Descendants("fieldSpec");
        foreach (XElement fieldSpecNode in fieldSpecNodes)
        {
          newDynamicTextField.AddFieldSpec(fieldSpecNode.Attribute("name").Value,
            fieldSpecNode.Attribute("type").Value);
        } // foreach  (fieldSpecNode)
      } // foreach  (textFieldNode)
      DynamicTextFields = dynamicTextFields;

      if (log.IsDebugEnabled)
        log.Debug("Finished reading dynamic text field section");
    } // ReadDynamicTextFieldConfig()

    /// <summary>
    /// Read table config
    /// </summary>
    private void ReadTableConfig()
    {
      if (log.IsDebugEnabled)
        log.Debug("Reading table section");

      IEnumerable<XElement> tableNodes = _TemplateNode.Descendants("table");
      List<Table> tables = new List<Table>();
      foreach (XElement tableNode in tableNodes)
      {
        tables.Add(new Table(tableNode.Attribute("name").Value,
          tableNode.Attribute("layerName").Value,
          tableNode.Attribute("orderByField").Value));
      } // foreach  (tableNode)
      Tables = tables;

      if (log.IsDebugEnabled)
        log.Debug("Finished reading table section");

    } // ReadTableConfig()

    /// <summary>
    /// Read group config
    /// </summary>
    private void ReadGroupConfig()
    {
      if (log.IsDebugEnabled)
        log.Debug("Reading group section");

      IEnumerable<XElement> groupNodes = _TemplateNode.Descendants("group");
      List<Group> groups = new List<Group>();
      foreach (XElement groupNode in groupNodes)
      {
        XElement sqlNode = groupNode.Descendants("sql").First();
        Group newGroup = new Group(groupNode.Attribute("name").Value, sqlNode.Value,
          groupNode.Attribute("modifyToField").Value);
        groups.Add(newGroup);

        XElement slotNode = groupNode.Descendants("slot").First();
        newGroup.Slot = slotNode.Attribute("field").Value;

        IEnumerable<XElement> dynamicTextNodes = groupNode.Descendants("dynamicText");
        foreach (XElement dynamicTextNode in dynamicTextNodes)
        {
          newGroup.AddDynamicTextField(new DynamicTextField(
            dynamicTextNode.Attribute("baseName").Value,
            dynamicTextNode.Attribute("modifyToField").Value,
            dynamicTextNode.Attribute("replaceFormat").Value));
        } // foreach  (dynamicTextNode)

        IEnumerable<XElement> dynamicPictureNodes = groupNode.Descendants("dynamicPicture");
        foreach (XElement dynamicPictureNode in dynamicPictureNodes)
        {
          newGroup.AddDynamicPictureField(new DynamicPictureField(dynamicPictureNode.Attribute("baseName").Value,
            dynamicPictureNode.Attribute("modifyToField").Value));
        } // foreach  (dynamicPictureNode)
      } // foreach  (groupNode)
      Groups = groups;

      if (log.IsDebugEnabled)
        log.Debug("Finished reading group section");
    } // ReadGroupConfig()

    /// <summary>
    /// Read selection
    /// </summary>
    private void ReadSelection()
    {
      if (log.IsDebugEnabled)
        log.Debug("Reading selection section");

      var selectionNodes = from item in _ConfigFile.Descendants("selection")
                           select item;
      foreach (XElement item in selectionNodes)
      {
        NamedSelection newNamedSelection = new NamedSelection(item.Attribute("name").Value,
          new List<string>(
            from selectionString in item.Descendants("objectID")
            select selectionString.Value));
      } // foreach  (item)

      if (log.IsDebugEnabled)
        log.Debug("Finished reading selection section");
    } // ReadSelection()
    #endregion

    #region Selection set manipulation
    /// <summary>
    /// Selection set
    /// </summary>
    /// <param name="name">Name</param>
    /// <returns>List</returns>
    public List<string> SelectionSet(string name)
    {
      var query = from item in _ConfigFile.Descendants("selection")
                  where item.Attribute("name").Value == name
                  select item;
      if (query.Count() > 0)
        return new List<string>(from item in query.Descendants("objectID") select item.Value);
      else
        return null;
    } // SelectionSet(name)

    /// <summary>
    /// Selection set names
    /// </summary>
    /// <returns>List</returns>
    public List<string> SelectionSetNames
    {
      get
      {
        return new List<string>(from item in _ConfigFile.Descendants("selection")
                                select item.Attribute("name").Value);
      } // get
    } // SelectionSetNames
    /// <summary>
    /// Save selection set
    /// </summary>
    public void SaveSelectionSet(string name, List<string> selectionSet)
    {
      DeleteSelectionSet(name);

      _ConfigFile.Add(new XElement("selection", new XAttribute("name", name)));
      XElement selectionNode = (from item in _ConfigFile.Descendants("selection")
                                where item.Attribute("name").Value == name
                                select item).First();

      selectionNode.RemoveNodes();
      selectionNode.Add(from item in selectionSet select new XElement("objectID", item));
      _ConfigFile.Save(ConfigFileName);
    } // SaveSelectionSet()

    /// <summary>
    /// Delete selection set
    /// </summary>
    /// <param name="name">Name</param>
    public void DeleteSelectionSet(string name)
    {

      var query = from item in _ConfigFile.Descendants("selection")
                  where item.Attribute("name").Value == name
                  select item;
      if (query.Count() > 0)
      {
        List<XElement> selectionSets = query.ToList();
        foreach (XElement item in selectionSets)
          item.Remove();

        _ConfigFile.Save(ConfigFileName);
      }
    } // DeleteSelectionSet(name)
    #endregion

    #endregion
  }
}
