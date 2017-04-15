using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class Created_upload_Page : Page
    {
        StorageFile media2 = null;
        List<Purchsed> p = new List<Purchsed>();
        Tour t = new Tour();
        Diary d = new Diary();
        public Created_upload_Page()
        {
            this.InitializeComponent();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            p = e.Parameter as List<Purchsed>;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                openPicker.FileTypeFilter.Add(".jpg");
                media2 = await openPicker.PickSingleFileAsync();
            }
            catch (Exception)
            {
                MessageDialog msgbox = new MessageDialog("Image not Selected");
                await msgbox.ShowAsync();
                return;
            }
        }

        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadingBar.Visibility = Visibility.Visible;
            LoadingBar.IsIndeterminate = true;
            if(media2 == null)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                MessageDialog msgbox = new MessageDialog("Please select a cover image icon");
                await msgbox.ShowAsync();
                return;
            }
            if(Name.Text == "")
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                MessageDialog msgbox = new MessageDialog("Please enter name of the item");
                await msgbox.ShowAsync();
                return;
            }
            string str = "";
            foreach (Purchsed ps in p)
            {
                str = str + ps.Id + ',';
            }
            str = str.Remove(str.Length - 1);
            try
            {
                var credentials = new StorageCredentials("vrdreamer", "lTD5XmjEhvfUsC/vVTLsl01+8pJOlMdF/ri7W1cNOydXwSdb8KQpDbiveVciOqdIbuDu6gJW8g44YtVjuBzFkQ==");
                var client = new CloudBlobClient(new Uri("https://vrdreamer.blob.core.windows.net/"), credentials);
                var container = client.GetContainerReference("second");
                var blockBlob = container.GetBlockBlobReference(Guid.NewGuid().ToString() + ".jpg");
                await blockBlob.UploadFromFileAsync(media2);

                if (p[0].Type == "T")
                {
                    d.Cover_Url = blockBlob.StorageUri.PrimaryUri.ToString();
                    d.Title = Name.Text;
                    d.Tags = Tags.Text;
                    d.Tour_List = str;
                    d.UserId = App.userId;
                    d.Price = Convert.ToInt32(Price.Text);
                    await App.MobileService.GetTable<Diary>().InsertAsync(d);
                    LoadingBar.Visibility = Visibility.Collapsed;
                    MessageDialog msgbox = new MessageDialog("Diary Created :):)");
                    await msgbox.ShowAsync();
                    Frame.Navigate(typeof(MainPage));
                }
                if (p[0].Type == "S")
                {
                    t.Cover_Url = blockBlob.StorageUri.PrimaryUri.ToString();
                    t.Title = Name.Text;
                    t.Scrap_List = str;
                    t.Tags = Tags.Text;
                    t.UserId = App.userId;
                    t.Price = Convert.ToInt32(Price.Text);
                    await App.MobileService.GetTable<Tour>().InsertAsync(t);
                    LoadingBar.Visibility = Visibility.Collapsed;
                    MessageDialog msgbox = new MessageDialog("Tour Created :):)");
                    await msgbox.ShowAsync();
                    Frame.Navigate(typeof(MainPage));

                }
            }
            catch(Exception)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                MessageDialog msgbox = new MessageDialog("Sorry can't upload now");
                await msgbox.ShowAsync();
            }
        }
    }
}
