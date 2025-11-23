using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeesManagement.Gui.ToastGui
{
    public partial class frmToast : Form
    {
        private static frmToast? _instance;
        private static string _title;
        private static string _description;
        public frmToast()
        {
            InitializeComponent();


        }

        public static frmToast Instance(string Title, string Description)
        {
            _title = Title;
            _description = Description;
            return _instance ?? (_instance = new frmToast());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Hide();
            timer1.Enabled = false;
        }

        private void frmToast_Activated(object sender, EventArgs e)
        {
            lblTitle.Text = _title;
            lblDescription.Text = _description;
            timer1.Interval = Properties.Settings.Default.ToastDuration;
            timer1.Enabled = true;
        }
    }
}
