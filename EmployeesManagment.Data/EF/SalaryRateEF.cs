using EmployeesManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagement.Data.EF
{
    public class SalaryRateEF : IDataHelper<Core.SalaryRate>
    {
        private DBContext db;

        private SalaryRate _SalaryRate;

        public SalaryRateEF()
        {
            db = new DBContext();
            _SalaryRate = new SalaryRate();
        }
        public string Add(SalaryRate item)
        {
            try
            {
                db.SalaryRates.Add(item);
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
                _SalaryRate = Find(id);
                db.SalaryRates.Remove(_SalaryRate);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string Edit(SalaryRate item)
        {
            try
            {
                db = new DBContext();
                db.SalaryRates.Update(item);
                db.SaveChanges();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public SalaryRate Find(int id)
        {
            try
            {
                return db.SalaryRates.Find(id) ?? new SalaryRate();
            }
            catch (Exception ex)
            {
                return new SalaryRate();
            }
        }

        public List<SalaryRate> GetAllData()
        {
            try
            {
                db = new DBContext();
                return db.SalaryRates.OrderByDescending(x => x.Id).ToList();
            }
            catch
            {
                return new List<SalaryRate>();
            }
        }

        public List<SalaryRate> GetDataByUser(string userId)
        {
            try
            {
                db = new DBContext();
                return db.SalaryRates.Where(e => e.UserId.ToString() == userId).ToList();
            }
            catch (Exception ex)
            {
                return new List<SalaryRate>();
            }
        }

        public List<SalaryRate> SearchAll(string searchIteam)
        {
            try
            {
                return db.SalaryRates.Where(x => x.Id.ToString() == searchIteam

                )
                    .OrderByDescending(x => x.Id).ToList();

            }
            catch
            {
                return new List<SalaryRate>();
            }
        }

        public List<SalaryRate> SearchByUser(string userId, string searchIteam)
        {
            try
            {
                return db.SalaryRates.Where(x => x.UserId.ToString() == userId).Where(x => x.Id.ToString() == searchIteam

                )
                    .OrderByDescending(x => x.Id).ToList();

            }
            catch
            {
                return new List<SalaryRate>();
            }
        }

    }
}
