[assembly: Xamarin.Forms.Dependency(typeof(Sales.Droid.Implementations.PathDatabase))]

namespace Sales.Droid.Implementations
{
    using Sales.Interfaces;
    using System;
    using System.IO;

    public class PathDatabase : IPathDatabase
    {
        public string GetDatabasePath()
        {
            string vStrPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(vStrPath, "Sales.db3");
        }
    }
}