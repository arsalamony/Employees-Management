using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagement.Data.EF
{
    public interface IDataHelper<Table>
    {

        // Read { Admin || User }

        List<Table> GetAllData(); // Admin

        List<Table> GetDataByUser(string UserId); // User

        List<Table> SearchAll(string SearchItem); // Admin

        List<Table> SearchByUser(string UserId, string SearchItem); // User

        Table Find(int id);


        // Write

        string Add(Table item); // 1 = success , error

        string Edit(Table item); // 1 = success , error

        bool Delete(int id); // 1 = success , error

        // Other

        bool CanConnect();
    }
}
