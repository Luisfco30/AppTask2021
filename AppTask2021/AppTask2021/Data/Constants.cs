using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppTask2021.Data
{
    public class Constants
    {
        public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite | 
                                            SQLite.SQLiteOpenFlags.Create | 
                                            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, "AppTask2021.db3");
            }
        }
    }
}
