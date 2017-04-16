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
        public Activity_Detail()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Monument_Detail_View m = new Monument_Detail_View();
            string blobUrl = e.Parameter as string;
            text.Text = m.Desc;
            img.Source = m.Image;

            VisionServiceClient cl = new VisionServiceClient("db82ef68dc95459fad7b46d7a50bb944");

            VisualFeature[] vf = new VisualFeature[] { VisualFeature.Tags };
            AnalysisResult ar = await cl.AnalyzeImageAsync(blobUrl, vf);
            Tag[] f = ar.Tags;
            int i = 0;
            foreach (Tag t in f)
            {
                if (t.Name.Contains("text"))
                {
                    Frame.Navigate(typeof(Ocr_Detail),blobUrl);
                }
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
    }
}
