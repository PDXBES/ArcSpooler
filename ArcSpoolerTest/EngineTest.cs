// Project: ArcSpoolerTest, File: EngineTest.cs
// Namespace: ArcSpoolerTest, Class: EngineTest
// Path: D:\Development\ArcSpooler\ArcSpoolerTest, Author: arnelm
// Code lines: 44, Size of file: 818 Bytes
// Creation date: 11/3/2008 9:30 AM
// Last modified: 11/18/2008 9:47 AM

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using ArcSpooler.UI;
#endregion

namespace ArcSpoolerTest
{
	[TestFixture]
	public class EngineTest
	{
		#region Constants
		const string TEST_CONFIG_FILE = @"D:\Development\ArcSpooler\UI\TestSpoolerConfig.xml";
		const string TEST_UNITID = "ADV203";
		const string TEST_OUTPUT_FILE = @"D:\Development\ArcSpooler\TestOutput\" + TEST_UNITID + ".mxd";
		#endregion

		#region Fields
		Engine engine;
		#endregion

		[SetUp]
		public void Setup()
		{
			engine = new Engine(TEST_CONFIG_FILE);
		}

		[TearDown]
		public void Teardown()
		{ }

		/// <summary>
		/// Test data source initialization
		/// </summary>
		[Test]
		public void TestDataSourceInitialization()
		{
			Assert.That(engine.HasDataSource, Is.True);
			Assert.That(engine.NumDataRecords, Is.EqualTo(133));
		} // TestDataSourceInitialization()

		/// <summary>
		/// Test output
		/// </summary>
		[Test]
		public void TestOutput()
		{
			if (File.Exists(TEST_OUTPUT_FILE))
				File.Delete(TEST_OUTPUT_FILE);

			engine.ProduceOutput(TEST_UNITID);
			Assert.That(File.Exists(TEST_OUTPUT_FILE), Is.True);
		} // TestOutput()

		/// <summary>
		/// Test named elements
		/// </summary>
		[Test]
		public void TestNamedElements()
		{
			IEnumerable<string> namedElements = engine.EngineTemplateNamedElements;

			Assert.That(namedElements.Count(), Is.GreaterThan(0));
		} // TestNamedElements()
	}
}
