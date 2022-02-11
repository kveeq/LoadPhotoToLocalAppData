using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoadPhotoToLocalAppData.db
{
    public class CRUDOperation
    {
        SQLiteConnection db;
        public CRUDOperation(string dbPath)
        {
            db = new SQLiteConnection(dbPath);
            db.CreateTable<CarModel>();
        }
        public IEnumerable<CarModel> GetItems()
        {
            return db.Table<CarModel>().ToList();
        }

        public CarModel GetItem(int id)
        {
            return db.Get<CarModel>(id);
        }

        public int DeleteItem(int id)
        {
            return db.Delete<CarModel>(id);
        }
        public int SaveItem(CarModel toDItem)
        {
            if (toDItem.Id != 0)
            {
                db.Update(toDItem);
                return toDItem.Id;
            }
            else
            {
                return db.Insert(toDItem);
            }
        }
    }
}
