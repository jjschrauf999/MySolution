using System;
using System.Windows.Forms;

namespace Budget.Forms
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            Tuple<bool, string> userLogin = BusinessLogic.Login.UserLogin(username, password);
            if (!userLogin.Item1)
            {
                lblLoginMessage.Text = userLogin.Item2;
                return;
            }

            if (BusinessLogic.TwoFactorAuth.SendTwoFactorAuthentication(username))
            {
                this.Hide();
                Forms.TwoFactorAuth twoFactorAuthForm = new Forms.TwoFactorAuth(username);
                twoFactorAuthForm.Show();
            }
            else if (BusinessLogic.Login.UserPasswordResetRequired(username))
            {
                this.Hide();
                Forms.ResetPassword resetpasswordForm = new Forms.ResetPassword(username);
                resetpasswordForm.Show();
            }
            else
            {
                this.Hide();
                Forms.Home homeForm = new Forms.Home();
                homeForm.Show();
            }
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            Forms.CreateUser createUserForm = new Forms.CreateUser();
            createUserForm.Show();
        }

        private void lnkLblForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            ForgotPassword forgotPasswordForm = new ForgotPassword();
            forgotPasswordForm.Show();
        }
    }
}
