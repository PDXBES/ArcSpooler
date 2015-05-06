// Project: ArcSpoolerTest, File: SpoolerConfigTest.cs
// Namespace: ArcSpoolerTest, Class: SpoolerConfigTest
// Path: D:\Development\ArcSpooler\ArcSpoolerTest, Author: arnelm
// Code lines: 43, Size of file: 745 Bytes
// Creation date: 10/31/2008 4:43 PM
// Last modified: 12/18/2008 8:19 AM

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using ESRI.ArcGIS.Carto;
using ArcSpooler.UI;
#endregion

namespace ArcSpoolerTest
{
	[TestFixture]
	public class SpoolerConfigTest
	{
		#region Constants
		private const string MASTER_DATA_FRAME_NAME = "main map 1";
		private const string MASTER_DATA_FRAME_LAYER_KEY_FIELD = "SUMPID1";
		private const string MASTER_DATA_FRAME_LAYER_TO_SHIFT = "UIC Map Views";
		private const string DATA_FRAME_NAME = "main map 2";
		const string TEST_CONFIG_FILE = @"D:\Development\ArcSpooler\UI\TestSpoolerConfig.xml";
		const string TEMPLATE_FILE = @"D:\Development\ArcSpooler\Resources\Map_Template.mxd";
		const string SOURCE_DATABASE_CONN_STRING = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
			@"Data Source=\\Cassio\gis2\PROJECTS\8292_UIC_RegionalPlan\GIS\PGDB\UIC_Photos_To_Map.mdb;"+
			@"User Id=admin;Password=;";
		#endregion

		#region Fields
		private SpoolerConfig config;
		#endregion

		[SetUp]
		public void Setup()
		{
			config = new SpoolerConfig(TEST_CONFIG_FILE);
			config.ReloadTemplateFile();
		}

		[TearDown]
		public void Teardown()
		{
		}

		[Test]
		public void TestConfigFileName()
		{
			Assert.That(config.ConfigFileName, Is.EqualTo(TEST_CONFIG_FILE));
		}

		/// <summary>
		/// Test template file name
		/// </summary>
		[Test]
		public void TestTemplateFileName()
		{
			Assert.That(config.TemplateFileName, Is.EqualTo(TEMPLATE_FILE));
		} // TestTemplateFileName()

		/// <summary>
		/// Test source database connection string
		/// </summary>
		[Test]
		public void TestSourceDatabaseConnectionString()
		{
			Assert.That(config.SourceConnectionString, Is.EqualTo(SOURCE_DATABASE_CONN_STRING));
		} // TestSourceDatabaseConnectionString()

		/// <summary>
		/// Test
		/// </summary>
		[Test]
		public void TestValidTemplateDocument()
		{
			Assert.That(config.TemplateDocument, Is.Not.Null);
			Assert.That(config.TemplateDocument.DocumentFilename, 
				Is.EqualTo(config.TemplateFileName).IgnoreCase);
		} // Test()

		/// <summary>
		/// Test master data frame
		/// </summary>
		[Test]
		public void TestMasterDataFrame()
		{
			Assert.That(config.MasterDataFrame.Name, Is.EqualTo(MASTER_DATA_FRAME_NAME));
			Assert.That(config.MasterDataFrame.LayerKeyField, Is.EqualTo(MASTER_DATA_FRAME_LAYER_KEY_FIELD));
			Assert.That(config.MasterDataFrame.LayerToShift, Is.EqualTo(MASTER_DATA_FRAME_LAYER_TO_SHIFT));

			MapDocumentHelper mdocHelper = new MapDocumentHelper(config.TemplateDocument);
			Assert.That(mdocHelper.NumMapFrames, Is.GreaterThan(0));
			Assert.That(mdocHelper.MapFrame(config.MasterDataFrame.Name), Is.Not.Null);
		} // TestMasterDataFrame()

		/// <summary>
		/// Test data frame
		/// </summary>
		[Ignore]
		public void TestDataFrame()
		{
			MapDocumentHelper mdocHelper = new MapDocumentHelper(config.TemplateDocument);
			Assert.That(mdocHelper.MapFrame(config.DataFrames[0].Name), Is.Not.Null);
			Assert.That(config.DataFrames.Count, Is.EqualTo(1));
		} // TestDataFrame()

		/// <summary>
		/// Test text fields
		/// </summary>
		[Test]
		public void TestTextFields()
		{
			MapDocumentHelper mdocHelper = new MapDocumentHelper(config.TemplateDocument);
			Assert.That(config.TextFields.Count, Is.EqualTo(5));

			ITextElement testTextElement = mdocHelper.TextElement("main title");
			string testTextElementText = testTextElement.Text;
			testTextElement.Text = "Blah blah";
			Assert.That(testTextElementText, Is.Not.EqualTo(testTextElement.Text));
		} // TestTextFields()

		/// <summary>
		/// Test groups
		/// </summary>
		[Test]
		public void TestGroups()
		{
			MapDocumentHelper mdocHelper = new MapDocumentHelper(config.TemplateDocument);
			Assert.That(config.Groups.Count, Is.EqualTo(1));

			Assert.That(config.Groups[0].Slot, Is.EqualTo("Photo_Slot"));
			Assert.That(config.Groups[0].NumDynamicTextFields, Is.EqualTo(2));
			Assert.That(config.Groups[0].NumDynamicPictureFields, Is.EqualTo(1));
		} // TestGroups()

		/// <summary>
		/// Test output
		/// </summary>
		[Test]
		public void TestOutput()
		{
			Assert.That(config.CreateMXD, Is.True);
			Assert.That(config.CreatePDF, Is.True);
			Assert.That(config.PDFResolution, Is.EqualTo(300));
			Assert.That(config.OutputPath.Length, Is.GreaterThan(0));
		} // TestOutput()

		/// <summary>
		/// Test selection set save
		/// </summary>
		[Test]
		public void TestSelectionSetSave()
		{
			List<string> selectionSet = new List<string>();
			selectionSet.Add("AAC311");

			config.SaveSelectionSet("TestSet", selectionSet);
			Assert.That(config.SelectionSet("TestSet").Count == 1);
		} // TestSelectionSetSave()

		/// <summary>
		/// Test selection set delete
		/// </summary>
		[Test]
		public void TestSelectionSetDelete()
		{
			List<string> selectionSet = new List<string>();
			selectionSet.Add("AAC311");
			config.SaveSelectionSet("TestSet", selectionSet);
			Assert.That(config.SelectionSet("TestSet").Count == 1);
			config.DeleteSelectionSet("TestSet");
			Assert.That(config.SelectionSet("TestSet") == null);
		} // TestSelectionSetDelete()

	}
}
