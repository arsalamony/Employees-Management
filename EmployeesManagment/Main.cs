using EmployeesManagement.Code.Helper;

namespace EmployeesManagement
{
    public partial class Main : Form
    {
        private PageHelper pageHelper;

        public Main()
        {
            InitializeComponent();

            pageHelper = new PageHelper(this);

            // Set Home Page
            pageHelper.SetPage(Gui.HomeGui.HomeUserControl.Instance());
            // Get And Set The Window State
            SetScreenState(Properties.Settings.Default.IsMaxScreen);

        }


        private void SetScreenState(bool IsMax)
        {
            if (IsMax)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveWindowStateSettings();
            Application.Exit();
        }

        private void SaveWindowStateSettings()
        {
            // Save Window State
            if (WindowState == FormWindowState.Maximized)
            {
                Properties.Settings.Default.IsMaxScreen = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.IsMaxScreen = false;
                Properties.Settings.Default.Save();
            }
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            pageHelper.SetPage(Gui.HomeGui.HomeUserControl.Instance());
        }

        private void buttonUsers_Click(object sender, EventArgs e)
        {
            pageHelper.SetPage(Gui.UsersGui.UsersUserControl.Instance(this));
        }

        private void buttonSystemRecords_Click(object sender, EventArgs e)
        {
            pageHelper.SetPage(Gui.SystemRecordGui.ctrlSystemRecord.Instance(this));
        }

        private void buttonSalayCategory_Click(object sender, EventArgs e)
        {
            pageHelper.SetPage(Gui.SalaryRate.ctrlSalaryRate.Instance(this));
        }

        private void buttonEmployees_Click(object sender, EventArgs e)
        {
            pageHelper.SetPage(Gui.EmployeesGui.ctrlEmployees.Instance(this));

        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
    //        Gui.SettingsGui.SettingForm settingForm = new Gui.SettingsGui.SettingForm();
    //        settingForm.Show();
        }
    }
}
