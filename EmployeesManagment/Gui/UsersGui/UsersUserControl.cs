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

        private async void buttonExportAll_Click(object sender, EventArgs e)
        {
            // Show Loading
            frmLoading.Instance(_main).Show();
            if (await Task.Run(() => _dataHelper.CanConnect()))
            {
                // Start Load Data
                // Check if Admin or not
                if (LocalUser.Role == "Admin")
                {
                    // Get All Data
                    _data = await Task.Run(() => _dataHelper.GetAllData());
                }
                else
                {
                    // Get Data By User
                    _data = await Task.Run(() => _dataHelper.GetDataByUser(LocalUser.UserId));
                }
                frmLoading.Instance(_main).Hide();

                ExportExcel(_data);
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
        }

        private void buttonExportDataGridView_Click(object sender, EventArgs e)
        {
            // Get Data
            var data = (List<Core.User>)dataGridView1.DataSource;
            ExportExcel(data);

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

        private async void comboBoxNoOfPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Show Loading
                frmLoading.Instance(_main).Show();

                if (await Task.Run(() => _dataHelper.CanConnect()))
                {
                    // Start Load Data
                    // Check if Admin or not
                    if (LocalUser.Role == "Admin")
                    {
                        // Get All Data
                        _data = await Task.Run(() => _dataHelper.GetAllData());
                    }
                    else
                    {
                        // Get Data By User
                        _data = await Task.Run(() => _dataHelper.GetDataByUser(LocalUser.UserId));
                    }

                    // Get and Set Param
                    int index = Convert.ToInt32(comboBoxNoOfPages.SelectedItem);
                    int skip = (index - 1) * Properties.Settings.Default.NoOfDataGridViewItems;

                    // Fill DataGridView
                    dataGridView1.DataSource = _data.Skip(skip)
                        .Take(Properties.Settings.Default.NoOfDataGridViewItems).ToList();

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
            catch (Exception)
            {

                throw;
            }

        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (comboBoxNoOfPages.SelectedIndex != 0)
                comboBoxNoOfPages.SelectedIndex = comboBoxNoOfPages.SelectedIndex - 1;
            
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if ((comboBoxNoOfPages.SelectedIndex+1) < comboBoxNoOfPages.Items.Count)
                comboBoxNoOfPages.SelectedIndex = comboBoxNoOfPages.SelectedIndex + 1;
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

                labelNofOfItmes.Text = _data.Count.ToString();
                // Fill DataGridView
                dataGridView1.DataSource = _data.Take(Properties.Settings.Default.NoOfDataGridViewItems).ToList();
                if (_data.Count <= Properties.Settings.Default.NoOfDataGridViewItems)
                {
                    comboBoxNoOfPages.Items.Clear();
                    comboBoxNoOfPages.Items.Add(0);
                }
                else
                {
                    // Get and Add No of pages
                    double value = Convert.ToDouble(_data.Count) / Convert.ToDouble(Properties.Settings.Default.NoOfDataGridViewItems);
                    int noOfPage = Convert.ToInt32(Math.Round(value, MidpointRounding.AwayFromZero));
                    comboBoxNoOfPages.Items.Clear();
                    for (int i = 1; i <= noOfPage; i++)
                    {
                        comboBoxNoOfPages.Items.Add(i);
                    }
                }

                if(comboBoxNoOfPages.Items.Count > 0) 
                    comboBoxNoOfPages.SelectedIndex = 0;

                // Set Columns Title
                SetColumns();

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

        private void ExportExcel(List<Core.User> data)
        {
            // Define Data Table
            DataTable dataTable = new DataTable();

            // Convert to Data Table
            using (var reader = FastMember.ObjectReader.Create(data))
            {
                dataTable.Load(reader);
            }

            // Re-Set DataTable
            dataTable = arrangedDataTable(dataTable);

            // Send to export
            ExcelHelper.Export(dataTable, "Users");

        }

        private DataTable arrangedDataTable(DataTable dt)
        {
            dt.Columns["Id"].SetOrdinal(0);
            dt.Columns["Id"].ColumnName = "j";

            dt.Columns["FullName"].SetOrdinal(1);
            dt.Columns["FullName"].ColumnName = "الاسم الكامل";

            dt.Columns["UserName"].SetOrdinal(2);
            dt.Columns["UserName"].ColumnName = "أسم المستخدم";

            dt.Columns["Password"].SetOrdinal(3);
            dt.Columns["Password"].ColumnName = "كلمة السر";

            dt.Columns["Role"].SetOrdinal(4);
            dt.Columns["Role"].ColumnName = "الصلاحية";

            dt.Columns["IsSecondaryUser"].SetOrdinal(5);
            dt.Columns["IsSecondaryUser"].ColumnName = "هل المستخدم ثانوي";

            dt.Columns["UserId"].SetOrdinal(6);
            dt.Columns["UserId"].ColumnName = "معرف المستخدم";

            dt.Columns["Phone"].SetOrdinal(7);
            dt.Columns["Phone"].ColumnName = "رقم الهاتف";

            dt.Columns["Email"].SetOrdinal(8);
            dt.Columns["Email"].ColumnName = "البريد الالكتروني";

            dt.Columns["Address"].SetOrdinal(9);
            dt.Columns["Address"].ColumnName = "العنوان";

            dt.Columns["CreatedDate"].SetOrdinal(10);
            dt.Columns["CreatedDate"].ColumnName = "تاريخ الانشاء";

            dt.Columns["EditedDate"].SetOrdinal(11);
            dt.Columns["EditedDate"].ColumnName = "تاريخ التحديث";

            // Remove Unwanted Columns
            dt.Columns.Remove("Roles");
            dt.Columns.Remove("SystemRecords");

            return dt;
        }

    }
}
