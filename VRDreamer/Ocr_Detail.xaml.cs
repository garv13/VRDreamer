using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public sealed partial class Ocr_Detail : Page
    {
        public Ocr_Detail()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            string text = e.Parameter as string;
            try
            {
                WebRequest request = WebRequest.Create("https://www.googleapis.com/language/translate/v2?q=" + text + "&target=hi&key=AIzaSyBMzL7mptHo33PNsuKmT9xKppNgkXotBOM");
                request.Method = "GET";
                string token = null;
                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader sr99 = new StreamReader(stream))
                        {
                            token = sr99.ReadToEnd();
                        }
                    }
                }
                RootObject tmp = new RootObject();
                tmp = JsonConvert.DeserializeObject<RootObject>(token);
                textTranslate.Text = tmp.data.translations[0].translatedText;
                textBox.Text = text;
            }
            catch (Exception ex)
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
    }
}
