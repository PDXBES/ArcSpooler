// Project: UI, File: Main.cs
// Namespace: ArcSpooler.UI, Class: Main
// Path: D:\Development\ArcSpooler\UI, Author: arnelm
// Code lines: 31, Size of file: 616 Bytes
// Creation date: 10/30/2008 11:26 AM
// Last modified: 9/2/2010 9:58 AM

#region Using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Reflection;
using Infragistics.Win.AppStyling;
using log4net;
#endregion

namespace ArcSpooler.UI
{
	public partial class Main : Form
	{
		#region Fields
		private Engine _PreEngine;
		private Engine _ProcessingEngine; // This is the one used for the background thread
		private System.Threading.Thread splashThread;

		private static readonly log4net.ILog log = LogManager.GetLogger(
			System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		#endregion

		#region Constructors
		public Main()
		{
			splashThread = new System.Threading.Thread(new System.Threading.ThreadStart(DoSplash));
			splashThread.Start();

			InitializeComponent();

			System.IO.Stream appStylingStream = (Assembly.GetExecutingAssembly()).
				GetManifestResourceStream("ArcSpooler.UI.Resources.ArcSpooler.isl");
			if (appStylingStream != null)
				StyleManager.Load(appStylingStream);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Do splash
		/// </summary>
		private static void DoSplash()
		{
			DoSplash(false);
		} // DoSplash()

		/// <summary>
		/// Do splash
		/// </summary>
		/// <param name="waitForClick">Wait for click</param>
		private static void DoSplash(bool waitForClick)
		{
			string versionText = "x.x.x.x";
			string dateText = "1/1/1900";
			if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
			{
				Version v = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
				versionText = v.Major + "." + v.Minor + "." + v.Build + "." + v.Revision;
				dateText = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("MMMM dd yyyy");
			} // if
			else
			{
				string version = System.Reflection.Assembly.GetExecutingAssembly().GetName(false).Version.ToString();
				int minorVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName(false).Version.Minor;
				int majorVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName(false).Version.Major;
				int build = System.Reflection.Assembly.GetExecutingAssembly().GetName(false).Version.Build;
				int revision = System.Reflection.Assembly.GetExecutingAssembly().GetName(false).Version.Revision;
				versionText = majorVersion + "." + minorVersion + "." + build + "." + revision;

				System.IO.FileInfo fi = new System.IO.FileInfo("PWMPDataSystem.exe");
				string date = fi.CreationTime.Date.ToString("MMMM dd yyyy");
				dateText = date;
			} // else

			SplashScreen sp = new SplashScreen(versionText, dateText);
			sp.ShowDialog(waitForClick);
		} // DoSplash(waitForClick)

		/// <summary>
		/// Enable config UI
		/// </summary>
		/// <param name="enabled">Enabled</param>
		private void EnableConfigUI(bool enabled)
		{
			edtConfigFile.Enabled = enabled;
			lstItems.Enabled = enabled;
			pnlControlItemSelection.Enabled = enabled;
			btnRun.Enabled = enabled;
			btnCancel.Enabled = !enabled;
			cmbSelectionSet.Enabled = enabled;
		} // EnableConfigUI(enabled)

		/// <summary>
		/// Update selection set
		/// </summary>
		private void UpdateSelectionSet(string selectionSetName)
		{

			List<string> selectionSet;
			if (selectionSetName.Length == 0)
				selectionSet = _PreEngine.SourceKeys;
			else
			{
				_PreEngine.SetSelection(selectionSetName);
				selectionSet = _PreEngine.ProcessKeys;
			}

			lstItems.Items.Clear();
			foreach (string item in _PreEngine.SourceKeys)
				lstItems.Items.Add(new Infragistics.Win.UltraWinListView.UltraListViewItem(item,
					new string[] { item })
					{
						CheckState = selectionSet.Contains(item) ?
							CheckState.Checked : CheckState.Unchecked
					});
		} // UpdateSelectionSet()

		private List<string> SelectionList()
		{
			List<string> selectionSet = new List<string>();
			foreach (Infragistics.Win.UltraWinListView.UltraListViewItem item in lstItems.Items)
			{
				if (item.CheckState == CheckState.Checked)
					selectionSet.Add(item.Value.ToString());
			} // foreach  (item)
			return selectionSet;
		}

		private void UpdateSelectionSetComboList()
		{
			cmbSelectionSet.Items.Clear();
			foreach (string item in _PreEngine.SelectionSetNames)
				cmbSelectionSet.Items.Add(item, item);
		}

		#endregion

		//
		// Events
		//

		private void edtConfigFile_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
			dlgOpen.Filter = "ArcSpooler config files (*.xml)|*.xml|All files (*.*)|*.*";
			dlgOpen.FilterIndex = 0;
			dlgOpen.Title = "Open ArcSpooler configuration file";
			if (dlgOpen.ShowDialog() == DialogResult.OK)
			{
				edtConfigFile.Text = dlgOpen.FileName;
				lstItems.Items.Clear();
				_PreEngine = new Engine(dlgOpen.FileName);
				_PreEngine.SourceKeys.Sort();

				UpdateSelectionSetComboList();
				cmbSelectionSet.SelectedIndex = 0;

				if (cmbSelectionSet.SelectedItem == null)
				{
					UpdateSelectionSet("");
				} // if
				else
				{
					UpdateSelectionSet(cmbSelectionSet.SelectedItem.DataValue.ToString());
				}
				UpdateNumSelectedLabel();
			} // if
		}

		private void btnRun_Click(object sender, EventArgs e)
		{
			List<string> processItems = new List<string>();
			foreach (Infragistics.Win.UltraWinListView.UltraListViewItem item in lstItems.Items)
				if (item.CheckState == CheckState.Checked)
					processItems.Add(item.Value.ToString());

			_PreEngine.ProcessKeys.Clear();
			_PreEngine.ProcessKeys.AddRange(processItems);

			prgRun.Minimum = 0;
			prgRun.Maximum = processItems.Count;
			EnableConfigUI(false);
			bkgwRunEngine.RunWorkerAsync();
			//_Engine.Run(null);
		}

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			foreach (Infragistics.Win.UltraWinListView.UltraListViewItem item in lstItems.Items)
				item.CheckState = CheckState.Checked;
			UpdateNumSelectedLabel();
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			foreach (Infragistics.Win.UltraWinListView.UltraListViewItem item in lstItems.Items)
				item.CheckState = CheckState.Unchecked;
			UpdateNumSelectedLabel();
		}

		private void btnToggleSelection_Click(object sender, EventArgs e)
		{
			foreach (Infragistics.Win.UltraWinListView.UltraListViewItem item in lstItems.Items)
				item.CheckState = (item.CheckState == CheckState.Checked) ?
					CheckState.Unchecked : CheckState.Checked;
			UpdateNumSelectedLabel();
		}

		private void bkgwRunEngine_DoWork(object sender, DoWorkEventArgs e)
		{
			_ProcessingEngine = new Engine(dlgOpen.FileName);
			_ProcessingEngine.ProcessKeys.Clear();
			_ProcessingEngine.ProcessKeys.AddRange(_PreEngine.ProcessKeys);
			_ProcessingEngine.Run(bkgwRunEngine);
			if (bkgwRunEngine.CancellationPending)
			{
				e.Cancel = true;
				e.Result = false;
				return;
			}
			e.Result = true;
		}

		private void bkgwRunEngine_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			prgRun.Value = e.ProgressPercentage;
		}

		private void bkgwRunEngine_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			prgRun.Value = 0;
			EnableConfigUI(true);

			if (_ProcessingEngine.ErrorKeys.Count > 0)
			{
				StringBuilder errorMessage = new StringBuilder();
				errorMessage.AppendLine("The following objects were not spooled due to an error:");
				foreach (KeyValuePair<string, string> item in _ProcessingEngine.ErrorKeys)
				{
					errorMessage.AppendLine(string.Format("{0}:{1}", item.Key, item.Value));
				} // foreach  (item)
				errorMessage.AppendLine("These keys have been added as a new selection list named ErrorPending");

        List<string> keys = new List<string>(_ProcessingEngine.ErrorKeys.Keys);
				_PreEngine.SaveSelection("ErrorPending", keys);
				UpdateSelectionSetComboList();
				cmbSelectionSet.SelectedIndex = cmbSelectionSet.Items.IndexOf("ErrorPending");

				MessageBox.Show(errorMessage.ToString(), "Errors in production", MessageBoxButtons.OK, 
					MessageBoxIcon.Warning);
			} // if
		}

		private void Main_Load(object sender, EventArgs e)
		{
			EnableConfigUI(true);
			lstItems.Items.Clear();
			if (log.IsDebugEnabled)
				log.Debug("UI initialized");
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			bkgwRunEngine.CancelAsync();
		}

		private void lstItems_ItemCheckStateChanged(object sender, Infragistics.Win.UltraWinListView.ItemCheckStateChangedEventArgs e)
		{
			UpdateNumSelectedLabel();
			cmbSelectionSet.Text = null;
		}

		private void UpdateNumSelectedLabel()
		{
			int numSelected = numItemsSelected();
			lblSelectedStats.Text = string.Format("{0} of {1} selected",
				numSelected, lstItems.Items.Count);
		}

		private int numItemsSelected()
		{
			int numSelected = 0;
			foreach (Infragistics.Win.UltraWinListView.UltraListViewItem item in lstItems.Items)
			{
				if (item.CheckState == CheckState.Checked)
					numSelected++;
			} // foreach  (item)
			return numSelected;
		}

		private void cmbSelectionSet_AfterCloseUp(object sender, EventArgs e)
		{
			if (cmbSelectionSet.SelectedItem != null)
			{
				UpdateSelectionSet(cmbSelectionSet.SelectedItem.DataValue.ToString());
				UpdateNumSelectedLabel();
			}
		}

		private void cmbSelectionSet_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
		{
			switch (e.Button.Key)
			{
				case "Save":
					string setToSave = cmbSelectionSet.Text;
					if (setToSave.Length > 0)
					{
						List<string> selectionSet = SelectionList();
						if (selectionSet.Count > 0)
							_PreEngine.SaveSelection(setToSave,selectionSet);

						UpdateSelectionSetComboList();
						cmbSelectionSet.SelectedIndex = cmbSelectionSet.Items.IndexOf(setToSave);
					} // if
					break;
				case "Delete":
					string setToDelete = cmbSelectionSet.Text;
					if (setToDelete.Length > 0)
					{
						_PreEngine.DeleteSelection(setToDelete);
						UpdateSelectionSetComboList();
						cmbSelectionSet.SelectedIndex = -1;
					}
					break;
			}
		}

		private void edtConfigFile_MouseMove(object sender, MouseEventArgs e)
		{
			Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditor =
				(Infragistics.Win.UltraWinEditors.UltraTextEditor)sender;
			Infragistics.Win.UIElement element = ultraTextEditor.UIElement.LastElementEntered;
			if (element != null)
			{
				Infragistics.Win.UltraWinEditors.EditorButtonBase editorButton =
					element.GetContext(typeof(Infragistics.Win.UltraWinEditors.EditorButtonBase)) as
					Infragistics.Win.UltraWinEditors.EditorButtonBase;
				if (editorButton != null)
				{
					Infragistics.Win.UltraWinToolTip.UltraToolTipInfo toolTipInfo =
						ultraToolTipManager1.GetUltraToolTip(ultraTextEditor);
					toolTipInfo.ToolTipText = editorButton.Key;
					return;
				}
			}
		}

		private void cmbSelectionSet_MouseMove(object sender, MouseEventArgs e)
		{
			Infragistics.Win.UltraWinEditors.UltraComboEditor ultraTextEditor =
				(Infragistics.Win.UltraWinEditors.UltraComboEditor)sender;
			Infragistics.Win.UIElement element = ultraTextEditor.UIElement.LastElementEntered;
			if (element != null)
			{
				Infragistics.Win.UltraWinEditors.EditorButtonBase editorButton =
					element.GetContext(typeof(Infragistics.Win.UltraWinEditors.EditorButtonBase)) as
					Infragistics.Win.UltraWinEditors.EditorButtonBase;
				if (editorButton != null)
				{
					Infragistics.Win.UltraWinToolTip.UltraToolTipInfo toolTipInfo =
						ultraToolTipManager1.GetUltraToolTip(ultraTextEditor);
					toolTipInfo.ToolTipText = editorButton.Key;
					return;
				}
			}
		}

		private void Main_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (log.IsDebugEnabled)
				log.Debug("UI closed");
		}
	}
}
