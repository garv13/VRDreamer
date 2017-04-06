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
    public sealed partial class Login_Page : Page
    {
        private IMobileServiceTable<User> Table = App.MobileService.GetTable<User>();
        private MobileServiceCollection<User, User> items;
        public Login_Page()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadingBar.Visibility = Visibility.Visible;
            LoadingBar.IsIndeterminate = true;
            try
            {
                items = await Table.Where(User
                                => User.UserName == UserName.Text).ToCollectionAsync();
                if (items.Count == 1)
                {
                    if (Password.Password == items[0].Password)
                    {
                        LoadingBar.Visibility = Visibility.Collapsed;
                        MessageDialog msgbox = new MessageDialog("Welcome " + UserName.Text);
                        await msgbox.ShowAsync();
                        App.userId = items[0].Id;
                        Frame.Navigate(typeof(ToolKit_Page));
                    }
                }
                else
                {
                    LoadingBar.Visibility = Visibility.Collapsed;
                    MessageDialog msgbox = new MessageDialog("Password or username incorrect");
                    await msgbox.ShowAsync();
                }
            }
            catch (Exception)
            {
                LoadingBar.Visibility = Visibility.Collapsed;
                MessageDialog msgbox = new MessageDialog("Sorry Can't connect");
                await msgbox.ShowAsync();
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignUp_Page));
        }
    }
}
