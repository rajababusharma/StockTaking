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
       
        public Scan()
        {
            InitializeComponent();
            viewModel = new ScanViewModel();
            BindingContext = viewModel;
            database = new DatabaseController();
            entrysku.Completed += Entrysku_Completed;
            entrysku.Unfocused += Entrysku_Unfocused;
           
            viewModel.USER = Preferences.Get(Constants.username,"");
            Device.BeginInvokeOnMainThread( () =>
            {
                // entrysku.CursorPosition = entrysku.Text.Length + 1;
               // entrysku.Focus();
                viewModel.ObjDocketList = database.GetAll_Articles();
                viewModel.TOTALCOUNT = database.GetTotalArticleCount().ToString();
            });
        }

        private void Entrysku_Unfocused(object sender, FocusEventArgs e)
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(5000.0), TimerElapsed);

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

        private async void Entrysku_Completed(object sender, EventArgs e)
        {
           await ValidateDocket();
        }

        private async Task ValidateDocket()
        {
            try
            {
                focus_status = true;
                DateTime date = DateTime.Now;
                if (!string.IsNullOrEmpty(entrysku.Text))
                {
                    /* if (entrydocket.Text.Length == 10)
                     {*/
                    viewModel.SKU = entrysku.Text;
                    viewModel.DATETIME = date.ToString();
                    //DisplayAlert("Alert", "Entry text changed", "Ok");

                    int result = database.CheckArticleDuplicate(viewModel.SKU);
                    if (result > 0)
                    {
                        if (!chk_duplicate.IsChecked)
                        {
                            SpeakNow("Already Scanned");
                            int k = database.Update_Articles(viewModel.SKU, viewModel.DATETIME, 1, viewModel.USER);
                            if (k > 0)
                            {
                               

                               // entrysku.CursorPosition = 0;
                              
                               
                                Device.BeginInvokeOnMainThread(() => {
                                    entrysku.CursorPosition = entrysku.Text.Length + 1;
                                     entrysku.Text = "";
                                    viewModel.SKU = "";
                                    viewModel.ObjDocketList = database.GetAll_Articles();
                                    focus_status = false;
                                });

                            }
                        }
                        else
                        {
                            //SpeakNow("Already Scanned");
                            int k = database.Update_Articles(viewModel.SKU, viewModel.DATETIME, result + 1, viewModel.USER);
                            if (k > 0)
                            {
                               

                                Device.BeginInvokeOnMainThread(() => {
                                    entrysku.CursorPosition = entrysku.Text.Length + 1;
                                    entrysku.Text = "";
                                    viewModel.SKU = "";
                                    viewModel.ObjDocketList = database.GetAll_Articles();
                                    focus_status = false;
                                });
                            }
                        }

                    }
                    else
                    {
                        List<Articles> mylist = new List<Articles>();
                        Articles articles = new Articles();
                        articles.ARTICLES = viewModel.SKU;
                        articles.DATETIME = viewModel.DATETIME;
                        articles.COUNT = 1;
                        articles.USER = viewModel.USER;
                        mylist.Add(articles);
                        int p = database.Insert_IntoARTICLES(mylist);
                        if (p > 0)
                        {
                            Device.BeginInvokeOnMainThread(() => {
                                entrysku.CursorPosition = entrysku.Text.Length + 1;
                                entrysku.Text = "";
                                viewModel.SKU = "";
                                viewModel.ObjDocketList = database.GetAll_Articles();
                                focus_status = false;
                            });
                        }


                    }
                   /* Device.BeginInvokeOnMainThread(() => {
                        entrysku.CursorPosition = entrysku.Text.Length + 1;
                        entrysku.Text = "";
                        viewModel.SKU = "";
                       // viewModel.ObjDocketList = database.GetAll_Articles();
                    });*/

                }
                else
                {

                    Device.BeginInvokeOnMainThread(() => {
                        entrysku.CursorPosition = entrysku.Text.Length + 1;
                        entrysku.Text = "";
                        viewModel.SKU = "";
                        // viewModel.ObjDocketList = database.GetAll_Articles();
                        focus_status = false;
                    });
                }
               
                Device.BeginInvokeOnMainThread(() => {
                    viewModel.TOTALCOUNT = database.GetTotalArticleCount().ToString();
                });
            }
            catch (Exception excp)
            {

                Device.BeginInvokeOnMainThread(async () => {
                    entrysku.CursorPosition = entrysku.Text.Length + 1;
                    entrysku.Text = "";
                    viewModel.SKU = "";
                    focus_status = false;
                    await DisplayAlert("Alert", excp.Message, "OK");

                    // viewModel.ObjDocketList = database.GetAll_Articles();
                });

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