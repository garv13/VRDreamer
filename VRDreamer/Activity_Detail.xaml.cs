using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
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
    public sealed partial class Activity_Detail : Page
    {
        List<Monument_Detail_View> MList = new List<Monument_Detail_View>();
        public Activity_Detail()
        {
            this.InitializeComponent();
        }     
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Monument_Detail_View m = new Monument_Detail_View();
            Tag[] f = e.Parameter as Tag[];
            foreach (Tag t in f)
            {
                if (t.Name != null || t.Name != "")
                {
                    m = new Monument_Detail_View();
                    m.Title = t.Name;
                    m.Image = null;
                    m.Desc = null;
                }
                MList.Add(m);
            }
            DiaryView.DataContext = MList;
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

        private void DiaryView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Monument_Detail_View m = e.ClickedItem as Monument_Detail_View;
            Frame.Navigate(typeof(Store), m.Title);
        }
    }
}
