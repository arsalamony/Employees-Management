using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EmployeesManagement.Code.Helper;
using EmployeesManagement.Code.Models;
using EmployeesManagement.Core;
using EmployeesManagement.Data.EF;
using EmployeesManagement.Gui.LoadingGui;
using System.Data;

namespace EmployeesManagement.Gui.BooksThanksGui
{
    public partial class frmAddBookThank : Form
    {
        private readonly IDataHelper<Core.BookThank> _dataHelperForBookThank;
        private readonly IDataHelper<Core.Employee> _dataHelperForEmployee;
        private readonly IDataHelper<Core.SystemRecord> _dataHelperForSystemRecords;
        private readonly Main _main;
        private int _Id;
        private readonly ctrlBookThank page;

        public Employee _employee;

        public frmAddBookThank(Main main, int Id, ctrlBookThank page, Employee employee)
        {
            InitializeComponent();

            _dataHelperForBookThank = new BookThankEF();
            _dataHelperForEmployee = new EmployeeEF();
            _dataHelperForSystemRecords = new SystemRecordEF();

            this.Owner = main;


            _main = main;
            this._Id = Id;
            this.page = page;
            _employee = employee;
            if (_Id > 0)
            {
                SetDataForEdit();
            }

        }


        private bool IsValid()
        {
            if (
                textBoxRef.Text.Trim() != string.Empty 
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
                if (await Task.Run(() => _dataHelperForBookThank.CanConnect()))
                {
                    // Check Duplicated Item

                    string bookRef = textBoxRef.Text;
                    var dateBook = dateTimePickerdate.Value.Date;

                    var result = await Task.Run(() => _dataHelperForBookThank
                    .GetDataByUser(LocalUser.UserId)
                    .Where(x => x.Id != _Id)
                    .Where(x => x.Ref == bookRef && x.BookThankDate == dateBook)
                    .FirstOrDefault() ?? new Core.BookThank());

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

            // Set BookThank
            Core.BookThank bookThank = new Core.BookThank
            {
                UserId = LocalUser.UserId,
                EffectValue = (int)numericUpDownEffect.Value,
                AddedDate = DateTime.Now.Date,
                BookThankDate = dateTimePickerdate.Value.Date,
                EmployeeId = _employee.Id,
                Note = richTextBoxNote.Text,
                Ref = textBoxRef.Text,
            };

            // Send Data to database
            var result = await Task.Run(() => _dataHelperForBookThank.Add(bookThank));
            if (result == "1")
            {

                // Success
                SystemRecordHelper.Add("اضافة كتاب شكر",
                    $"تم اضافة كتاب شكر جديد يحمل الرقم التعريفي {bookThank.Id}");

                // Add Data to Employees
                _employee.Note = _employee.Note + " | " + $"شكر بتأثير {bookThank.EffectValue} يوم ذي عدد {bookThank.Ref} في {bookThank.BookThankDate}";
                _employee.NextDate = _employee.NextDate.AddDays(bookThank.EffectValue * -1);

                await Task.Run(() => _dataHelperForEmployee.Edit(_employee));
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

            // Set BookThanks
            BookThank bookThanks = new BookThank
            {
                UserId = LocalUser.UserId,
                EffectValue = (int)numericUpDownEffect.Value,
                AddedDate = DateTime.Now.Date,
                BookThankDate = dateTimePickerdate.Value.Date,
                EmployeeId = _employee.Id,
                Note = richTextBoxNote.Text,
                Ref = textBoxRef.Text,
                Id = _Id
            };


            // Send Data to database
            var result = await Task.Run(() => _dataHelperForEmployee.Edit(_employee));
            if (result == "1")
            {

                // Success
                SystemRecordHelper.Add("تعديل كتاب شكر ",
                    $"تم تعديل كتاب شكر  يحمل الرقم التعريفي {bookThanks.Id}");
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
            // Get Edit BookThanks Data
            var _BookThanks = await Task.Run(() => _dataHelperForBookThank.Find(_Id));
            if (_BookThanks != null)
            {
                textBoxRef.Text = _BookThanks.Ref;
                numericUpDownEffect.Value = _BookThanks.EffectValue;
                richTextBoxNote.Text = _BookThanks.Note;
                dateTimePickerdate.Value = _BookThanks.BookThankDate;

            }

        }


    }
}
