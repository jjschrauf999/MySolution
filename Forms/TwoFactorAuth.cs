using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Budget.Forms
{
    public partial class TwoFactorAuth : Form
    {
        private string _username;

        public TwoFactorAuth(string username)
        {
            InitializeComponent();
            _username = username;
        }
    }
}
