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
    public sealed partial class Tour_Store_View_Page : Page
    {

        List<StoreListing> sl = new List<StoreListing>();

        private StoreListing rec;
        // private Tour n;
        private IMobileServiceTable<Scrap> Table = App.MobileService.GetTable<Scrap>();
        private MobileServiceCollection<Scrap, Scrap> items;
        private IMobileServiceTable<User> Table2 = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items2;
        public Tour_Store_View_Page()
        {
            this.InitializeComponent();
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            rec = new StoreListing();

            rec = e.Parameter as StoreListing;
            Title.Text = rec.Title;
            Cover.Source = rec.Image;
            FullCost.Text = "Tour Price: " + rec.Price;
            string[] ids = rec.MyId.Split(',');
            try
            {
                foreach (string nid in ids)
                {
                    if (nid != "")
                    {
                        rec = new StoreListing();
                        items = await Table.Where(Scrap
                             => Scrap.Id == nid).ToCollectionAsync();
                        rec.Id = items[0].Id;
                        rec.Title = items[0].Title;
                        rec.MyId = items[0].Point_List;
                        rec.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(this.BaseUri, "Assets/augmented-reality-for-blog.jpg")); // image fromasset store
                        sl.Add(rec);
                    }
                }
                StoreListView.DataContext = sl;
            }
            catch (Exception)
            {

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
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                items2 = await Table2.Where(User
                    => User.Id == App.userId).ToCollectionAsync();
                User a = items2[0];
                if (!a.TourPurchases.Contains(rec.Id))
                {
                    if (a.wallet > int.Parse(rec.Price))
                    {
                        a.TourPurchases += "," + rec.Id;
                        a.wallet = a.wallet - int.Parse(rec.Price);
                        await Table2.UpdateAsync(a);
                    }
                    MessageDialog mess = new MessageDialog("Purchased successful");
                    await mess.ShowAsync();
                }
                // buy button
            }
            catch (Exception)
            {
                MessageDialog mess = new MessageDialog("Can't purchase");
                await mess.ShowAsync();
            }
        }
    }
}
