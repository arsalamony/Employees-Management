using EmployeesManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagement.Code.Models
{
    internal static class LocalUser
    {
        public static int Id { get; set; } = 1;

        public static string FullName { get; set; } = "Elfofo";
        public static string UserName { get; set; }

        public static string Password { get; set; }

        public static string Role { get; set; } = "Admin";

        public static bool IsSecondaryUser { get; set; } = false;

        public static string UserId { get; set; } = "sdafio12423sdfdsf1";

        public static string? Phone { get; set; }

        public static string? Email { get; set; }

        public static string? Address { get; set; }

        public static DateTime CreatedDate { get; set; }
        public static DateTime EditedDate { get; set; }

        //Navigation

        public static List<Role> Roles { get; set; }

        public static List<SystemRecord> SystemRecords { get; set; }
    }
}
