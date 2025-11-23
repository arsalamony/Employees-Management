using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagement.Code.Helper
{
    public static class dgvHelper
    {
        public static bool IsEmpty(DataGridView dgv) 
        {
            if(dgv.Rows.Count == 0) 
                return true;
            return false;
        }
    }
}
