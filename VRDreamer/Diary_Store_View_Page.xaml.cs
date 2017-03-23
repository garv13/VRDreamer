using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Diary_Store_View_Page : Page
    {


        List<StoreListing> sl = new List<StoreListing>();

        private StoreListing rec;
        private Tour n;
        private IMobileServiceTable<Tour> Table = App.MobileService.GetTable<Tour>();
        private MobileServiceCollection<Tour,Tour> items;
        public Diary_Store_View_Page()
        {
            this.InitializeComponent();
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            rec = new StoreListing();

            rec = e.Parameter as StoreListing;
            Title.Text = rec.Title;
            Cover.Source = rec.Image;
            FullCost.Text = "Diary Price: " + rec.Price;
            string[] ids = rec.MyId.Split(',');
            try
            {
                foreach (string nid in ids)
                {
                    if (nid != "")
                    {
                        rec = new StoreListing();
                        items = await Table.Where(Tour
                             => Tour.Id == nid).ToCollectionAsync();
                        rec.Id = items[0].Id;
                        rec.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(items[0].Cover_Url)); // image fromasset store
                        sl.Add(rec);
                    }
                }
                StoreListView.DataContext = sl;
            }
            catch (Exception)
            {

            }
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void MainPage_Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Canvas_Button_Click(object sender, RoutedEventArgs e)
        {
            //  Frame.Navigate(typeof(CanvasPage));
        }

        private void Store_Button_Click(object sender, RoutedEventArgs e)
        {
            // Frame.Navigate(typeof(StorePage));
        }

        private void Notes_Botton_Click(object sender, RoutedEventArgs e)
        {
            // Frame.Navigate(typeof(MyNotesPage));
        }

        private void About_Button_Click(object sender, RoutedEventArgs e)
        {
            // Frame.Navigate(typeof(AboutPage));
        }

        private void SignOut_Button_Click(object sender, RoutedEventArgs e)
        {
            // Frame.Navigate(typeof(SignUp));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
