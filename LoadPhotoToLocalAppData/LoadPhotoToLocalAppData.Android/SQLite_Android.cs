using Xamarin.Forms;
using System;
using System.IO;
using LoadPhotoToLocalAppData.Droid;
using LoadPhotoToLocalAppData.db;

[assembly: Dependency(typeof(SQLite_Android))]
namespace LoadPhotoToLocalAppData.Droid
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android() { }
        public string GetDatabasePath(string sqlfilename)
        {
            string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentPath, sqlfilename);

            return path;
        }
    }
}