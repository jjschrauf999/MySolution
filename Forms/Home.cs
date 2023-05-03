using System.Windows.Forms;

namespace Budget.Forms
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, System.EventArgs e)
        {
            //TODO remove this button
            this.Hide();
            Forms.Login loginForm = new Forms.Login();
            loginForm.Show();
        }
    }
}
