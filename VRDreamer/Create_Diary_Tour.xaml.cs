using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VRDreamer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Create_Diary_Tour : Page
    {
        public string userid = App.userId;
        private IMobileServiceTable<User> Table = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<Scrap> Table1 = App.MobileService.GetTable<Scrap>();
        private MobileServiceCollection<Scrap, Scrap> items1;
        private IMobileServiceTable<Tour> Table2 = App.MobileService.GetTable<Tour>();
        private MobileServiceCollection<Tour, Tour> items2;
        Purchsed m = new Purchsed();
        User u = new User();
        string purchases = " ";
        List<string> Scrap_Purchases = new List<string>();
        List<string> Tour_Purchases = new List<string>();
        List<Purchsed> Slist = new List<Purchsed>();
        List<Purchsed> Tlist = new List<Purchsed>();
        List<Purchsed> selected = new List<Purchsed>(); 
        public Create_Diary_Tour()
        {
            this.InitializeComponent();
            Loaded += Create_Diary_Tour_Loaded;
        }

        private async void Create_Diary_Tour_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingBar2.Visibility = Visibility.Visible;
                LoadingBar2.IsIndeterminate = true;

                items = await Table.Where(User
                            => User.Id == userid).ToCollectionAsync(); // get the username from login page

                Scrap_Purchases = items[0].ScrapePurchase.Split(',').ToList<string>();
                Tour_Purchases = items[0].TourPurchases.Split(',').ToList<string>();
                
                foreach (string id in Scrap_Purchases)
                {
                    m = new Purchsed();
                    items1 = await Table1.Where(Scrap
                           => Scrap.Id == id).ToCollectionAsync();

                    m.Id = items1[0].Id;
                    m.Title = items1[0].Title;
                    m.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(this.BaseUri, "Assets/augmented-reality-for-blog.jpg")); // some static iage for scrap
                    m.Type = "S";
                    Slist.Add(m);
                }

                foreach (string id in Tour_Purchases)
                {
                    m = new Purchsed();
                    items2 = await Table2.Where(Tour
                           => Tour.Id == id).ToCollectionAsync();

                    m.Id = items2[0].Id;
                    m.Title = items2[0].Title;
                    m.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(items2[0].Cover_Url));
                    m.Type = "T";
                    Tlist.Add(m);
                }
              
                items1 = await Table1.Where(Scrap
                           => Scrap.UserId == userid).ToCollectionAsync();

                foreach (Scrap sr in items1)
                {
                    m = new Purchsed();

                    m.Id = sr.Id;
                    m.Title = sr.Title;
                    m.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(this.BaseUri, "Assets/augmented-reality-for-blog.jpg")); // some static iage for scrap
                    m.Type = "S";
                    Slist.Add(m);
                }

                items2 = await Table2.Where(Tour
                   => Tour.UserId == userid).ToCollectionAsync();
                foreach (Tour sr in items2)
                {
                    m = new Purchsed();

                    m.Id = sr.Id;
                    m.Title = sr.Title;
                    m.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(sr.Cover_Url)); // some static iage for scrap
                    m.Type = "T";
                    Tlist.Add(m);
                }

                LoadingBar2.Visibility = Visibility.Collapsed;
                TourView.DataContext = Tlist;
                ScarpeView.DataContext = Slist;
            }
            catch (Exception)
            {

                MessageDialog msgbox = new MessageDialog("Sorry can't update now");
                await msgbox.ShowAsync();
                LoadingBar2.Visibility = Visibility.Collapsed;
            }
        }
        private void Create_Diary_Botton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Create_Diary_Tour));
        }

        private void About_Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private void Store_Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Store));
        }

        private void Scrap_Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewScrape));
        }

        private void Purchase_Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private async void UploadNew_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int j = 0;
            foreach(Purchsed p in TourView.SelectedItems)
            {
                i++;
                selected.Add(p);
            }

            foreach (Purchsed p in ScarpeView.SelectedItems)
            {
                j++;
                selected.Add(p);
            }

            if(i==0 && j==0)
            {
                MessageDialog msgbox = new MessageDialog("No Item Selected");
                await msgbox.ShowAsync();
                selected.Clear();
                return;
            }

            if (i != 0 && j != 0)
            {
                MessageDialog msgbox = new MessageDialog("Please select only one type of item");
                await msgbox.ShowAsync();
                selected.Clear();
                return;
            }
            Frame.Navigate(typeof(Created_upload_Page), selected);
        }
    }
}
