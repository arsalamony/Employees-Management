using EmployeesManagement.Core;
using EmployeesManagement.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordsManagement.Data.EF
{
    public class EmployeeRecordEF : IDataHelper<EmployeeRecord>
    {

        private DBContext db;

        private EmployeeRecord _EmployeeRecord;

        public EmployeeRecordEF()
        {
            db = new DBContext();
            _EmployeeRecord = new EmployeeRecord();
        }
        public string Add(EmployeeRecord item)
        {
            try
            {
                db.EmployeeRecords.Add(item);
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
                _EmployeeRecord = Find(id);
                db.EmployeeRecords.Remove(_EmployeeRecord);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string Edit(EmployeeRecord item)
        {
            try
            {
                db = new DBContext();
                db.EmployeeRecords.Update(item);
                db.SaveChanges();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public EmployeeRecord Find(int id)
        {
            try
            {
                return db.EmployeeRecords.Find(id) ?? new EmployeeRecord();
            }
            catch (Exception ex)
            {
                return new EmployeeRecord();
            }
        }

        public List<EmployeeRecord> GetAllData()
        {
            try
            {
                db = new DBContext();
                return db.EmployeeRecords.OrderByDescending(x => x.Id).ToList();
            }
            catch
            {
                return new List<EmployeeRecord>();
            }
        }

        public List<EmployeeRecord> GetDataByUser(string userId)
        {
            try
            {
                db = new DBContext();
                return db.EmployeeRecords.Where(e => e.UserId.ToString() == userId).ToList();
            }
            catch (Exception ex)
            {
                return new List<EmployeeRecord>();
            }
        }

        public List<EmployeeRecord> SearchAll(string searchIteam)
        {
            try
            {
                return db.EmployeeRecords.Where(x => x.Id.ToString() == searchIteam ||
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
                return new List<EmployeeRecord>();
            }
        }

        public List<EmployeeRecord> SearchByUser(string userId, string searchIteam)
        {
            try
            {
                return db.EmployeeRecords.Where(x => x.UserId == userId).Where(x => x.Id.ToString() == searchIteam ||
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
                return new List<EmployeeRecord>();
            }
        }
    }
}
