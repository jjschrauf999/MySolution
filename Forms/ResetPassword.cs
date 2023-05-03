using System;
using System.Windows.Forms;

namespace Budget.Forms
{
    public partial class ResetPassword : Form
    {
        private string _username;

        public ResetPassword(string username)
        {
            InitializeComponent();
            _username = username;
        }

        private void btnBack_Click(object sender, System.EventArgs e)
        {
            this.Hide();
            Forms.Login loginForm = new Forms.Login();
            loginForm.Show();
        }

        private void btnResetPassword_Click(object sender, System.EventArgs e)
        {
            string existingPassword = txtExistingPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            Tuple<bool, string> userPassword = BusinessLogic.ResetPassword.ResetUserPassword(_username, existingPassword, newPassword, confirmPassword);
            if (!userPassword.Item1)
            {
                lblErrorMessage.Text = userPassword.Item2;
                return;
            }

            this.Hide();
            Forms.Home homeForm = new Forms.Home();
            homeForm.Show();
        }
    }
}
