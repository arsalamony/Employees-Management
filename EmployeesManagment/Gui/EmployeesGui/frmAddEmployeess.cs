using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EmployeesManagement.Code.Helper;
using EmployeesManagement.Code.Models;
using EmployeesManagement.Core;
using EmployeesManagement.Data.EF;
using EmployeesManagement.Gui.BooksThanksGui;
using EmployeesManagement.Gui.LoadingGui;
using System.Data;

namespace EmployeesManagement.Gui.EmployeesGui
{
    public partial class frmAddEmployees : Form
    {
        private readonly IDataHelper<Core.Employee> _dataHelperForEmployees;
        //private readonly IDataHelper<Core.EmployeesRecord> _dataHelperForEmployeesRecords;
        private readonly IDataHelper<Core.SalaryRate> _dataHelperForSalaryRate;
        private readonly IDataHelper<Core.SystemRecord> _dataHelperForSystemRecords;
        private readonly Main _main;
        private int _Id;
        private List<Core.SalaryRate> salaryList;
        private readonly ctrlEmployees page;


        public frmAddEmployees(Main main, int Id, ctrlEmployees page)
        {
            InitializeComponent();

            _dataHelperForEmployees = new EmployeeEF();
            //_dataHelperForEmployeesRecords = new EmployeesRecordsRecordsEF();
            _dataHelperForSystemRecords = new SystemRecordEF();
            _dataHelperForSalaryRate = new SalaryRateEF();
            this.Owner = main;
            salaryList = new List<Core.SalaryRate>();


            this._main = main;
            this._Id = Id;
            this.page = page;

            if (Id > 0)
            {
                SetDataForEdit();
            }

            // Set Gui
            labelSBonusCurrency.Text = Properties.Settings.Default.Currency;
            labelSalryCurrency.Text = Properties.Settings.Default.Currency;
            SetRoleOfTabs();
            AutoFillData();
            GetSalryRates();
        }

        private void AutoComplete(List<Core.SalaryRate> salaryRateslist)
        {

            // Get Current Data
            int currentDegree = (int)numericUpDownCurrentDegree.Value;
            int currentStage = (int)numericUpDownCurrentStage.Value;

            var currentRate = salaryRateslist.Where(x => x.Degree == currentDegree)
                .FirstOrDefault() ?? new Core.SalaryRate();
            if (currentRate != null)
            {
                if (currentRate.PromotionYear == currentStage)
                {
                    // ترفيع
                    numericUpDownNextDegree.Value = currentDegree > 1 ? currentDegree - 1 : currentDegree;
                    numericUpDownNextStage.Value = 1;
                    comboBoxEmpState.SelectedItem = "ترفيع";
                }
                else
                {
                    // علاوة

                    numericUpDownNextDegree.Value = currentDegree;
                    numericUpDownNextStage.Value = currentStage + 1;
                    comboBoxEmpState.SelectedItem = "علاوة";
                }

                // Set Date

                dateTimePickerNextDate.Value = dateTimePickerCurrentDate.Value.AddYears(1);

                // Set Salary

                textBoxCurrentSalary.Text = GetSalary(currentStage, currentRate).ToString();
                textBoxNextSalary.Text = GetSalary((int)numericUpDownNextStage.Value, currentRate).ToString();
            }


        }

        private double GetSalary(int stage, Core.SalaryRate salaryRate)
        {
            if (stage == 1)
            {
                return salaryRate.Salary;
            }
            else
            {
                return (--stage * salaryRate.BonusYearRate) + salaryRate.Salary;
            }
        }
        private async void GetSalryRates()
        {

            frmLoading.Instance(_main).Show();
            if (await Task.Run(() => _dataHelperForEmployees.CanConnect()))
            {

                salaryList = await Task.Run(() => _dataHelperForSalaryRate.GetDataByUser(LocalUser.UserId));
            }
            frmLoading.Instance(_main).Hide();
        }


        private AutoCompleteStringCollection ConvertToAutoCompleteStringCollection(List<string?> dataList)
        {
            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            collection.Clear();
            foreach (var item in dataList)
            {
                collection.Add(item);
            }

            return collection;
        }

        private async void AutoFillData()
        {
            List<string?> dataList = new List<string?>();

            frmLoading.Instance(_main).Show();
            if (await Task.Run(() => _dataHelperForEmployees.CanConnect()))
            {
                // AutoFill Job Title
                // Get Data
                dataList = await Task.Run(() => _dataHelperForEmployees
                .GetDataByUser(LocalUser.UserId)
                .Select(x => x.JobTitle).Distinct().ToList());
                // Fill ComboBox
                comboBoxJobTitle.DataSource = dataList;
                comboBoxJobTitle.AutoCompleteCustomSource = ConvertToAutoCompleteStringCollection(dataList);


                // AutoFill Job Title
                // Get Data
                dataList = await Task.Run(() => _dataHelperForEmployees
                .GetDataByUser(LocalUser.UserId)
                .Select(x => x.EmpState).Distinct().ToList());
                // Fill ComboBox
                dataList.Add("علاوة");
                dataList.Add("ترفيع");
                dataList.Add("قيد علاوة");
                dataList.Add("قيد ترفيع");
                comboBoxEmpState.DataSource = dataList.Distinct().ToList();
                comboBoxEmpState.AutoCompleteCustomSource = ConvertToAutoCompleteStringCollection(dataList);

            }
            frmLoading.Instance(_main).Hide();
        }
        private bool IsValid()
        {
            if (
                textBoxFullName.Text.Length < 3 ||
                comboBoxJobTitle.Text.Length < 3 ||
                comboBoxEmpState.Text.Length < 2
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
                if (await Task.Run(() => _dataHelperForEmployees.CanConnect()))
                {
                    // Check Duplicated Item

                    string FullName = textBoxFullName.Text;

                    var result = await Task.Run(() => _dataHelperForEmployees
                    .GetDataByUser(LocalUser.UserId)
                    .Where(x => x.Id != _Id)
                    .Where(x => x.Name == FullName)
                    .FirstOrDefault() ?? new Employee());

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
            // Set Employees
            Employee employees = new Employee
            {
                Name = textBoxFullName.Text,
                JobTitle = comboBoxJobTitle.Text,
                EmpState = comboBoxEmpState.Text,
                LastPromotionDate = dateTimePickerLastPromotion.Value.Date,

                CurrentDegree = (int)numericUpDownCurrentDegree.Value,
                CurrentStage = (int)numericUpDownCurrentStage.Value,
                CurrentSalary = (float)Convert.ToDouble(textBoxCurrentSalary.Text),
                CurrentDate = dateTimePickerCurrentDate.Value.Date,

                NextDegree = (int)numericUpDownNextDegree.Value,
                NextStage = (int)numericUpDownNextStage.Value,
                NextSalary = (float)Convert.ToDouble(textBoxNextSalary.Text),
                NextDate = dateTimePickerNextDate.Value.Date,

                Note = richTextBoxNote.Text,

                AddedDate = DateTime.Now,
                UpdateDate = DateTime.Now,

                UserId = LocalUser.UserId,
            };

            // Send Data to database
            var result = await Task.Run(() => _dataHelperForEmployees.Add(employees));
            if (result == "1")
            {

                // Success
                SystemRecordHelper.Add("اضافة موظف",
                    $"تم اضافة موظف جديد يحمل الرقم التعريفي {employees.Id}");
                page.LoadData();
                ToastHelper.ShowAddToast();
                _Id = employees.Id;
                SetRoleOfTabs();
            }
            else
            {
                // Msg Box with result
                MessageBox.Show(result);
            }
        }

        private async void Edit()
        {

            // Set Employees
            Employee employees = new Employee
            {
                Name = textBoxFullName.Text,
                JobTitle = comboBoxJobTitle.Text,
                EmpState = comboBoxEmpState.Text,
                LastPromotionDate = dateTimePickerLastPromotion.Value.Date,

                CurrentDegree = (int)numericUpDownCurrentDegree.Value,
                CurrentStage = (int)numericUpDownCurrentStage.Value,
                CurrentSalary = (float)Convert.ToDouble(textBoxCurrentSalary.Text),
                CurrentDate = dateTimePickerCurrentDate.Value.Date,

                NextDegree = (int)numericUpDownNextDegree.Value,
                NextStage = (int)numericUpDownNextStage.Value,
                NextSalary = (float)Convert.ToDouble(textBoxNextSalary.Text),
                NextDate = dateTimePickerNextDate.Value.Date,

                Note = richTextBoxNote.Text,

                AddedDate = DateTime.Now,
                UpdateDate = DateTime.Now,



                UserId = LocalUser.UserId,
                Id = _Id
            };

            // Send Data to database
            var result = await Task.Run(() => _dataHelperForEmployees.Edit(employees));
            if (result == "1")
            {

                // Success
                SystemRecordHelper.Add("تعديل موظف",
                    $"تم تعديل موظف حالة يحمل الرقم التعريفي {employees.Id}");
                page.LoadData();
                ToastHelper.ShowEditToast();
            }
            else
            {
                // Msg Box with result
                MessageBox.Show(result);
            }
        }

        private async void SetDataForEdit()
        {
            // Get Edit Employees Data
            var _employees = await Task.Run(() => _dataHelperForEmployees.Find(_Id));
            if (_employees.Id > 0)
            {
                textBoxFullName.Text = _employees.Name;
                comboBoxJobTitle.Text = _employees.JobTitle;
                comboBoxEmpState.Text = _employees.EmpState;
                dateTimePickerLastPromotion.Value = _employees.LastPromotionDate;

                numericUpDownCurrentDegree.Value = _employees.CurrentDegree;
                numericUpDownCurrentStage.Value = _employees.CurrentStage;
                textBoxCurrentSalary.Text = _employees.CurrentSalary.ToString();
                dateTimePickerCurrentDate.Value = _employees.CurrentDate;

                numericUpDownNextDegree.Value = _employees.NextDegree;
                numericUpDownNextStage.Value = _employees.NextStage;
                textBoxNextSalary.Text = _employees.NextSalary.ToString();
                dateTimePickerNextDate.Value = _employees.NextDate;

                richTextBoxNote.Text = _employees.Note;

            }
        }

        private async void SetRoleOfTabs()
        {
            if (_Id == 0)
            {
                buttonAutoCal.Enabled = false;
                buttonNew.Enabled = false;
                buttonUpgrade.Enabled = false;

                foreach (TabPage tab in tabControl1.TabPages)
                {
                    if (tab.Name != "tabPage1")
                    {
                        tab.Enabled = false;
                    }
                }


            }
            else
            {
                buttonAutoCal.Enabled = true;
                buttonNew.Enabled = true;
                buttonUpgrade.Enabled = true;

                foreach (TabPage tab in tabControl1.TabPages)
                {
                    if (tab.Name != "tabPage1")
                    {
                        tab.Enabled = true;
                    }
                }

                AddUserControlEffetValueGui();
            }
        }

        private async void AddUserControlEffetValueGui()
        {
            // Book Thanks
            tabControl1.TabPages[1].Controls.Clear();
            Employee employees = await Task.Run(() => _dataHelperForEmployees.Find(_Id));
            ctrlBookThank bookThankUserControl = new ctrlBookThank(employees);
            bookThankUserControl.Dock = DockStyle.Fill;
            tabControl1.TabPages[1].Controls.Add(bookThankUserControl);

            // Bonus Records
            //tabControl1.TabPages[4].Controls.Clear();
            //EmployeesRecordUserControl employeesRecordUserControl = new EmployeesRecordUserControl(employees);
            //employeesRecordUserControl.Dock = DockStyle.Fill;
            //tabControl1.TabPages[4].Controls.Add(employeesRecordUserControl);
        }

        private void textBoxCurrentSalary_MouseLeave(object sender, EventArgs e)
        {
            if (!float.TryParse(textBoxCurrentSalary.Text, out float value) || value < 0)
            {
                textBoxCurrentSalary.Text = "0";
                MsgHelper.ShowNumberValid();
            }
        }

        private void textBoxNextSalary_MouseLeave(object sender, EventArgs e)
        {
            if (!float.TryParse(textBoxNextSalary.Text, out float value) || value < 0)
            {
                textBoxNextSalary.Text = "0";
                MsgHelper.ShowNumberValid();
            }
        }

        private void numericUpDownCurrentDegree_ValueChanged(object sender, EventArgs e)
        {
            AutoComplete(salaryList);
        }

        private void numericUpDownCurrentStage_ValueChanged(object sender, EventArgs e)
        {
            AutoComplete(salaryList);
        }

        private void numericUpDownNextDegree_ValueChanged(object sender, EventArgs e)
        {
            AutoComplete(salaryList);
        }

        private void numericUpDownNextStage_ValueChanged(object sender, EventArgs e)
        {
            AutoComplete(salaryList);
        }

        private void buttonAutoCal_Click(object sender, EventArgs e)
        {
            SetDataForEdit();
            AutoComplete(salaryList);
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            this.Close();
            //page.buttonAdd_Click(sender, e);
        }

        private void AddEmployeesForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonUpgrade_Click(object sender, EventArgs e)
        {

            //var reuslt = MessageBox.Show("هل انت متأكد من هذا الاجراء", "اجراء ترقية", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (reuslt == DialogResult.Yes)
            //{
            //    // Save Record
            //    AddRecored();

            //    // Update Data
            //    UpdateBeforeSaveRecord();

            //    // Save Data
            //    Edit();
            //}

        }
    }
}
