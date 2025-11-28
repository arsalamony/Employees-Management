using EmployeesManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagement.Data.EF
{
    public class EmployeeEF : IDataHelper<Employee>
    {

        private DBContext db;

        private Employee _Employee;

        public EmployeeEF()
        {
            db = new DBContext();
            _Employee = new Employee();
        }
        public string Add(Employee item)
        {
            try
            {
                db.Employees.Add(item);
                db.SaveChanges();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool CanConnect()
        {
            db = new DBContext();
            return db.Database.CanConnect();
        }

        public bool Delete(int id)
        {
            try
            {
                _Employee = Find(id);
                db.Employees.Remove(_Employee);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string Edit(Employee item)
        {
            try
            {
                db = new DBContext();
                db.Employees.Update(item);
                db.SaveChanges();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Employee Find(int id)
        {
            try
            {
                return db.Employees.Find(id) ?? new Employee();
            }
            catch (Exception ex)
            {
                return new Employee();
            }
        }

        public List<Employee> GetAllData()
        {
            try
            {
                db = new DBContext();
                return db.Employees.OrderByDescending(x => x.Id).ToList();
            }
            catch
            {
                return new List<Employee>();
            }
        }

        public List<Employee> GetDataByUser(string userId)
        {
            try
            {
                db = new DBContext();
                return db.Employees.Where(e => e.UserId.ToString() == userId).ToList();
            }
            catch (Exception ex)
            {
                return new List<Employee>();
            }
        }

        public List<Employee> SearchAll(string searchIteam)
        {
            try
            {
                return db.Employees.Where(x => x.Id.ToString() == searchIteam ||
                x.UserId == searchIteam ||

                x.Name.Contains(searchIteam) ||
                x.JobTitle.Contains(searchIteam) ||
                x.EmpState == searchIteam ||
                x.LastPromotionDate.ToString() == searchIteam ||


                x.CurrentDegree.ToString() == searchIteam ||
                x.CurrentStage.ToString() == searchIteam ||
                x.CurrentSalary.ToString() == searchIteam ||
                x.CurrentDate.ToString() == searchIteam ||

                x.NextDegree.ToString() == searchIteam ||
                x.NextStage.ToString() == searchIteam ||
                x.NextSalary.ToString() == searchIteam ||
                x.NextDate.ToString() == searchIteam ||

                x.Note.Contains(searchIteam) ||


                x.AddedDate.ToString() == searchIteam ||
                x.UpdateDate.ToString() == searchIteam

                )
                    .OrderByDescending(x => x.Id).ToList();

            }
            catch
            {
                return new List<Employee>();
            }
        }

        public List<Employee> SearchByUser(string userId, string searchIteam)
        {
            try
            {
                return db.Employees.Where(x => x.UserId == userId).Where(x => x.Id.ToString() == searchIteam ||
                x.UserId == searchIteam ||

                x.Name.Contains(searchIteam) ||
                x.JobTitle.Contains(searchIteam) ||
                x.EmpState == searchIteam ||
                x.LastPromotionDate.ToString() == searchIteam ||


                x.CurrentDegree.ToString() == searchIteam ||
                x.CurrentStage.ToString() == searchIteam ||
                x.CurrentSalary.ToString() == searchIteam ||
                x.CurrentDate.ToString() == searchIteam ||

                x.NextDegree.ToString() == searchIteam ||
                x.NextStage.ToString() == searchIteam ||
                x.NextSalary.ToString() == searchIteam ||
                x.NextDate.ToString() == searchIteam ||

                x.Note.Contains(searchIteam) ||


                x.AddedDate.ToString() == searchIteam ||
                x.UpdateDate.ToString() == searchIteam
                )
                    .OrderByDescending(x => x.Id).ToList();

            }
            catch
            {
                return new List<Employee>();
            }
        }
    }
}
