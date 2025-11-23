using EmployeesManagement.Code.Helper;
using EmployeesManagement.Code.Models;
using EmployeesManagement.Core;
using EmployeesManagement.Data.EF;
using EmployeesManagement.Gui.LoadingGui;
using EmployeesManagement.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeesManagement.Gui.UsersGui
{
    public partial class UsersUserControl : UserControl
    {
        private static UsersUserControl _instance;
        private frmAddUser _frmAddUser;
        private static Main _main;
        private IDataHelper<User> _dataHelper;
        private List<User> _data;

        public UsersUserControl()
        {
            InitializeComponent();
            _dataHelper = new UserEF();
            _data = new List<User>();
            LoadData();
        }

        public static UsersUserControl Instance(Main main)
        {
            _main = main;
            if (_instance == null)
                _instance = new UsersUserControl();
            return _instance;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {

            if (_frmAddUser == null || _frmAddUser.IsDisposed)
            {
                _frmAddUser = new frmAddUser(_main, 0, this);
                _frmAddUser.Show();
            }
            else
            {
                _frmAddUser.Focus();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            Edit();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Edit();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Search();
        }
        private void buttonExportAll_Click(object sender, EventArgs e)
        {

        }

        private void buttonExportDataGridView_Click(object sender, EventArgs e)
        {

        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
            }
        }


        // Methods

        public async void LoadData()
        {
            frmLoading.Instance(_main).Show();
            if (await Task.Run(() => _dataHelper.CanConnect()))
            {
                // Start Load Data
                // Check If Admin Or Not
                if (Code.Models.LocalUser.Role == "Admin")
                {
                    // Get All Data
                    _data = await Task.Run(() => _dataHelper.GetAllData());
                }
                else
                {
                    // Get Data By User
                    _data = await Task.Run(() => _dataHelper.GetDataByUser(LocalUser.UserId));
                }

                // Fill Data Grid View
                dataGridView1.DataSource = _data.ToList();

                // Set Columns Title
                SetColumns();

                // Clear Data
                _data.Clear();

            }
            else
            {
                // No Connection
                frmLoading.Instance(_main).Hide();
                ShowServerErrorState();
                MsgHelper.ShowServerError();
            }

            // Hide Loading
            frmLoading.Instance(_main).Hide();

            // Show Empty Data
            ShowEmptyDataState();
        }

        private void SetColumns()
        {
            dataGridView1.Columns[0].HeaderCell.Value = "المعرف";
            dataGridView1.Columns[1].HeaderCell.Value = "الاسم الكامل";
            dataGridView1.Columns[2].HeaderCell.Value = "أسم المستخدم";
            dataGridView1.Columns[3].HeaderCell.Value = "كلمة السر";
            dataGridView1.Columns[4].HeaderCell.Value = "الصلاحية";
            dataGridView1.Columns[5].HeaderCell.Value = "هل المستخدم ثانوي";
            dataGridView1.Columns[6].HeaderCell.Value = "المعرف الاساس";
            dataGridView1.Columns[7].HeaderCell.Value = "رقم الهاتف";
            dataGridView1.Columns[8].HeaderCell.Value = "البريد ألالكتروني";
            dataGridView1.Columns[9].HeaderCell.Value = "السكن";
            dataGridView1.Columns[10].HeaderCell.Value = "تاريخ الانشاء";
            dataGridView1.Columns[11].HeaderCell.Value = "تاريخ التعديل";

            // Unvisible Columns

            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;

        }

        private void ShowEmptyDataState()
        {
            // Set Title And Description
            labelStateTitle.Text = Properties.Resources.EmptyDataStateTitle;
            labelStateDescription.Text = Properties.Resources.EmptyDataStateDescription;
            panelState.Visible = dgvHelper.IsEmpty(dataGridView1);
        }
        private void ShowServerErrorState()
        {
            // Set Title And Description
            labelStateTitle.Text = Properties.Resources.ServerErrorTitle;
            labelStateDescription.Text = Properties.Resources.ServerErrorDescription;
            panelState.Visible = true;
        }

        private void Edit()
        {
            if (!dgvHelper.IsEmpty(dataGridView1))
            {
                // Get Selected Id
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                if (_frmAddUser == null || _frmAddUser.IsDisposed)
                {
                    _frmAddUser = new frmAddUser(_main, id, this);
                    _frmAddUser.Show();
                }
                else
                {
                    _frmAddUser.Focus();
                }
            }
            else
            {
                MsgHelper.ShowEmptyDataGridView();
            }
        }

        private async void Delete()
        {
            if (!dgvHelper.IsEmpty(dataGridView1))
            {

                // Confirm Delete
                if (!MessageBox.Show("هل انت متأكد أنك تريد حذف هذا المستخدم؟",
                    "تأكيد",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question).Equals(DialogResult.Yes))
                    return;


                // Get Selected Id
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                frmLoading.Instance(_main).Show();
                if (await Task.Run(() => _dataHelper.CanConnect()))
                {
                    var result = await Task.Run(() => _dataHelper.Delete(id));

                    if (result)
                    {
                        ToastHelper.ShowDeleteToast();
                        LoadData();
                    }
                    else
                    {
                        frmLoading.Instance(_main).Hide();
                        MessageBox.Show("An Error Occured", "Fail");
                    }

                    frmLoading.Instance(_main).Hide();

                }
                else
                {
                    frmLoading.Instance(_main).Hide();
                    MsgHelper.ShowServerError();
                }

            }
            else
            {
                frmLoading.Instance(_main).Hide();
                MsgHelper.ShowEmptyDataGridView();
            }
        }

        public async void Search()
        {
            // Show Loading
            frmLoading.Instance(_main).Show();
            if (await Task.Run(() => _dataHelper.CanConnect()))
            {
                // Start Load Data
                string searchItem = textBoxSearch.Text;
                // Check if Admin or not
                if (LocalUser.Role == "Admin")
                {
                    // Get All Data
                    _data = await Task.Run(() => _dataHelper.SearchAll(searchItem));
                }
                else
                {
                    // Get Data By User
                    _data = await Task.Run(() => _dataHelper.SearchByUser(LocalUser.UserId, searchItem));
                }

                // Fill DataGridView
                dataGridView1.DataSource = _data.ToList();


                // Set Columns Title
                //SetColumns();

                // Show Empty Data
                ShowEmptyDataState();

                // Clear Data
                _data.Clear();
                frmLoading.Instance(_main).Hide();
            }
            else
            {
                // No Connection
                frmLoading.Instance(_main).Hide();
                ShowServerErrorState();
                MsgHelper.ShowServerError();
            }

            // Hide Loading
            frmLoading.Instance(_main).Hide();


        }
    }
}
