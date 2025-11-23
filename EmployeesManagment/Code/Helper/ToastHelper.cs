using EmployeesManagement.Gui.ToastGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagement.Code.Helper
{
    public static class ToastHelper
    {
        public static void ShowAddToast() 
        {
            frmToast.Instance("إضافة البيانات", "تم إضافة البيانات بنجاح").Show();
        }

        public static void ShowEditToast()
        {
            frmToast.Instance("تعديل بيانات", "تم تعديل البيانات بنجاح").Show();
        }

        public static void ShowDeleteToast()
        {
            frmToast.Instance("حذف بيانات", "تم حذف البيانات بنجاح").Show();
        }
    }
}
