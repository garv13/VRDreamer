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
    public sealed partial class DiaryViewer_Page : Page
    {
        private IMobileServiceTable<Tour> Table2 = App.MobileService.GetTable<Tour>();
        private MobileServiceCollection<Tour, Tour> items2;
        private IMobileServiceTable<Diary> Table3 = App.MobileService.GetTable<Diary>();
        private MobileServiceCollection<Diary, Diary> items3;

        Purchsed m = new Purchsed();
        List<Purchsed> Tlist = new List<Purchsed>();

        public DiaryViewer_Page()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string id = e.Parameter as string;

            try
            {
                items3 = await Table3.Where(Diary
                         => Diary.Id == id).ToCollectionAsync();
                string[] ids = items3[0].Tour_List.Split(',');
                foreach (string nid in ids)
                {
                    if (nid != "")
                    {
                        items2 = await Table2.Where(Tour
                       => Tour.Id == nid).ToCollectionAsync();
                        m = new Purchsed();
                        m.Id = items2[0].Id;
                        m.Title = items2[0].Title;
                        m.Image = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(items2[0].Cover_Url));
                        m.Type = "T";
                        Tlist.Add(m);
                    }
                }
                DiaryView.DataContext = Tlist;
            }

            catch(Exception)
            {

            }
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

            Frame.Navigate(typeof(TourViewer_Page), t.Text);
        }
    }
}
