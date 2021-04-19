using AppTask2021.Data;
using AppTask2021.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTask2021
{
    public partial class App : Application
    {
        private static SQLiteDatabase _SQLiteDatabase;
        public static SQLiteDatabase SQLiteDatabase
        {
            get
            {
                if (_SQLiteDatabase == null) _SQLiteDatabase = new SQLiteDatabase();
                return _SQLiteDatabase;
            }
        }

        public App()
        {
            InitializeComponent();

            var navPage = new NavigationPage(new TaskListPage());
            navPage.BarBackgroundColor = (Color)App.Current.Resources["Green"];
            navPage.BarTextColor = Color.White;
            MainPage = navPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
