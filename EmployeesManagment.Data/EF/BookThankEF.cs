using EmployeesManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesManagement.Data.EF
{
    public class BookThankEF : IDataHelper<Core.BookThank>
    {
        private DBContext db;

        private BookThank _BookThank;

        public BookThankEF()
        {
            db = new DBContext();
            _BookThank = new BookThank();
        }
        public string Add(BookThank item)
        {
            try
            {
                db.BookThanks.Add(item);
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
                _BookThank = Find(id);
                db.BookThanks.Remove(_BookThank);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string Edit(BookThank item)
        {
            try
            {
                db = new DBContext();
                db.BookThanks.Update(item);
                db.SaveChanges();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public BookThank Find(int id)
        {
            try
            {
                return db.BookThanks.Find(id) ?? new BookThank();
            }
            catch (Exception ex)
            {
                return new BookThank();
            }
        }

        public List<BookThank> GetAllData()
        {
            try
            {
                db = new DBContext();
                return db.BookThanks.OrderByDescending(x => x.Id).ToList();
            }
            catch
            {
                return new List<BookThank>();
            }
        }

        public List<BookThank> GetDataByUser(string userId)
        {
            try
            {
                db = new DBContext();
                return db.BookThanks.Where(e => e.UserId.ToString() == userId).ToList();
            }
            catch (Exception ex)
            {
                return new List<BookThank>();
            }
        }

        public List<BookThank> SearchAll(string searchIteam)
        {
            try
            {
                return db.BookThanks.Where(x => x.Id.ToString() == searchIteam ||
                x.UserId == searchIteam ||
                x.AddedDate.ToString() == searchIteam ||
                x.BookThankDate.ToString() == searchIteam ||
                x.Ref.ToString() == searchIteam ||
                x.Note.Contains(searchIteam)

                )
                    .OrderByDescending(x => x.Id).ToList();

            }
            catch
            {
                return new List<BookThank>();
            }
        }

        public List<BookThank> SearchByUser(string userId, string searchIteam)
        {
            try
            {
                return db.BookThanks.Where(x => x.UserId == userId).Where(x => x.Id.ToString() == searchIteam ||
              x.UserId == searchIteam ||
                x.AddedDate.ToString() == searchIteam ||
                x.BookThankDate.ToString() == searchIteam ||
                x.Ref.ToString() == searchIteam ||
                x.Note.Contains(searchIteam)
                )
                    .OrderByDescending(x => x.Id).ToList();

            }
            catch
            {
                return new List<BookThank>();
            }
        }

    }
}
