using EmployeesManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagement.Data.EF
{
    public class UserEF : IDataHelper<User>
    {
        private DBContext db;

        private User user;

        public UserEF()
        {
            db = new DBContext();
            user = new User();
        }
        public string Add(User item)
        {
            try
            {
                db.Users.Add(item);
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
                user = Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Edit(User item)
        {
            try
            {
                db.Users.Update(item);
                db.SaveChanges();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public User Find(int id)
        {
            try
            {
                return db.Users.Find(id)?? new User();
            }
            catch (Exception ex)
            {
                return new User();
            }
        }

        public List<User> GetAllData()
        {
            try
            {
                db = new DBContext();
                return db.Users.ToList();
            }
            catch (Exception ex)
            {
                return new List<User>();
            }
        }

        public List<User> GetDataByUser(string userId)
        {
            try
            {
                db = new DBContext();
                return db.Users.Where(e => e.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                return new List<User>();
            }
        }

        public List<User> SearchAll(string searchIteam)
        {
            try
            {
                return db.Users.Where(x => x.Id.ToString() == searchIteam ||
                x.UserId == searchIteam ||
                x.Address.Contains(searchIteam) ||
                x.Email.Contains(searchIteam) ||
                x.FullName.Contains(searchIteam) ||
                x.UserName.Contains(searchIteam) ||
                x.Role.Contains(searchIteam) ||
                x.CreatedDate.ToString().Contains(searchIteam) ||
                x.EditedDate.ToString().Contains(searchIteam)
                )
                    .OrderByDescending(x => x.Id).ToList();

            }
            catch
            {
                return new List<User>();
            }
        }

        public List<User> SearchByUser(string userId, string searchIteam)
        {
            try
            {
                return db.Users.Where(x => x.UserId == userId).Where(x => x.Id.ToString() == searchIteam ||
                x.UserId == searchIteam ||
                x.Address.Contains(searchIteam) ||
                x.Email.Contains(searchIteam) ||
                x.FullName.Contains(searchIteam) ||
                x.UserName.Contains(searchIteam) ||
                x.Role.Contains(searchIteam) ||
                x.CreatedDate.ToString().Contains(searchIteam) ||
                x.EditedDate.ToString().Contains(searchIteam)
                )
                    .OrderByDescending(x => x.Id).ToList();

            }
            catch
            {
                return new List<User>();
            }
        }
    }
}
