using EmployeesManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagement.Code.Helper
{
    public static class RoleHelper
    {
        public static Dictionary<string, bool> localRoles { get; set; } // key: Role Name, Value: Has Role or Not
    }
}
