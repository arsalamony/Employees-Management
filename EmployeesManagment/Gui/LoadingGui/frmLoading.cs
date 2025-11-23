using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeesManagement.Gui.LoadingGui
{
    public partial class frmLoading : Form
    {
        private static frmLoading _frm;

        private static Main _main;
        public frmLoading()
        {
            InitializeComponent();
        }

        public static frmLoading Instance(Main main)
        {
            _main = main;

            if (_frm == null)
                _frm = new frmLoading();
            return _frm;
        }
    }
}
