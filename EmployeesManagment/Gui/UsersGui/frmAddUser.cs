using EmployeesManagement.Code.Helper;
using EmployeesManagement.Code.Models;
using EmployeesManagement.Core;
using EmployeesManagement.Data.EF;
using EmployeesManagement.Gui.LoadingGui;
using System.Data;


namespace EmployeesManagement.Gui.UsersGui
{
    public partial class frmAddUser : Form
    {
        private readonly IDataHelper<Core.User> _dataHelperForUser;
        private readonly IDataHelper<Role> _dataHelperForRole;
        private readonly IDataHelper<SystemRecord> _dataHelperForSystemRecord;
        private Main _main;
        private int _Id;
        private DateTime userCreatedDate;
        private readonly UsersUserControl page;


        public frmAddUser(Main main, int Id, UsersUserControl page)
        {
            InitializeComponent();
            _dataHelperForUser = new UserEF();
            _dataHelperForRole = new RoleEF();
            _dataHelperForSystemRecord = new SystemRecordEF();

            this.Owner = main;



            AddSecondaryUser();
            SetRoles();
            _main = main;
            this._Id = Id;
            this.page = page;

            if (Id > 0)
            {
                SetDataForEdit();
            }
        }

        private void checkBoxSecondayUser_CheckedChanged(object sender, EventArgs e)
        {

            comboBoxUserId.Enabled = checkBoxSecondayUser.Checked;
        }


        private void SetRoles()
        {
            if (Code.Models.LocalUser.Role == "Admin")
            {
                // Add Roles
                comboBoxRole.Items.Clear();
                comboBoxRole.Items.AddRange(new string[] { "Admin", "User", "Read" });

                checkBoxSecondayUser.Enabled = true;
                comboBoxUserId.Enabled = true;
                comboBoxRole.SelectedIndex = 1;
            }
            else
            {
                // Add Roles
                comboBoxRole.Items.Clear();
                comboBoxRole.Items.AddRange(new string[] { "User", "Read" });

                checkBoxSecondayUser.Enabled = false;
                checkBoxSecondayUser.Checked = true;
                comboBoxUserId.Enabled = false;
                comboBoxRole.SelectedIndex = 0;
            }
        }

        private void AddSecondaryUser()
        {
            comboBoxUserId.Items.Clear();
            comboBoxUserId.Items.Add(Code.Models.LocalUser.UserId);
            comboBoxUserId.SelectedIndex = 0;
        }

        private void comboBoxRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRolesByMainRole();
        }

        private void SetRolesByMainRole()
        {
            if (comboBoxRole.SelectedItem.ToString().Equals("Admin"))
            {
                checkBoxAbout.Checked = true;
                checkBoxAdd.Checked = true;
                checkBoxDelete.Checked = true;
                checkBoxEdit.Checked = true;
                checkBoxExport.Checked = true;
                checkBoxHelp.Checked = true;
                checkBoxHome.Checked = true;
                checkBoxHomeSearch.Checked = true;
                checkBoxReport.Checked = true;
                checkBoxRetirmnet.Checked = true;
                checkBoxSalary.Checked = true;
                checkBoxSearch.Checked = true;
                checkBoxSettings.Checked = true;
                checkBoxSystemRecords.Checked = true;
                checkBoxEmployees.Checked = true;
                checkBoxUsers.Checked = true;
                checkBoxPrint.Checked = true;
            }
            else if (comboBoxRole.SelectedItem.ToString().Equals("Admin"))
            {
                checkBoxAbout.Checked = true;
                checkBoxAdd.Checked = true;
                checkBoxDelete.Checked = true;
                checkBoxEdit.Checked = true;
                checkBoxExport.Checked = true;
                checkBoxHelp.Checked = true;
                checkBoxHome.Checked = true;
                checkBoxHomeSearch.Checked = true;
                checkBoxReport.Checked = true;
                checkBoxRetirmnet.Checked = true;
                checkBoxSalary.Checked = true;
                checkBoxSearch.Checked = true;
                checkBoxSettings.Checked = true;
                checkBoxSystemRecords.Checked = true;
                checkBoxEmployees.Checked = true;
                checkBoxUsers.Checked = true;
                checkBoxPrint.Checked = true;
            }
            else // Read
            {
                checkBoxAbout.Checked = true;
                checkBoxAdd.Checked = false;
                checkBoxDelete.Checked = false;
                checkBoxEdit.Checked = false;
                checkBoxExport.Checked = true;
                checkBoxHelp.Checked = true;
                checkBoxHome.Checked = true;
                checkBoxHomeSearch.Checked = true;
                checkBoxReport.Checked = true;
                checkBoxRetirmnet.Checked = true;
                checkBoxSalary.Checked = true;
                checkBoxSearch.Checked = true;
                checkBoxSettings.Checked = true;
                checkBoxSystemRecords.Checked = true;
                checkBoxEmployees.Checked = true;
                checkBoxUsers.Checked = false;
                checkBoxPrint.Checked = true;
            }
        }

        private bool IsValid()
        {
            if (
                textBoxFullName.Text.Length < 3 ||
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
                frmLoading.Instance(_main).Show();
                // Check Connection
                if (await Task.Run(() => _dataHelperForUser.CanConnect()))
                {
                    // Check Duplicated Item

                    string fullName = textBoxFullName.Text;
                    string userName = textBoxUserName.Text;

                    var result = await Task.Run(() => _dataHelperForUser
                    .GetAllData()
                    .Where(x => x.Id != _Id)
                    .Where(x => x.FullName == fullName || x.UserId == userName)
                    .FirstOrDefault() ?? new Core.User());

                    if (result.Id > 0)
                    {
                        // Msg
                        frmLoading.Instance(_main).Hide();
                        MsgHelper.ShowDuplicatedItems();
                    }
                    else
                    {
                        // Add
                        if (_Id == 0)
                        {
                            Add();
                        }
                        else
                        {
                            // Edit
                            Edit();
                        }
                    }


                }
                else
                {
                    frmLoading.Instance(_main).Hide();
                    MsgHelper.ShowServerError();
                }

                frmLoading.Instance(_main).Hide();
            }

        }

        private async void Add()
        {
            // Set User
            Core.User user = new Core.User
            {
                FullName = textBoxFullName.Text,
                Password = textBoxPassword.Text,
                UserName = textBoxUserName.Text,
                Email = textBoxEmail.Text,
                Address = textBoxAddress.Text,
                CreatedDate = DateTime.Now.Date,
                EditedDate = DateTime.Now.Date,
                Role = comboBoxRole.SelectedItem.ToString() ?? "User",
                Phone = textBoxPhone.Text,
                IsSecondaryUser = checkBoxSecondayUser.Checked,
                UserId = SetUserId()
            };

            // Send Data to database
            var result = await Task.Run(() => _dataHelperForUser.Add(user));
            if (result == "1")
            {
                // Add User Roles
                foreach (var item in flowLayoutPanelRoles.Controls)
                {
                    CheckBox checkBox = (CheckBox)item;
                    // Set
                    Role roles = new Role
                    {
                        Key = checkBox.Name,
                        Value = checkBox.Checked,
                        UserId = user.Id

                    };

                    // Send
                    await Task.Run(() => _dataHelperForRole.Add(roles));
                }

                // Success
                SystemRecordHelper.Add("اضافة مستخدم",
                    $"تم اضافة مستخدم جديد يحمل الرقم التعريفي {user.Id}");
                page.LoadData();
                ToastHelper.ShowAddToast();
                this.Close();
            }
            else
            {
                // Msg Box with result
                MessageBox.Show(result);
            }
        }

        private async void Edit()
        {

            // Set User
            Core.User user = new Core.User
            {
                Id = _Id,
                FullName = textBoxFullName.Text,
                Password = textBoxPassword.Text,
                UserName = textBoxUserName.Text,
                Email = textBoxEmail.Text,
                Address = textBoxAddress.Text,
                CreatedDate = userCreatedDate,
                EditedDate = DateTime.Now.Date,
                Role = comboBoxRole.SelectedItem.ToString() ?? "User",
                Phone = textBoxPhone.Text,
                IsSecondaryUser = checkBoxSecondayUser.Checked,
                UserId = SetUserId()
            };

            // Send Data to database
            var result = await Task.Run(() => _dataHelperForUser.Edit(user));
            if (result == "1")
            {
                // Remove Old User Roles
                var oldroles = await Task.Run(() => _dataHelperForRole.
                GetAllData().Where(x => x.UserId == _Id).ToList() ?? new List<Role>());
                foreach (var role in oldroles)
                {
                    await Task.Run(() => _dataHelperForRole.Delete(role.Id));
                }

                // Add User Roles
                foreach (var item in flowLayoutPanelRoles.Controls)
                {
                    CheckBox checkBox = (CheckBox)item;
                    // Set
                    Role roles = new Role
                    {
                        Key = checkBox.Name,
                        Value = checkBox.Checked,
                        UserId = user.Id

                    };

                    // Send
                    await Task.Run(() => _dataHelperForRole.Add(roles));
                }

                // Success
                // Success
                SystemRecordHelper.Add("تعديل مستخدم",
                    $"تم تعديل مستخدم حالي يحمل الرقم التعريفي {user.Id}");
                page.LoadData();
                ToastHelper.ShowEditToast();
                this.Close();
            }
            else
            {
                // Msg Box with result
                MessageBox.Show(result);
            }
        }

        private string SetUserId()
        {
            if (checkBoxSecondayUser.Checked)
            {
                return comboBoxUserId.SelectedItem.ToString() ?? LocalUser.UserId;
            }
            else
            {
                return Guid.NewGuid().ToString();
            }
        }


        private async void SetDataForEdit()
        {
            // Get Edit User Data
            var user = await Task.Run(() => _dataHelperForUser.Find(_Id));
            if (user != null)
            {
                textBoxFullName.Text = user.FullName;
                textBoxPassword.Text = user.Password;
                textBoxUserName.Text = user.UserName;
                textBoxEmail.Text = user.Email;
                textBoxPhone.Text = user.Phone;
                textBoxAddress.Text = user.Address;
                comboBoxRole.SelectedItem = user.Role;
                checkBoxSecondayUser.Checked = user.IsSecondaryUser;
                userCreatedDate = user.CreatedDate;
            }

            // Set Roles

            // Add User Roles
            foreach (var item in flowLayoutPanelRoles.Controls)
            {
                CheckBox checkBox = (CheckBox)item;

                checkBox.Checked = await Task.Run(() => _dataHelperForRole
                .GetAllData()
                .Where(x => x.UserId == _Id && x.Key == checkBox.Name)
                .Select(x => x.Value).FirstOrDefault());

            }

        }
    }
}
