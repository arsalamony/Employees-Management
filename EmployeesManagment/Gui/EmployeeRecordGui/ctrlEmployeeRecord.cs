using DocumentFormat.OpenXml.Office2010.Excel;
using EmployeeRecordsManagement.Data.EF;
using EmployeesManagement.Code.Helper;
using EmployeesManagement.Code.Models;
using EmployeesManagement.Core;
using EmployeesManagement.Data.EF;
using EmployeesManagement.Gui.LoadingGui;
using System.Data;


namespace EmployeesManagement.Gui.EmployeeRecordGui
{
    public partial class ctrlEmployeeRecord : UserControl
    {
        private static Main _main;
        private IDataHelper<Core.EmployeeRecord> _dataHelper;
        private List<Core.EmployeeRecord> _data;
        private readonly Employee employee;

        public ctrlEmployeeRecord(Core.Employee employee)
        {
            InitializeComponent();
            _dataHelper = new EmployeeRecordEF();
            _data = new List<Core.EmployeeRecord>();
            this.employee = employee;
            LoadData();
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
                    _data = await Task.Run(() => _dataHelper.GetDataByUser(LocalUser.UserId));
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

        private async void buttonExport_Click(object sender, EventArgs e)
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
                    _data = await Task.Run(() => _dataHelper.GetDataByUser(LocalUser.UserId));
                }
                else
                {
                    // Get Data By User
                    _data = await Task.Run(() => _dataHelper.GetDataByUser(LocalUser.UserId));
                }
                frmLoading.Instance(_main).Hide();

                ExportExcel(_data);
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
            var data = (List<Core.EmployeeRecord>)dataGridView1.DataSource;
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
                        _data = await Task.Run(() => _dataHelper.GetDataByUser(LocalUser.UserId));
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
                    _data = await Task.Run(() => _dataHelper.GetDataByUser(LocalUser.UserId).Where(x => x.EmployeeId == employee.Id).ToList());
                }
                else
                {
                    // Get Data By User
                    _data = await Task.Run(() => _dataHelper.GetDataByUser(LocalUser.UserId).Where(x => x.EmployeeId == employee.Id).ToList());
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
            dataGridView1.Columns[2].HeaderCell.Value = "العنوان الوظيفي";
            dataGridView1.Columns[3].HeaderCell.Value = "الحالة";
            dataGridView1.Columns[4].Visible = false;

            dataGridView1.Columns[5].HeaderCell.Value = "درجة-ح";
            dataGridView1.Columns[6].HeaderCell.Value = "مرحلة-ح";
            dataGridView1.Columns[7].HeaderCell.Value = "راتب-ح";
            dataGridView1.Columns[8].HeaderCell.Value = "التاريخ-ح";

            dataGridView1.Columns[9].HeaderCell.Value = "درجة-ق";
            dataGridView1.Columns[10].HeaderCell.Value = "مرحلة-ق";
            dataGridView1.Columns[11].HeaderCell.Value = "راتب-ق";
            dataGridView1.Columns[12].HeaderCell.Value = "التاريخ-ق";

            // Visible of Columns
            dataGridView1.Columns[13].Visible = false;
            dataGridView1.Columns[14].Visible = false;
            dataGridView1.Columns[15].Visible = false;
            dataGridView1.Columns[16].Visible = false;
            dataGridView1.Columns[17].Visible = false;

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


        private async void Delete()
        {
            if (!dgvHelper.IsEmpty(dataGridView1))
            {
                // Get Selected Id
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                // Confirm Delete
                if (!MessageBox.Show($"هل انت متأكد أنك تريد حذف هذه العلاوة التي تحمل الرقم التعريفي {id}؟",
                    "تأكيد",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question).Equals(DialogResult.Yes))
                    return;



                frmLoading.Instance(_main).Show();
                if (await Task.Run(() => _dataHelper.CanConnect()))
                {
                    var result = await Task.Run(() => _dataHelper.Delete(id));

                    if (result)
                    {
                        ToastHelper.ShowDeleteToast();
                        LoadData();
                        SystemRecordHelper.Add("حذف علاوة",
                            $"تم حذف علاوة حالية الذي تحمل الرقم التعريفي {id.ToString()}");
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


        }

        private void ExportExcel(List<Core.EmployeeRecord> data)
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
            ExcelHelper.Export(dataTable, "DataRates");

        }

        private DataTable arrangedDataTable(DataTable dataTable)
        {
            dataTable.Columns["Id"].SetOrdinal(0);
            dataTable.Columns["Id"].ColumnName = "ت";

            dataTable.Columns["Degree"].SetOrdinal(1);
            dataTable.Columns["Degree"].ColumnName = "الدرجة";


            dataTable.Columns["Salary"].SetOrdinal(2);
            dataTable.Columns["Salary"].ColumnName = $"الراتب الاسمي {Properties.Settings.Default.Currency}";


            dataTable.Columns["BonusYearRate"].SetOrdinal(3);
            dataTable.Columns["BonusYearRate"].ColumnName = $"العلاوة السنوية {Properties.Settings.Default.Currency}";

            dataTable.Columns["PromotionYear"].SetOrdinal(4);
            dataTable.Columns["PromotionYear"].ColumnName = "سنوات الترفيع";


            // Removed columns
            dataTable.Columns.Remove("UsersId");

            return dataTable;
        }

    }
}
