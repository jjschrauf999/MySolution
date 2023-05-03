using System;
using System.Windows.Forms;

namespace Budget.Forms
{
    public partial class ForgotPassword : Form
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, System.EventArgs e)
        {
            this.Hide();
            Forms.Login loginForm = new Forms.Login();
            loginForm.Show();
        }

        private void btnForgotPasswrod_Click(object sender, System.EventArgs e)
        {
            string email = txtEmail.Text;

            Tuple<bool, string> forgotPassword = BusinessLogic.ForgotPassword.UserForgotPassword(email);
            if (!forgotPassword.Item1)
            {
                lblErrorMessage.Text = forgotPassword.Item2;
                return;
            }

            this.Hide();
            Forms.Login loginForm = new Forms.Login();
            loginForm.Show();
        }
    }
}
