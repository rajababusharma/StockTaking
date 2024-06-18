using StockTaking.DB;
using StockTaking.Models;
using StockTaking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StockTaking.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Scan : ContentPage
    {
        ScanViewModel viewModel;
        DatabaseController database;
        bool focus_status = false;
        string user = "";
        public Scan()
        {
            InitializeComponent();
            viewModel = new ScanViewModel();
            BindingContext = viewModel;
            database = new DatabaseController();
            entrysku.Completed += Entrysku_Completed;
            entrysku.Unfocused += Entrysku_Unfocused;
            viewModel.ObjDocketList = database.GetAll_Articles();

            txtCount.Text = database.GetTotalArticleCount().ToString();
            user = Preferences.Get(Constants.username,"");
        }

        private void Entrysku_Unfocused(object sender, FocusEventArgs e)
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(2000.0), TimerElapsed);

            bool TimerElapsed()
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!focus_status)
                    {
                        //put here your code 
                        entrysku.CursorPosition = entrysku.Text.Length + 1;
                    }
                    //viewModel.DOCKETNUMBER = "";
                    // entrydocket.Focus();

                });
                //return true to keep timer reccurring
                //return false to stop timer
                return false;
            }
        }

        private void Entrysku_Completed(object sender, EventArgs e)
        {
            ValidateDocket();
        }

        private void ValidateDocket()
        {
            try
            {

                DateTime date = DateTime.Now;
                if (!string.IsNullOrEmpty(entrysku.Text))
                {
                    /* if (entrydocket.Text.Length == 10)
                     {*/
                    viewModel.ARTICLES = entrysku.Text;
                    viewModel.DATETIME = date.ToString();
                    //DisplayAlert("Alert", "Entry text changed", "Ok");

                    int result = database.CheckArticleDuplicate(viewModel.ARTICLES);
                    if (result > 0)
                    {
                        if (!chk_duplicate.IsChecked)
                        {
                            SpeakNow("Already Scanned");
                            int k = database.Update_Articles(viewModel.ARTICLES, viewModel.DATETIME, 1,user);
                            if (k > 0)
                            {
                                viewModel.ObjDocketList = database.GetAll_Articles();

                                entrysku.CursorPosition = 0;
                                viewModel.ARTICLES = "";
                                entrysku.Text = "";
                            }
                        }
                        else
                        {
                            //SpeakNow("Already Scanned");
                            int k = database.Update_Articles(viewModel.ARTICLES, viewModel.DATETIME, result + 1, user);
                            if (k > 0)
                            {
                                viewModel.ObjDocketList = database.GetAll_Articles();

                                entrysku.CursorPosition = 0;
                                viewModel.ARTICLES = "";
                                entrysku.Text = "";
                            }
                        }

                    }
                    else
                    {
                        List<Articles> mylist = new List<Articles>();
                        Articles articles = new Articles();
                        articles.ARTICLES = viewModel.ARTICLES;
                        articles.DATETIME = viewModel.DATETIME;
                        articles.COUNT = 1;
                        articles.USER = user;
                        mylist.Add(articles);
                        int p = database.Insert_IntoARTICLES(mylist);
                        if (p > 0)
                        {
                            viewModel.ObjDocketList = database.GetAll_Articles();

                            entrysku.CursorPosition = 0;
                            viewModel.ARTICLES = "";
                            entrysku.Text = "";
                        }


                    }
                    entrysku.CursorPosition = 0;
                    viewModel.ARTICLES = "";
                    entrysku.Text = "";

                }
                else
                {

                    entrysku.CursorPosition = 0;
                    viewModel.ARTICLES = "";
                    entrysku.Text = "";
                }
                txtCount.Text = database.GetTotalArticleCount().ToString();

            }
            catch (Exception excp)
            {

                entrysku.CursorPosition = 0;
                viewModel.ARTICLES = "";
                entrysku.Text = "";

            }
        }
        public static void SpeakNow(string textTospeak)
        {
            var settings = new SpeechOptions()
            {
                Volume = 1.0f,
                Pitch = 1.0f
            };

            TextToSpeech.SpeakAsync(textTospeak, settings);
        }
    }
}