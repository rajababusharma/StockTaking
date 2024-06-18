using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockTaking.Interfaces
{
    public interface IDBInterface
    {
        SQLiteConnection GetConnection();
        string GetCSVPath();
    }
}
