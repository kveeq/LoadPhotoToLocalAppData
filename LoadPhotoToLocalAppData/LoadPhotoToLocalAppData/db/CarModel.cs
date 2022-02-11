using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace LoadPhotoToLocalAppData
{
    [Table("Car")]
    public class CarModel
    {
        public CarModel()
        {

        }

        public CarModel(string path, string title)
        {
            Path = path;
            Title = title;
        }

        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Path { get; set; }
        [Unique]
        public string Title { get; set; }
    }
}
