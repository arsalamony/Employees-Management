using EmployeesManagement.Code.Models;
using EmployeesManagement.Core;
using EmployeesManagement.Data.EF;
using System.Net.NetworkInformation;


namespace EmployeesManagement.Code.Helper
{
    public static class SystemRecordHelper
    {
        public static void Add(string title, string description)
        {
            IDataHelper<SystemRecord> dataHelper = new SystemRecordEF();
            SystemRecord systemRecord = new SystemRecord
            {
                CreatedDate = DateTime.Now,
                Description = description,
                Title = title,
                DeviceName = Environment.UserName,
                UserFullName = LocalUser.FullName,
                UserId = LocalUser.Id,
                MachineId = GetMachineId()
                
            };
            dataHelper.Add(systemRecord);
        }

        private static string GetMachineId()
        {
            var networkinterfaces = NetworkInterface.GetAllNetworkInterfaces();
            string machineid = string.Empty;
            foreach (var networkinterface in networkinterfaces)
            {
                if (networkinterface.OperationalStatus == OperationalStatus.Up &&
                    networkinterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel &&
                    networkinterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    machineid = networkinterface.GetPhysicalAddress().ToString();
                }
            }

            if (machineid == string.Empty)
            {
                machineid = "Null";
            }
            return machineid;
        }
    }
}
