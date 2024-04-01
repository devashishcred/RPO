using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rpo.Efiling
{
    public partial class MasterForm : Form
    {
        public MasterForm()
        {
            InitializeComponent();
        }

        public static int IdEmployee { get; set; }
        public static string EmployeeName { get; set; }

        //private void eF1ToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    eF1ToolStripMenuItem.Enabled = false;
        //    aHVToolStripMenuItem.Enabled = false;
        //    RpoEfiling rpoEfiling = new RpoEfiling();
        //    rpoEfiling.MdiParent = this;
        //    rpoEfiling.FormClosed += RpoEfiling_FormClosed;
        //    rpoEfiling.Show();
        //}

        private void RpoEfiling_FormClosed(object sender, FormClosedEventArgs e)
        {
            //eF1ToolStripMenuItem.Enabled = true;
            //aHVToolStripMenuItem.Enabled = true;
        }

        //private void aHVToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    MessageBox.Show("This funcationality is under construction.");
        //}

        private void MasterForm_Load(object sender, EventArgs e)
        {

            if (IdEmployee <= 0)
            {
                //eF1ToolStripMenuItem.Enabled = false;
                //aHVToolStripMenuItem.Enabled = false;
                //menuStrip1.Visible = false;
                Login login = new Login();
                login.MdiParent = this;
                login.FormClosed += Login_FormClosed; ;
                login.Show();
            }
            else
            {
                RpoEfiling rpoEfiling = new RpoEfiling();
                rpoEfiling.MdiParent = this;
                rpoEfiling.FormClosed += RpoEfiling_FormClosed;
                rpoEfiling.Show();
            }
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (IdEmployee > 0)
            {
                //eF1ToolStripMenuItem.Enabled = true;
                //aHVToolStripMenuItem.Enabled = true;
                //lblLoggedInUser.Text = EmployeeName;
                //menuStrip1.Visible = true;
                RpoEfiling rpoEfiling = new RpoEfiling();
                rpoEfiling.MdiParent = this;
                rpoEfiling.FormBorderStyle = FormBorderStyle.None;
                rpoEfiling.ControlBox = false;
                rpoEfiling.MaximizeBox = false;
                rpoEfiling.MinimizeBox = false;
                rpoEfiling.ShowIcon = false;
                rpoEfiling.Text = "";
                rpoEfiling.Dock = DockStyle.Fill;
                rpoEfiling.FormClosed += RpoEfiling_FormClosed;
                rpoEfiling.Show();
            }
            //else
            //{
            //    eF1ToolStripMenuItem.Enabled = false;
            //    aHVToolStripMenuItem.Enabled = false;
            //    lblLoggedInUser.Text = string.Empty;
            //    menuStrip1.Visible = false;
            //}
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
