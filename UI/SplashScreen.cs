using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.IO;

namespace ArcSpooler.UI
{
    /// <summary>
    /// Form to display information screen about this application
    /// </summary>
    public partial class SplashScreen : Form
    {
        public SplashScreen(string versionText, string dateText)
        {
            InitializeComponent();

						int centeredLabelX = ultraLabel1.Left + (ultraLabel1.Width / 2);
            ultraLabel1.Text = "Version " + versionText + " (" + dateText + ")";
						ultraLabel1.Left = centeredLabelX - (ultraLabel1.Width / 2);
						ultraLabel1.Update();
            timer1.Start();
        }

        private void SplashScreen_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void ultraPictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //  Do nothing here!
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics gfx = e.Graphics; 
            Assembly ass = Assembly.GetExecutingAssembly();
            Stream stream = ass.GetManifestResourceStream(
							"ArcSpooler.UI.Resources.SplashArcSpooler.png");
            Image SplashScreen = Image.FromStream(stream);

            gfx.DrawImage(SplashScreen, new Rectangle(0, 0, this.Width, this.Height));

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Shows the SplashScreen as a modal dialog
        /// </summary>
        /// <param name="wait">Species whether to wait or user to click to close the dialog, or to close automatically</param>
        /// <returns>DialogResult.OK</returns>
        public DialogResult ShowDialog(Boolean wait)
        {
            if (wait)
            {
                timer1.Stop();
            }
            return this.ShowDialog();
        }
    }
}