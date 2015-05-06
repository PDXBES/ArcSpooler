// Project: GridTemplateMapper, File: ArcLicenser.cs
// Namespace: GridTemplateMapper, Class: ArcLicenser
// Path: D:\Development\GridTemplateMapper\GridTemplateMapper, Author: ARNELM
// Code lines: 51, Size of file: 1.28 KB
// Creation date: 9/14/2006 2:15 PM
// Last modified: 9/20/2006 9:45 AM

#region Using directives
using System;
using System.Windows.Forms;
using System.ComponentModel;
#endregion

/// <summary>
/// Grid template mapper
/// </summary>
namespace ArcSpooler.UI
{
	/// <remarks>
	/// The ArcLicenser is used to check out an Arc (ArcView) license.  A license
	/// is retrieved upon creating an ArcLicenser object or by calling the
	/// GetLicense() method.  The license is cleaned up on garbage cleanup.
	/// </remarks>
	public class ArcLicenser
	{
		#region Fields
		/// <summary>
		/// The Arc License checkout initializer
		/// </summary>
		private ESRI.ArcGIS.esriSystem.IAoInitialize ArcLicense;
		#endregion

		#region Methods
		/// <summary>
		/// Creates and retrieves an Arc license (ArcView)
		/// </summary>
		public ArcLicenser()
		{
			GetLicense();
		} // ArcLicenser()

		/// <summary>
		///Destructor for ArcLicenser
		/// </summary>
		~ArcLicenser()
		{
			//ArcLicense.Shutdown();
		} // ~ArcLicenser()

		/// <summary>
		/// Check out Arc license
		/// </summary>
		/// <param name="productCode">Product code</param>
		/// <returns>esriLicenseStatus code returned when checkout occurred</returns>
		public ESRI.ArcGIS.esriSystem.esriLicenseStatus CheckOutArcLicense(
			ESRI.ArcGIS.esriSystem.esriLicenseProductCode productCode)
		{
      ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Desktop);
			ArcLicense = new ESRI.ArcGIS.esriSystem.AoInitializeClass();
			ESRI.ArcGIS.esriSystem.esriLicenseStatus licenseStatus =
				ArcLicense.IsProductCodeAvailable(productCode);
			if (licenseStatus == ESRI.ArcGIS.esriSystem.esriLicenseStatus.esriLicenseAvailable)
				licenseStatus = ArcLicense.Initialize(productCode);
			return licenseStatus;
		} // CheckOutArcLicense(productCode)


		/// <summary>
		/// Retrieves a license from an Arc license server
		/// </summary>
		public void GetLicense()
		{
			ESRI.ArcGIS.esriSystem.esriLicenseStatus licenseStatus =
				CheckOutArcLicense(ESRI.ArcGIS.esriSystem.esriLicenseProductCode.esriLicenseProductCodeStandard);

			if (licenseStatus == ESRI.ArcGIS.esriSystem.esriLicenseStatus.esriLicenseUnavailable)
				MessageBox.Show("Could not check out ArcView license.");
		} // GetLicense()
		#endregion
	} // class ArcLicenser
} // namespace GridTemplateMapper
