using System.Data;
using ClosedXML.Excel;

namespace EmployeesManagement.Code.Helper
{
    public static class ExcelHelper
    {

        public static void Export(DataTable dt, string sheetName)  
        {
            
            // Define Save Dialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.DefaultExt = "xlsx";
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "Excel Files (.xlsx)|*.xlsx";
            saveFileDialog.Title = "Export Excel File";
            saveFileDialog.FileName = "Untiteled.xlsx";
            var result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Export
                try
                {
                    using (XLWorkbook xl = new XLWorkbook())
                    {
                        xl.AddWorksheet(dt, sheetName);

                        using (MemoryStream stream = new MemoryStream())
                        {
                            xl.SaveAs(stream);
                            File.WriteAllBytes(saveFileDialog.FileName, stream.ToArray());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }
    }
}
