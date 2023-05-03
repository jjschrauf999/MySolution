using System;
using System.Windows.Forms;

namespace Budget.Forms
{
    public partial class CreateUser : Form
    {
        public CreateUser()
        {
            InitializeComponent();
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string email = txtEmail.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            Tuple<bool, string> createUser = BusinessLogic.CreateUser.CreateNewUser(firstName, lastName, email, username, password);
            if (!createUser.Item1)
            {
                lblCreateUserMessage.Text = createUser.Item2;
                return;
            }

            this.Hide();
            Forms.Home homeForm = new Forms.Home();
            homeForm.Show();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Forms.Login loginForm = new Forms.Login();
            loginForm.Show();
        }
    }
}
