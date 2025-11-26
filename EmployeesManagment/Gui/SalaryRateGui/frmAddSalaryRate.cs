using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EmployeesManagement.Code.Helper;
using EmployeesManagement.Code.Models;
using EmployeesManagement.Core;
using EmployeesManagement.Data.EF;
using EmployeesManagement.Gui.LoadingGui;
using System.Data;

namespace EmployeesManagement.Gui.SalaryRate
{
    public partial class frmAddSalaryRate : Form
    {
        private readonly IDataHelper<Core.SalaryRate> _dataHelperForSalary;
        private readonly IDataHelper<Core.SystemRecord> _dataHelperForSystemRecords;
        private readonly Main _main;
        private int _Id;
        private readonly ctrlSalaryRate page;


        public frmAddSalaryRate(Main main, int Id, ctrlSalaryRate page)
        {
            InitializeComponent();

            _dataHelperForSalary = new SalaryRateEF();
            _dataHelperForSystemRecords = new SystemRecordEF();

            this.Owner = main;


            _main = main;
            this._Id = Id;
            this.page = page;

            if (_Id > 0)
            {
                SetDataForEdit();
            }

            // Set Gui
            labelSBonusCurrency.Text = Properties.Settings.Default.Currency;
            labelSalryCurrency.Text = Properties.Settings.Default.Currency;
        }

        private void textBoxSalary_MouseLeave(object sender, EventArgs e)
        {
            if (!float.TryParse(textBoxSalary.Text, out float salaryRate) || salaryRate < 0)
            {
                textBoxSalary.Text = "0";
                MsgHelper.ShowNumberValid();
            }
        }

        private void textBoxBonusYear_MouseLeave(object sender, EventArgs e)
        {
            if (!float.TryParse(textBoxBonusYear.Text, out float salaryRate) || salaryRate < 0)
            {
                textBoxBonusYear.Text = "0";
                MsgHelper.ShowNumberValid();
            }
        }

        private bool IsValid()
        {
            if (
                numericUpDownDegree.Value >= 0
                )
            {
                return true;
            }
            else
            {
                return false;
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
                if (await Task.Run(() => _dataHelperForSalary.CanConnect()))
                {
                    // Check Duplicated Item

                    int degree = (int)numericUpDownDegree.Value;

                    var result = await Task.Run(() => _dataHelperForSalary
                    .GetDataByUser(LocalUser.UserId)
                    .Where(x => x.Id != _Id)
                    .Where(x => x.Degree == degree)
                    .FirstOrDefault() ?? new Core.SalaryRate());

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

            // Set Salary
            Core.SalaryRate salaryRate = new Core.SalaryRate
            {
                Degree = (int)numericUpDownDegree.Value,
                PromotionYear = (int)numericUpDownPromtion.Value,
                Salary = (float)Convert.ToDecimal(textBoxSalary.Text),
                BonusYearRate = (float)Convert.ToDecimal(textBoxBonusYear.Text),
                UserId = LocalUser.UserId,
            };

            // Send Data to database
            var result = await Task.Run(() => _dataHelperForSalary.Add(salaryRate));
            if (result == "1")
            {

                // Success
                SystemRecordHelper.Add("اضافة درجة",
                    $"تم اضافة درجة جديد تحمل الرقم التعريفي {salaryRate.Id}");
                page.LoadData();
                ToastHelper.ShowAddToast();
            }
            else
            {
                // Msg Box with result
                MessageBox.Show(result);
            }

        }

        private async void Edit()
        {

            // Set Salary
            Core.SalaryRate salaryRate = new Core.SalaryRate
            {
                Degree = (int)numericUpDownDegree.Value,
                PromotionYear = (int)numericUpDownPromtion.Value,
                Salary = (float)Convert.ToDecimal(textBoxSalary.Text),
                BonusYearRate = (float)Convert.ToDecimal(textBoxBonusYear.Text),
                UserId = LocalUser.UserId,
                Id = _Id
            };

            // Send Data to database
            var result = await Task.Run(() => _dataHelperForSalary.Edit(salaryRate));
            if (result == "1")
            {

                // Success
                SystemRecordHelper.Add("تعديل درجة",
                    $"تم تعديل درجة حالية تحمل الرقم التعريفي {salaryRate.Id}");
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

        private async void SetDataForEdit()
        {
            // Get Edit Salary Data
            var _salary = await Task.Run(() => _dataHelperForSalary.Find(_Id));
            if (_salary != null)
            {
                textBoxSalary.Text = _salary.Salary.ToString();
                textBoxBonusYear.Text = _salary.BonusYearRate.ToString();
                numericUpDownDegree.Value = _salary.Degree;
                numericUpDownPromtion.Value = _salary.PromotionYear;
            }

        }


    }
}
