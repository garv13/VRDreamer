using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VRDreamer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public static class Extensions
    {
        public static bool CaseInsensitiveContains(this string text, string value,
            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return text.IndexOf(value, stringComparison) >= 0;
        }
    }
    public sealed partial class Store : Page
    {

        private IMobileServiceTable<Scrap> Table1 = App.MobileService.GetTable<Scrap>();
        private MobileServiceCollection<Scrap, Scrap> items1;
        private IMobileServiceTable<Tour> Table2 = App.MobileService.GetTable<Tour>();
        private MobileServiceCollection<Tour, Tour> items2;
        private IMobileServiceTable<Diary> Table3 = App.MobileService.GetTable<Diary>();
        private MobileServiceCollection<Diary, Diary> items3;
        StoreListing s = new StoreListing();
        string[] words;
        ObservableCollection<StoreListing> Slist = new ObservableCollection<StoreListing>();
        ObservableCollection<StoreListing> Tlist = new ObservableCollection<StoreListing>();
        ObservableCollection<StoreListing> Dlist = new ObservableCollection<StoreListing>();
        public Store()
        {
            this.InitializeComponent();
            Loaded += Store_Loaded;
        }
      
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string search = e.Parameter as string;
            if (search != null)
            {
                await search_Func(search);
            }       
        }
        private async void Store_Loaded(object sender, RoutedEventArgs e)
        {
            Tlist.Clear();
            Dlist.Clear();
            LoadingBar2.Visibility = Visibility.Visible;
            LoadingBar2.IsActive = true;

            try
            {
                //items1 = await Table1.ToCollectionAsync();
                items2 = await Table2.ToCollectionAsync();
                items3 = await Table3.ToCollectionAsync();

                //foreach (Scrap si in items1)
                //{
                //    s = new StoreListing();
                //    s.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(this.BaseUri, "Assets/augmented-reality-for-blog.jpg")); // some static iage for scrap
                //    s.Price = "Price: " + "Free";
                //    s.Title = si.Title;
                //    s.Id = si.Id;
                //    s.MyId = si.Point_List;
                //    s.Type = "S";
                //    Slist.Add(s);
                //}

                foreach (Tour si in items2)
                {
                    s = new StoreListing();
                    s.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(si.Cover_Url)); // some static iage for scrap
                    s.Price = "Price: " + si.Price.ToString();
                    s.Title = si.Title;
                    s.MyId = si.Scrap_List;
                    s.UserId = si.UserId;
                    s.Id = si.Id;
                    s.Type = "T";
                    Tlist.Add(s);
                }

                foreach (Diary si in items3)
                {
                    
                    s = new StoreListing();
                    s.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(si.Cover_Url)); // some static iage for scrap
                    s.Price = "Price: " + si.Price.ToString();
                    s.Title = si.Title;
                    s.Id = si.Id;
                    s.UserId = si.UserId;
                    s.MyId = si.Tour_List;
                    s.Type = "D";
                    Dlist.Add(s);
                }
                LoadingBar2.IsActive = false;
                LoadingBar2.Visibility = Visibility.Collapsed;
                DiaryView.DataContext = Dlist;
                TourView.DataContext = Tlist;
                //ScarpeView.DataContext = Slist;
            }
            catch(Exception)
            {
                MessageDialog msgbox = new MessageDialog("Sorry can't update now");
                await msgbox.ShowAsync();
                LoadingBar2.Visibility = Visibility.Collapsed;
            }
        }

        //private void SearchButton3_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private async void SearchButton2_Click(object sender, RoutedEventArgs e)
        {
            await search_Func(Box2.Text);

        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            await search_Func(Box.Text);
        }

        private async Task search_Func(string str1)
        {
            try
            {
                words = str1.Split(' ');
                LoadingBar2.Visibility = Visibility.Visible;
                LoadingBar2.IsActive = true;
                Tlist.Clear();
                Dlist.Clear();
                items2 = await Table2.ToCollectionAsync();
                items3 = await Table3.ToCollectionAsync();
                foreach (Tour si in items2)
                {
                    foreach (string str in words)
                    {
                        if (si.Title.CaseInsensitiveContains(str))
                        {
                            s = new StoreListing();
                            s.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(si.Cover_Url)); // some static iage for scrap
                            s.Price = "Price: " + si.Price.ToString();
                            s.Title = si.Title;
                            s.MyId = si.Scrap_List;
                            s.UserId = si.UserId;
                            s.Id = si.Id;
                            s.Type = "T";
                            Tlist.Add(s);
                        }
                        else if (si.Tags != null && si.Tags.CaseInsensitiveContains(str))
                        {
                            s = new StoreListing();
                            s.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(si.Cover_Url)); // some static iage for scrap
                            s.Price = "Price: " + si.Price.ToString();
                            s.Title = si.Title;
                            s.MyId = si.Scrap_List;
                            s.UserId = si.UserId;
                            s.Id = si.Id;
                            s.Type = "T";
                            Tlist.Add(s);
                        }
                        else if (si.Desc != null && si.Desc.CaseInsensitiveContains(str))
                        {
                            s = new StoreListing();
                            s.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(si.Cover_Url)); // some static iage for scrap
                            s.Price = "Price: " + si.Price.ToString();
                            s.Title = si.Title;
                            s.MyId = si.Scrap_List;
                            s.UserId = si.UserId;
                            s.Id = si.Id;
                            s.Type = "T";
                            Tlist.Add(s);
                        }
                    }
                }
                foreach (Diary si in items3)
                {
                    foreach (string str in words)
                    {
                        if (si.Title.CaseInsensitiveContains(str))
                        {
                            s = new StoreListing();
                            s.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(si.Cover_Url)); // some static iage for scrap
                            s.Price = "Price: " + si.Price.ToString();
                            s.Title = si.Title;
                            s.Id = si.Id;
                            s.UserId = si.UserId;
                            s.MyId = si.Tour_List;
                            s.Type = "D";
                            Dlist.Add(s);
                        }
                        else if (si.Tags != null && si.Tags.CaseInsensitiveContains(str))
                        {
                            s = new StoreListing();
                            s.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(si.Cover_Url)); // some static iage for scrap
                            s.Price = "Price: " + si.Price.ToString();
                            s.Title = si.Title;
                            s.Id = si.Id;
                            s.UserId = si.UserId;
                            s.MyId = si.Tour_List;
                            s.Type = "D";
                            Dlist.Add(s);
                        }

                        else if (si.Desc != null && si.Desc.CaseInsensitiveContains(str))
                        {
                            s = new StoreListing();
                            s.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(si.Cover_Url)); // some static iage for scrap
                            s.Price = "Price: " + si.Price.ToString();
                            s.Title = si.Title;
                            s.Id = si.Id;
                            s.UserId = si.UserId;
                            s.MyId = si.Tour_List;
                            s.Type = "D";
                            Dlist.Add(s);
                        }
                    }
                }            
                LoadingBar2.IsActive = false;
                LoadingBar2.Visibility = Visibility.Collapsed;
                DiaryView.DataContext = Dlist;
                TourView.DataContext = Tlist;
            }
            catch(Exception)
            {
                MessageDialog msgbox = new MessageDialog("Sorry can't update now");
                await msgbox.ShowAsync();
                LoadingBar2.Visibility = Visibility.Collapsed;
            }
        }
        private void StoreListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            StoreListing sent = e.ClickedItem as StoreListing;
            if (sent.Type == "S")
                Frame.Navigate(typeof(ViewScrape), sent.Id);
            else if (sent.Type == "D")
                Frame.Navigate(typeof(Diary_Store_View_Page), sent);
            else if (sent.Type == "T")
                Frame.Navigate(typeof(Tour_Store_View_Page), sent);
        }
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
        //    Grid g = new Grid();
        //    g = sender as Grid;
        //    FrameworkElement type = null;
        //    FrameworkElement id = null;
        //    foreach (FrameworkElement child in g.Children)
        //    {
        //        if ((Grid.GetRow(child) == 2) && (Grid.GetColumn(child) == 1))
        //        {
        //            Border b = child as Border;

        //            id = b.Child as FrameworkElement;
        //        }


        //        if ((Grid.GetRow(child) == 1) && (Grid.GetColumn(child) == 0))
        //        {
        //            Border b = child as Border;

        //            type = b.Child as FrameworkElement;
        //        }
        //    }

        //    TextBlock t = id as TextBlock;
        //    TextBlock t2 = type as TextBlock;

        //    if (t2.Text == "S")
        //        Frame.Navigate(typeof(ViewScrape), t.Text);
        //    else if (t2.Text == "D")
        //        Frame.Navigate(typeof(DiaryViewer_Page), t.Text);
        //    else if (t2.Text == "T")
        //        Frame.Navigate(typeof(TourViewer_Page), t.Text);
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
    }
}
