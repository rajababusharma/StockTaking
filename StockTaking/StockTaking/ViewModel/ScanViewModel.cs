using MigraDocCore.DocumentObjectModel;
using MigraDocCore.Rendering;
using PdfSharpCore.Fonts;
using StockTaking.DB;
using StockTaking.Interfaces;
using StockTaking.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using static System.Collections.Specialized.BitVector32;
using Section = MigraDocCore.DocumentObjectModel.Section;

namespace StockTaking.ViewModels
{
    public class ScanViewModel : BaseViewModel
    {
        DatabaseController database;
        public ScanViewModel()
        {
            database = new DatabaseController();
            GlobalFontSettings.FontResolver = new FileFontResolver();

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

        private string _user;
        public string USER
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                NotifyPropertyChanged("USER");
            }
        }

        private string article;
        public string SKU
        {
            get
            {
                return article;
            }
            set
            {
                article = value;
                NotifyPropertyChanged("SKU");
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
                return new Command(SubmitDetailsPdf1);
            }
        }

        public async void SubmitDetailsPdf1()
        {
            try
            {

                var pdfPath = DependencyService.Get<IDBInterface>().GetPDFPath();



               // string[] lines = ARTICLES.Split('\n');
                Document doc = new Document();
                Section section = doc.AddSection();

                var font = new MigraDocCore.DocumentObjectModel.Font("Verdana", 12);
                var HeaderFont = new MigraDocCore.DocumentObjectModel.Font("Verdana", 16);
                MigraDocCore.DocumentObjectModel.Style style = doc.Styles["Normal"];
                style.Font.Name = "Verdana";


                //just font arrangements as you wish
                // MigraDoc.DocumentObjectModel.Font font = new Font("Times New Roman", 15);
                font.Bold = false;
                HeaderFont.Bold = true;

                Paragraph paragraph = section.AddParagraph();
                paragraph.AddFormattedText("GRN Report", HeaderFont);
                paragraph.AddLineBreak();
               
                //add each line to pdf 
                List<Articles> articles = new List<Articles>();
                articles = database.GetAll_Articles();
          
                int i=1;
                foreach (var line in articles)
                {
                    Paragraph para = section.AddParagraph();
                    para.AddFormattedText(i.ToString()+":", HeaderFont);

                    Paragraph para1 = section.AddParagraph();
                    para1.AddFormattedText(line.ARTICLES, font);

                    Paragraph para2 = section.AddParagraph();
                    para2.AddFormattedText("User: "+line.USER, font);

                    Paragraph para3 = section.AddParagraph();
                    para3.AddFormattedText("Date: "+line.DATETIME, font);

                    para3.AddLineBreak();
                    para3.AddLineBreak();
                    i++;
                }

                //save pdf document
                PdfDocumentRenderer renderer = new PdfDocumentRenderer();
                renderer.Document = doc;
                renderer.RenderDocument();
                renderer.Save(pdfPath);
                

                database.Delete_AllArticles();
                ObjDocketList = database.GetAll_Articles();

               
                TOTALCOUNT = database.GetTotalArticleCount().ToString();

                await App.Current.MainPage.DisplayAlert("Alert", "Data saved in " + pdfPath + " location", "Ok");
                SKU = "";
                // Process.Start(pdfPath);
                if (pdfPath != null)
                {
                    await Launcher.OpenAsync(new OpenFileRequest
                    {
                        File = new ReadOnlyFile(pdfPath)
                    });
                }
            }
            catch (Exception excp)
            {
                await App.Current.MainPage.DisplayAlert("Exception", excp.Message, "Ok");
            }
        }


        /*private async void SubmitDetails(object obj)
        {
            try
            {

                var PdfPath = DependencyService.Get<IDBInterface>().GetPDFPath();
               // File.Create(csvPath);
              
                List<Articles> articles = new List<Articles>();
                articles = database.GetAll_Articles();
               
               
              
                await App.Current.MainPage.DisplayAlert("Alert", "Data saved in " + csvPath + " location", "Ok");
                TOTALCOUNT = database.GetTotalArticleCount().ToString();


            }
            catch(Exception excp)
            {
                await App.Current.MainPage.DisplayAlert("Exception", excp.Message, "Ok");
            }
        }*/
    }
}
