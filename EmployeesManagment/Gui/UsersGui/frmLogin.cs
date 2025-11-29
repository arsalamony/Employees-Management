using EmployeesManagement.Code.Helper;
using EmployeesManagement.Code.Models;
using EmployeesManagement.Core;
using EmployeesManagement.Data.EF;
using EmployeesManagement.Gui.LoadingGui;
using System.Data;


namespace EmployeesManagement.Gui.UsersGui
{
    public partial class frmLogin : Form
    {
        private readonly IDataHelper<Core.User> _dataHelperForUser;
        private readonly IDataHelper<Role> _dataHelperForRole;
        private readonly IDataHelper<SystemRecord> _dataHelperForSystemRecord;
        private int _Id;



        public frmLogin()
        {
            InitializeComponent();
            _dataHelperForUser = new UserEF();
            _dataHelperForRole = new RoleEF();
            _dataHelperForSystemRecord = new SystemRecordEF();


        }

        private bool IsValid()
        {
            if (

                textBoxPassword.Text.Length < 3 ||
                textBoxUserName.Text.Length < 3
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private async void buttonSave_Click(object sender, EventArgs e)
        {

            // Check the fields
            if (!IsValid())
            {
                MsgHelper.ShowRequiredFields();
            }
            else
            {
                // Show Loading
                pictureBoxLoding.Visible = true;
                // Check Connection
                if (await Task.Run(() => _dataHelperForUser.CanConnect()))
                {

                    Login();

                }

                else
                {
                    pictureBoxLoding.Visible = false;

                    MsgHelper.ShowServerError();
                }

                pictureBoxLoding.Visible = false;

            }
        }

        private async void Login()
        {

            // Check user Name and Password
            string userName = textBoxUserName.Text,
                    password = textBoxPassword.Text;
            Core.User user = await Task.Run(() =>
            _dataHelperForUser.GetAllData()
            .Where(x => x.UserName == userName && x.Password == password).FirstOrDefault() ?? new Core.User());
            if (user.Id > 0)
            {
                // Set User Info
                LocalUser.Id = user.Id;
                LocalUser.UserName = user.UserName;
                LocalUser.Password = user.Password;
                LocalUser.Address = user.Address;
                LocalUser.UserId = user.UserId;
                LocalUser.FullName = user.FullName;
                LocalUser.Email = user.Email;
                LocalUser.IsSecondaryUser = user.IsSecondaryUser;

                // Get and set roles
                RoleHelper.localRoles = await Task.Run(() => _dataHelperForRole
                    .GetAllData()
                    .Where(x => x.UserId == user.Id).ToDictionary(e => e.Key , x => x.Value));

                // Success
                SystemRecordHelper.Add("تسجيل الدخول",
                    $"تم تسجيل دخول مستخدم     {user.FullName}");

                Main main = new Main();
                main.Show();
                this.Hide();
            }
            else
            {

                MessageBox.Show("معلومات تسجيل الدخول خاطئة", "خطأ في تسجيل الدخول", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
