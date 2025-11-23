using EmployeesManagement.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagement.Data.EF
{
    public class RoleEF : IDataHelper<Role>
    {
        private DBContext db;

        private Role _Role;

        public RoleEF()
        {
            db = new DBContext();
            _Role = new Role();
        }
        public string Add(Role item)
        {
            try
            {
                db.Roles.Add(item);
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
                _Role = Find(id);
                db.Roles.Remove(_Role);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string Edit(Role item)
        {
            try
            {
                db = new DBContext();
                db.Roles.Update(item);
                db.SaveChanges();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Role Find(int id)
        {
            try
            {
                return db.Roles.Find(id)?? new Role();
            }
            catch (Exception ex)
            {
                return new Role();
            }
        }

        public List<Role> GetAllData()
        {
            try
            {
                db = new DBContext();
                return db.Roles.OrderByDescending(x => x.Id).ToList();
            }
            catch
            {
                return new List<Role>();
            }
        }

        public List<Role> GetDataByUser(string userId)
        {
            try
            {
                db = new DBContext();
                return db.Roles.Where(e => e.UserId.ToString() == userId).ToList();
            }
            catch (Exception ex)
            {
                return new List<Role>();
            }
        }

        public List<Role> SearchAll(string searchIteam)
        {
            try
            {
                return db.Roles.Where(x => x.Id.ToString() == searchIteam

                )
                    .OrderByDescending(x => x.Id).ToList();

            }
            catch
            {
                return new List<Role>();
            }
        }

        public List<Role> SearchByUser(string userId, string searchIteam)
        {
            try
            {
                return db.Roles.Where(x => x.UserId.ToString() == userId).Where(x => x.Id.ToString() == searchIteam

                )
                    .OrderByDescending(x => x.Id).ToList();

            }
            catch
            {
                return new List<Role>();
            }
        }
    }
}
