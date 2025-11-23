using EmployeesManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagement.Data.EF
{
    public class SystemRecordEF : IDataHelper<SystemRecord>
    {
        private DBContext db;

        private SystemRecord SystemRecord;

        public SystemRecordEF()
        {
            db = new DBContext();
            SystemRecord = new SystemRecord();
        }
        public string Add(SystemRecord item)
        {
            try
            {
                db.SystemRecords.Add(item);
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
            return db.Database.CanConnect();
        }

        public bool Delete(int id)
        {
            try
            {
                SystemRecord = Find(id);
                db.SystemRecords.Remove(SystemRecord);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Edit(SystemRecord item)
        {
            try
            {
                db.SystemRecords.Update(item);
                db.SaveChanges();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public SystemRecord Find(int id)
        {
            try
            {
                return db.SystemRecords.Find(id)?? new SystemRecord();
            }
            catch (Exception ex)
            {
                return new SystemRecord();
            }
        }

        public List<SystemRecord> GetAllData()
        {
            try
            {
                return db.SystemRecords.ToList();
            }
            catch (Exception ex)
            {
                return new List<SystemRecord>();
            }
        }

        public List<SystemRecord> GetDataByUser(string SystemRecordId)
        {
            throw new NotImplementedException();
        }

        public List<SystemRecord> SearchAll(string SearchItem)
        {
            throw new NotImplementedException();
        }

        public List<SystemRecord> SearchByUser(string SystemRecordId, string SearchItem)
        {
            throw new NotImplementedException();
        }
    }
}
