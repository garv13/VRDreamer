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
        public string userid = " ";
        private IMobileServiceTable<User> Table = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<Scrap> Table1 = App.MobileService.GetTable<Scrap>();
        private MobileServiceCollection<Scrap,Scrap> items1;
        private IMobileServiceTable<Tour> Table2 = App.MobileService.GetTable<Tour>();
        private MobileServiceCollection<Tour,Tour> items2;
        private IMobileServiceTable<Diary> Table3 = App.MobileService.GetTable<Diary>();
        private MobileServiceCollection<Diary,Diary> items3;
        Purchsed m = new Purchsed();
        User u = new User();
        string purchases = " ";
        List<string> Scrap_Purchases = new List<string>();
        List<string> Tour_Purchases = new List<string>();
        List<string> Diary_Purchases = new List<string>();
        List<Purchsed> Slist = new List<Purchsed>();
        List<Purchsed> Tlist = new List<Purchsed>();
        List<Purchsed> Dlist = new List<Purchsed>();

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
                            => User.Id == userid).ToCollectionAsync(); // get the username from login page
                purchases = items[0].Purchases;// split the string to get all the ids

                foreach (string id in Scrap_Purchases)
                {
                    m = new Purchsed();
                    items1 = await Table1.Where(Scrap
                           => Scrap.Id == id).ToCollectionAsync();

                    m.Id = items1[0].Id;
                    m.Title = items1[0].Title;
                    m.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("some static image")); // some static iage for scrap
                    m.Type = "S";
                    Slist.Add(m);
                }

                foreach (string id in Tour_Purchases)
                {
                    m = new Purchsed();
                    items2 = await Table2.Where(Tour
                           =>Tour.Id == id).ToCollectionAsync();

                    m.Id = items2[0].Id;
                    m.Title = items2[0].Title;
                    m.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(items2[0].Cover_Url));
                    m.Type = "T";
                    Tlist.Add(m);
                }
                foreach (string id in Diary_Purchases)
                {
                    m = new Purchsed();
                    items3 = await Table3.Where(Diary
                           => Diary.Id == id).ToCollectionAsync();

                    m.Id = items3[0].Id;
                    m.Title = items3[0].Title;
                    m.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(items3[0].Cover_Url)); 
                    m.Type = "D";
                    Dlist.Add(m);
                }
                DiaryView.DataContext = Dlist;
                TourView.DataContext = Tlist;
                ScarpeView.DataContext = Slist;
            }
            catch (Exception)
            {

            }
        }

        private void NextBar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewScrape));
        }

        private void ColorSelect_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Store));
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Grid g = new Grid();
            g = sender as Grid;
            FrameworkElement type = null;
            FrameworkElement id = null;
            foreach (FrameworkElement child in g.Children)
            {
                if ((Grid.GetRow(child) == 2) && (Grid.GetColumn(child) == 1))
                {
                    Border b = child as Border;

                    id = b.Child as FrameworkElement;
                }


                if ((Grid.GetRow(child) == 1) && (Grid.GetColumn(child) == 0))
                {
                    Border b = child as Border;

                    type = b.Child as FrameworkElement;
                }
            }

            TextBlock t = id as TextBlock;
            TextBlock t2 = type as TextBlock;

            if (t2.Text == "S")
                Frame.Navigate(typeof(ViewScrape), t.Text);
            else if (t2.Text == "D")
                Frame.Navigate(typeof(DiaryViewer_Page), t.Text);
            else if (t2.Text == "T")
                Frame.Navigate(typeof(TourViewer_Page), t.Text);
        }
    }
}
