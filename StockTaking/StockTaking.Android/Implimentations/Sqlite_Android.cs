﻿using SQLite;
using System.IO;
using Xamarin.Forms;
using StockTaking.Droid.Implimentations;
using StockTaking.Interfaces;

[assembly: Dependency(typeof(Sqlite_Android))]
namespace StockTaking.Droid.Implimentations
{
    public class Sqlite_Android : IDBInterface
    {
        public Sqlite_Android()
        {

        }
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "VXPRESS.db";
            string documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentPath, sqliteFilename);
            var conn = new SQLite.SQLiteConnection(path);
            return conn;
        }

        public string GetCSVPath()
        {
            // var filePath = Path.Combine(Android.OS.Environment.DirectoryDownloads, "stocktake.csv");

            string directory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
            string filePath = Path.Combine(directory, "stocktake.csv");

            return filePath;
        }
    }
}