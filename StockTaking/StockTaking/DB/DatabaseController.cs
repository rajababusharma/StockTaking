using SQLite;
using StockTaking.Interfaces;
using StockTaking.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StockTaking.DB
{
    public class DatabaseController
    {
        private SQLiteConnection database;
        private static object collisionLock = new object();
        public DatabaseController()
        {
            database = DependencyService.Get<IDBInterface>().GetConnection();
            database.CreateTable<Articles>();
            database.CreateTable<Users>();
        }
        public int Insert_IntoUsers(Users user)
        {
            int status = 0;
            try
            {
                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    lock (collisionLock)
                    {
                        // DeleteAll_from_DocketDetList();
                        status = dbConn.InsertOrReplace(user);
                        dbConn.Commit();
                        dbConn.Dispose();
                        dbConn.Close();
                    }
                }

                return status;
            }

            catch (SQLiteException excp)
            {

                return status;
            }
        }
        public int Insert_IntoARTICLES(List<Articles> Mylist)
        {
            int status = 0;
            try
            {
                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    lock (collisionLock)
                    {
                        // DeleteAll_from_DocketDetList();
                        status = dbConn.InsertAll(Mylist);
                        dbConn.Commit();
                        dbConn.Dispose();
                        dbConn.Close();
                    }
                }

                return status;
            }

            catch (SQLiteException excp)
            {
             
                return status;
            }
        }

        public int Update_Articles(string article,string dt,int count,string user)
        {

          
            int status = 0;
            try
            {
                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    lock (collisionLock)
                    {
                        // DeleteAll_from_DocketDetList();
                        //  status = dbConn.Execute("Update DocketDetList SET Scanned_status='" + setstatus + "'  where Docket_No='" + docket + "'");
                        status = dbConn.Execute("Update Articles SET USER=" + "'" + user + "',COUNT=" + "'" + count + "', DATETIME=" + "'" + dt + "' where ARTICLES=" + "'" + article + "'");
                        dbConn.Commit();
                        dbConn.Dispose();
                        dbConn.Close();
                    }
                }

                return status;
            }
            catch (SQLiteException ex)
            {
               
                return status;
            }
        }

        public List<Articles> GetAll_Articles()
        {
            List<Articles> Mylist = null;
           
            try
            {
                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    lock (collisionLock)
                    {
                        Mylist = new List<Articles>();
                        // Mylist = dbConn.Query<DocketDetList>("Select Docket_No,Articles,Scanned_status From DocketDetList");
                        // Mylist = dbConn.Table<DocketDetList_Actualloading>().ToList();
                        // Mylist = dbConn.Query<DocketDetList_Actualloading>("select * from DocketDetList_Actualloading order by Scanned_Article ASC");
                        Mylist = dbConn.Query<Articles>("select * from Articles order by DATETIME DESC");
                        dbConn.Dispose();
                        dbConn.Close();
                    }
                }

                return Mylist;
            }
            catch (SQLiteException ex)
            {
              
                return Mylist;
            }

        }
        public void Delete_AllArticles()
        {
            using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
            {
                lock (collisionLock)
                {
                    //dbConn.RunInTransaction(() =>   
                    //   {   
                    dbConn.DropTable<Articles>();
                    dbConn.CreateTable<Articles>();
                    dbConn.Dispose();
                    dbConn.Close();
                }
                //});   
            }
        }
        public int CheckArticleDuplicate(string article)
        {
            int flag = 0;
            List<Articles> Mylist = null;
            try
            {
                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    lock (collisionLock)
                    {
                        // Mylist = new List<AUSAddShortAccessEnitity>();
                        // Mylist = database.Query<DocketDetList>("Select Docket_No,Articles,Scanned_status From DocketDetList");
                        //var existingItem = dbConn.Get<DocketDetList>(dock);
                        // Mylist = dbConn.Query<ArticlesLoading>("Select * From ArticlesLoading where ARTICLE_ID=" + "'" + article + "'");
                        Mylist = dbConn.Query<Articles>("Select * From Articles where ARTICLES=" + "'" + article + "'");
                        if (Mylist.Count > 0)
                        {
                            flag = Mylist.Count;
                        }
                        dbConn.Dispose();
                        dbConn.Close();
                    }
                }
                return flag;
            }
            catch (SQLiteException excp)
            {
                
                return flag;
            }

        }
        public int Authenticate(Users user)
        {
            int flag = 0;
            List<Users> Mylist = null;
            try
            {
                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    lock (collisionLock)
                    {
                        // Mylist = new List<AUSAddShortAccessEnitity>();
                        // Mylist = database.Query<DocketDetList>("Select Docket_No,Articles,Scanned_status From DocketDetList");
                        //var existingItem = dbConn.Get<DocketDetList>(dock);
                        // Mylist = dbConn.Query<ArticlesLoading>("Select * From ArticlesLoading where ARTICLE_ID=" + "'" + article + "'");
                        Mylist = dbConn.Query<Users>("Select * From Users where username=" + "'" + user.username + "' and password=" + "'" + user.password + "'");
                        if (Mylist.Count > 0)
                        {
                           
                                flag = 1;
                        }
                        dbConn.Dispose();
                        dbConn.Close();
                    }
                }
                return flag;
            }
            catch (SQLiteException excp)
            {

                return flag;
            }

        }
        public int GetTotalArticleCount()
        {
            int flag = 0;
            List<Articles> Mylist = null;
            try
            {
                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    lock (collisionLock)
                    {
                        // Mylist = new List<AUSAddShortAccessEnitity>();
                        // Mylist = database.Query<DocketDetList>("Select Docket_No,Articles,Scanned_status From DocketDetList");
                        //var existingItem = dbConn.Get<DocketDetList>(dock);
                        // Mylist = dbConn.Query<ArticlesLoading>("Select * From ArticlesLoading where ARTICLE_ID=" + "'" + article + "'");
                        Mylist = dbConn.Query<Articles>("Select * From Articles");
                        if (Mylist.Count > 0)
                        {
                            flag = Mylist.Count;
                        }
                        dbConn.Dispose();
                        dbConn.Close();
                    }
                }
                return flag;
            }
            catch (SQLiteException excp)
            {

                return flag;
            }

        }
        public List<Users> GetAll_Users()
        {
            List<Users> Mylist = null;

            try
            {
                using (var dbConn = DependencyService.Get<IDBInterface>().GetConnection())
                {
                    lock (collisionLock)
                    {
                        Mylist = new List<Users>();
                        // Mylist = dbConn.Query<DocketDetList>("Select Docket_No,Articles,Scanned_status From DocketDetList");
                        // Mylist = dbConn.Table<DocketDetList_Actualloading>().ToList();
                        // Mylist = dbConn.Query<DocketDetList_Actualloading>("select * from DocketDetList_Actualloading order by Scanned_Article ASC");
                        Mylist =  dbConn.Query<Users>("select * from Users");
                        dbConn.Dispose();
                        dbConn.Close();
                    }
                }

                return Mylist;
            }
            catch (SQLiteException ex)
            {

                return Mylist;
            }

        }
    }
}
