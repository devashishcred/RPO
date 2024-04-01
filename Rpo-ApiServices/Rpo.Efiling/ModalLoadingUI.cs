using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rpo.Efiling
{
    public partial class ModalLoadingUI : Form
    {
        public ModalLoadingUI()
        {
            InitializeComponent();
        }

        private void ModalLoadingUI_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            this.TransparencyKey = Color.White;
        }

        private void ModalLoadingUI_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
