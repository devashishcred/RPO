using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Rpo.ApiServices.Model;
using Rpo.ApiServices.Model.Models;

namespace Rpo.Efiling
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            label2.Text = DateTime.Now.ToShortDateString();            
        }

        /// <summary>
        /// Handles the Click event of the btnLogin control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                lblErrorMsg.Text = "Please enter the username.";
                lblErrorMsg.ForeColor = Color.Red;
                txtUserName.Focus();
            }
            else if (string.IsNullOrEmpty(txtPassword.Text))
            {
                lblErrorMsg.Text = "Please enter the password.";
                lblErrorMsg.ForeColor = Color.Red;
                txtPassword.Focus();
            }
            else
            {
                StartProgress();
                lblErrorMsg.Text = string.Empty;
                lblErrorMsg.ForeColor = Color.Red;
                RpoContext rpoContext = new RpoContext();
                Employee employee = rpoContext.Employees.FirstOrDefault(x => x.Email == txtUserName.Text && x.LoginPassword == txtPassword.Text);
                if (employee != null && employee.Id > 0)
                {
                    if (employee.LoginPassword == txtPassword.Text)
                    {
                        CloseProgress();
                        MessageBox.Show("You have successfully login as : " + employee.FirstName + " " + employee.LastName);
                        MasterForm.IdEmployee = employee.Id;
                        MasterForm.EmployeeName = "Logged in as : " + employee.FirstName + " " + employee.LastName;
                        this.Close();
                    }
                    else
                    {
                        CloseProgress();
                        MessageBox.Show("Invalid username or password");
                    }
                }
                else
                {
                    CloseProgress();
                    MessageBox.Show("Invalid username or password");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            //txtUserName.Text = "prajesh.baria@credencys.com";
            //txtPassword.Text = "123456";

          
        }

        ModalLoadingUI objfrmShowProgress;

        private void StartProgress()
        {
            Cursor = Cursors.WaitCursor;
            objfrmShowProgress = new ModalLoadingUI();
            ShowProgress();
        }

        private void CloseProgress()
        {
            Thread.Sleep(200);
            try
            {
                if (objfrmShowProgress != null)
                {
                    objfrmShowProgress.Invoke(new Action(objfrmShowProgress.Close));
                }
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
            }
        }

        private void ShowProgress()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    try
                    {
                        objfrmShowProgress.ShowDialog();
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    Thread th = new Thread(ShowProgress);
                    th.IsBackground = false;
                    th.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
