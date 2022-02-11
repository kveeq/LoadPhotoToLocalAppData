using System;
using System.Collections.Generic;
using System.Text;

namespace LoadPhotoToLocalAppData.db
{
    public interface ISQLite
    {
        string GetDatabasePath(string filename);
    }
}
