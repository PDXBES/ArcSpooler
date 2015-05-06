// Project: ArcSpoolerTest, File: MapDocumentHelperTest.cs
// Namespace: ArcSpoolerTest, Class: MapDocumentHelperTest
// Path: D:\Development\ArcSpooler\ArcSpoolerTest, Author: arnelm
// Code lines: 47, Size of file: 1.07 KB
// Creation date: 11/3/2008 10:08 AM
// Last modified: 11/6/2008 4:46 PM

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ArcSpooler.UI;
#endregion

namespace ArcSpoolerTest
{
	[TestFixture]
	public class MapDocumentHelperTest
	{
		#region Constants
		const string TEST_CONFIG_FILE = @"D:\Development\ArcSpooler\UI\TestSpoolerConfig.xml";
		const string TEST_UNITID = "AMY402";
		#endregion

		#region Fields
		private MapDocumentHelper _MdocHelper;
		private ArcLicenser _ArcLicenser;
		private SpoolerConfig _Config;
		#endregion

		[SetUp]
		public void Setup()
		{
			_ArcLicenser = new ArcLicenser();
			_Config = new SpoolerConfig(TEST_CONFIG_FILE);
			_Config.ReloadTemplateFile();
			_MdocHelper = new MapDocumentHelper(_Config.TemplateDocument);
		}

		[TearDown]
		public void Teardown()
		{ }

		/// <summary>
		/// Test map frames
		/// </summary>
		[Test]
		public void TestMapFrames()
		{
			Assert.That(_MdocHelper.NumMapFrames, Is.EqualTo(1));
			Assert.That(_MdocHelper.MapFrame("main map 1"), Is.Not.Null);
		} // TestMapFrames()

		/// <summary>
		/// Test maps
		/// </summary>
		[Test]
		public void TestMaps()
		{
			Assert.That(_MdocHelper.NumMaps, Is.EqualTo(1));
			Assert.That(_MdocHelper.Map("main map 1"), Is.Not.Null);
		} // TestMaps()

		/// <summary>
		/// Test feature layers
		/// </summary>
		[Test]
		public void TestFeatureLayers()
		{
			Assert.That(_MdocHelper.FeatureLayer(_MdocHelper.Map("main map 1"), "UIC Locations to Map"),
				Is.Not.Null);
		} // TestFeatureLayers()

		/// <summary>
		/// Test feature
		/// </summary>
		[Test]
		public void TestFeature()
		{
			IFeatureLayer layer = _MdocHelper.FeatureLayer(_MdocHelper.Map("main map 1"), "UIC Locations to Map");
			Assert.That(_MdocHelper.Feature(layer, "UNITID", TEST_UNITID), Is.Not.Null);
		} // TestFeature()

		/// <summary>
		/// Test shift map frames
		/// </summary>
		[Test]
		public void TestShiftMapFrames()
		{
			IMapFrame mapFrame = _MdocHelper.MapFrame("main map 1");
			IEnvelope originalMapBounds = mapFrame.MapBounds;
			_MdocHelper.ShiftMapFrame(_Config.MasterDataFrame.Name,
				_Config.MasterDataFrame.LayerToShift,
				_Config.MasterDataFrame.LayerKeyField,
				TEST_UNITID, _Config.MasterDataFrame.ZoomToObject,
				_Config.MasterDataFrame.RotateMap,
				_Config.MasterDataFrame.ScaleInterval);
			IEnvelope newMapBounds = mapFrame.MapBounds;

			Assert.That(originalMapBounds.XMax, Is.Not.EqualTo(newMapBounds.XMax));

			_Config.TemplateDocument.SaveAs(@"D:\Development\ArcSpooler\TestOutput\Test.mxd", false, true);
		} // TestShiftMapFrames()

		/// <summary>
		/// Test rectangle elements
		/// </summary>
		[Test]
		public void TestRectangleElements()
		{
			Assert.That(_MdocHelper.RectangleElement("image frame 1"), Is.Not.Null);
		} // TestRectangleElements()

		///// <summary>
		///// Test named elements
		///// </summary>
		//[Test]
		//public void TestNamedElements()
		//{

		//  Assert.That(_MdocHelper.NumNamedElements, Is.GreaterThan(0));
		//} // TestNamedElements()
	}
}
