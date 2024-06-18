using StockTaking.DB;
using StockTaking.Interfaces;
using StockTaking.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace StockTaking.ViewModels
{
    public class ScanViewModel : BaseViewModel
    {
        DatabaseController database;
        public ScanViewModel()
        {
            database = new DatabaseController();
           
        }

        private string count;
        public string COUNT
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                NotifyPropertyChanged("COUNT");
            }
        }

        private string totalcount;
        public string TOTALCOUNT
        {
            get
            {
                return totalcount;
            }
            set
            {
                totalcount = value;
                NotifyPropertyChanged("TOTALCOUNT");
            }
        }

        /*private string batch;
        public string BATCH_NO
        {
            get
            {
                return batch;
            }
            set
            {
                batch = value;
                NotifyPropertyChanged("BATCH_NO");
            }
        }*/

        private string article;
        public string ARTICLES
        {
            get
            {
                return article;
            }
            set
            {
                article = value;
                NotifyPropertyChanged("ARTICLES");
            }
        }

        private string dt;
        public string DATETIME
        {
            get
            {
                return dt;
            }
            set
            {
                dt = value;
                NotifyPropertyChanged("DATETIME");
            }
        }

        List<Articles> _objdocketList;

        public List<Articles> ObjDocketList
        {
            get { return _objdocketList; }

            set
            {
                if (_objdocketList != value)
                {
                    _objdocketList = value;
                    // OnPropertyChanged("ObjPaymentList");
                    NotifyPropertyChanged("ObjDocketList");
                }
            }
        }
        public Command SUBMIT
        {
            get
            {
                return new Command(SubmitDetails);
            }
        }

        private async void SubmitDetails(object obj)
        {
            try
            {

                var csvPath = DependencyService.Get<IDBInterface>().GetCSVPath();
               // File.Create(csvPath);
              
                List<Articles> articles = new List<Articles>();
                articles = database.GetAll_Articles();
                //// build the data in memory
                //StringBuilder s = new StringBuilder();
                //foreach (var listing in articles)
                //{
                //    s = s.AppendLine(listing.BATCH_NO + "," + listing.ARTICLES + "," + listing.DATETIME);
                //}

                //// write the data all at once
                // File.WriteAllText(csvPath, s.ToString());
                //s.Clear();
                using (var logStream = new FileStream(csvPath, FileMode.Append, FileAccess.Write, FileShare.Write))
                using (var streamWriter = new StreamWriter(logStream))
                {
                    foreach (var listing in articles)
                    {
                        streamWriter.WriteLine(listing.ARTICLES + "," + listing.COUNT + "," + listing.DATETIME + "," + listing.USER);
                    }

                    streamWriter.Flush();
                    streamWriter.Close();
                    logStream.Close();
                }

                database.Delete_AllArticles();
                ObjDocketList = database.GetAll_Articles();
              
                await App.Current.MainPage.DisplayAlert("Alert", "Data saved in " + csvPath + " location", "Ok");
                TOTALCOUNT = database.GetTotalArticleCount().ToString();


            }
            catch(Exception excp)
            {
                await App.Current.MainPage.DisplayAlert("Exception", excp.Message, "Ok");
            }
        }
    }
}
