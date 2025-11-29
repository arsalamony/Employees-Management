using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagement.Core
{
    public class EmployeeRecord
    {

        public int Id { get; set; }

        // General Data
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string EmpState { get; set; }
        public DateTime LastPromotionDate { get; set; }

        // Current Bonus
        public int CurrentDegree { get; set; }
        public int CurrentStage { get; set; }
        public float CurrentSalary { get; set; }
        public DateTime CurrentDate { get; set; }

        // Next Bonus
        public int NextDegree { get; set; }
        public int NextStage { get; set; }
        public float NextSalary { get; set; }
        public DateTime NextDate { get; set; }


        // Other

        public string Note { get; set; }

        public DateTime AddedDate { get; set; }
        public DateTime UpdateDate { get; set; }

        // Relationship
        public string UserId { get; set; }
        
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}
