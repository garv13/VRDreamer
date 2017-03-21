using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VRDreamer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string username = " ";
        private IMobileServiceTable<User> Table = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<Scrap> Table1 = App.MobileService.GetTable<Scrap>();
        private MobileServiceCollection<Scrap,Scrap> items1;
        private IMobileServiceTable<Tour> Table2 = App.MobileService.GetTable<Tour>();
        private MobileServiceCollection<Tour,Tour> items2;
        private IMobileServiceTable<Diary> Table3 = App.MobileService.GetTable<Diary>();
        private MobileServiceCollection<Diary,Diary> items3;
        MainPage_Listing m = new MainPage_Listing();
        User u = new User();
        string purchases = " ";
        List<string> Scrap_Purchases = new List<string>();
        List<string> Tour_Purchases = new List<string>();
        List<string> Diary_Purchases = new List<string>();
        List<MainPage_Listing> mpList = new List<MainPage_Listing>();
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                items = await Table.Where(User
                            => User.Name == username).ToCollectionAsync(); // get the username from login page
                purchases = items[0].Purchase_History;// split the string to get all the ids

                foreach (string id in Scrap_Purchases)
                {
                    m = new MainPage_Listing();
                    items1 = await Table1.Where(Scrap
                           => Scrap.Id == id).ToCollectionAsync();

                    m.Id = items1[0].Id;
                    m.Title = items1[0].Title;
                    m.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("some static image")); // some static iage for scrap
                    m.Type = "S";
                    m.color = (Color)ColorConverter.ConvertFromString("#FFDFD991");

                }

            }
            catch (Exception)
            {

            }
        }

        private void StoreListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
