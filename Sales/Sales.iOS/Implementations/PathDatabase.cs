[assembly: Xamarin.Forms.Dependency(typeof(Sales.iOS.Implementations.PathDatabase))]

namespace Sales.iOS.Implementations
{
    using Interfaces;
    using System;
    using System.IO;

    public class PathDatabase : IPathDatabase
    {
        public string GetDatabasePath()
        {
            string vStrDocFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string vStrLibFolder = Path.Combine(vStrDocFolder, "..", "Library", "Databases");

            if (!Directory.Exists(vStrLibFolder))
            {
                Directory.CreateDirectory(vStrLibFolder);
            }

            return Path.Combine(vStrLibFolder, "Sales.db3");
        }
    }
}