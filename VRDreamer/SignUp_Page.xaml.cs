using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class SignUp_Page : Page
    {
        public SignUp_Page()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private IMobileServiceTable<User> Table = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items;
        string error = "";

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
            else
            {
                Frame.Navigate(typeof(Login_Page));
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadingBar.Visibility = Visibility.Visible;
            LoadingBar.IsIndeterminate = true;
            User a = new User();
            int i = 1;
            error = "";
            if (!Regex.Match(Name.Text, @"^[a-zA-Z\s\.]+$").Success)
            {
                i = 0;
                error = error + "*Enter a valid name  ";
            }
            if (!Regex.Match(Email.Text, @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$").Success)
            {
                error = error + "*Enter valid Email Id  ";
                i = 0;
            }

            if (!(UserName.Text.All(char.IsLetterOrDigit) && UserName.Text.Length != 0))
            {

                error = error + "*Username can have only alphanumeric values  ";
                i = 0;
            }
            else
            {
                //username exist check
                items = await Table.Where(User
                              => User.UserName == UserName.Text).ToCollectionAsync();
                if (items.Count != 0)
                {
                    error = error + "*Username already exist  ";
                    i = 0;
                }
            }

            if ((Password.Password.Length < 8))
            {
                i = 0;
                error = error + "*Password should be minimum of length 8  ";
            }
            else
            {
                if (Password.Password != ConfirmPassword.Password)
                {
                    i = 0;
                    error = error + "*Password didn't matched  ";
                }
            }

            if (i == 0)
            {
                MistakeBox.Text = error;
                MistakeBox.Visibility = Visibility.Visible;
                LoadingBar.Visibility = Visibility.Collapsed;
            }

            else
            {

                try
                {
                    a.UserId = Name.Text;
                    a.Email = Email.Text;
                    a.Password = Password.Password;
                    a.UserName = UserName.Text;
                    a.wallet = 3000;
                    a.TourPurchases = "";
                    a.DiaryPurchases = "";
                    a.ScrapePurchase = "";
                    await App.MobileService.GetTable<User>().InsertAsync(a);
                    MessageDialog msgbox = new MessageDialog("Register Successful:):)");
                    await msgbox.ShowAsync();
                    LoadingBar.Visibility = Visibility.Collapsed;
                    Frame.Navigate(typeof(Login_Page));
                }
                catch (Exception)
                {
                    MessageDialog msgbox = new MessageDialog("Something is not right Please try again later:(:(");
                    await msgbox.ShowAsync();
                    LoadingBar.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
