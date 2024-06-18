using StockTaking.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StockTaking
{
    public partial class MainPage : ContentPage
    {
        LoginViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();
            viewModel = new LoginViewModel();
            BindingContext = viewModel;
        }
    }
}
