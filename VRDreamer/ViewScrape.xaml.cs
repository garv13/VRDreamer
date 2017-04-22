using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.System.Display;
using Windows.UI.Core;
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
    public sealed partial class ViewScrape : Page
    {
        MediaCapture _mediaCapture;
        bool _isPreviewing;
        OrientationSensor or;
        Compass c;
        DisplayRequest _displayRequest;
        List<Pointer> li;
        List<PointerViewAR> li2;
        Geoposition pos;
        bool myBool;
        int cou;
        double yaw;
        double wid, h, stepW, stepH, temppitch, tempyaw;
        double yaw5, pitch5;

        private IMobileServiceTable<Scrap> Table1 = App.MobileService.GetTable<Scrap>();
        private MobileServiceCollection<Scrap, Scrap> items1;
        private IMobileServiceTable<Pointer> Table2 = App.MobileService.GetTable<Pointer>();
        private MobileServiceCollection<Pointer, Pointer> items2;
        public ViewScrape()
        {
            cou = 0;
            li = new List<Pointer>();
            li2 = new List<PointerViewAR>();
            or = OrientationSensor.GetDefault();
            myBool = false;
            yaw5 = 0; pitch5 = 0;
            c = Compass.GetDefault();
            try
            {
                c.ReportInterval = 4;
                c.ReadingChanged += C_ReadingChanged;
                or.ReportInterval = 4;
                or.ReadingChanged += Or_ReadingChanged;
            }
            catch
            { }
            
            Application.Current.Suspending += Application_Suspending;
            
            this.InitializeComponent();
            PreviewControl.Loaded += PreviewControl_Loaded;
        }

        private void PreviewControl_Loaded(object sender, RoutedEventArgs e)
        {
            wid = PreviewControl.ActualWidth;
            h = PreviewControl.ActualHeight;
            stepH = h / 0.5;
            stepW = wid / 90;
        }

        private async void Or_ReadingChanged(OrientationSensor sender, OrientationSensorReadingChangedEventArgs args)
        {
            if (myBool)
            {
                OrientationSensorReading reading = args.Reading;

                // Quaternion values
                SensorQuaternion q = reading.Quaternion;  
                double y = args.Reading.Quaternion.Y;
                if (y < 0)
                    y = y + 2;

                double pitch = y;
                //    pitch = pitch * 180 / Math.PI;
                   if (yaw < 0)
                        yaw += 360;
                   Debug.WriteLine(yaw.ToString() + "," + pitch.ToString());


                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        for (; lol.Children.Count > 1;)
                        {
                            lol.Children.RemoveAt(1);
                        }

                        foreach (PointerViewAR n in li2)
                        {
                            Image img = new Image();
                            img.Name = n.Id;
                            img.Width = 250;
                            img.Height = 250;
                            img.Source = n.Media;
                            TranslateTransform t = new TranslateTransform();

                            t.X =(n.Yaw - yaw) * stepW;
                            t.Y = (n.Pitch-pitch) * stepH;

                            img.RenderTransform = t;
                            img.IsTapEnabled = true;
                            img.Tapped += Img_Tapped;
                            lol.Children.Add(img);                       
                        }
                    });
                
            }
        }

        private void Img_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image img = sender as Image;
            string s = img.Name;
            Frame.Navigate(typeof(Image_Tapped_Detail), s);
        }

        private void C_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            CompassReading reading = args.Reading;
            yaw = reading.HeadingTrueNorth.Value;
            
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Loading.Visibility = Visibility.Visible;
          

            string id = e.Parameter as string;
            items1 = await Table1.Where(s => s.Id == id).ToCollectionAsync();
            if (items1 != null)
            {
                Scrap s = items1[0];
                string[] str = s.Point_List.Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    string po = str[i];
                    items2 = await Table2.Where(t => t.Id == po).ToCollectionAsync();
                    Pointer p = new Pointer();
                    if (items2.Count>0)
                    {
                        p = items2[0];
                        li.Add(p);
                    }
                }
            }
            await StartPreviewAsync();
            for (int i = 0; i < li.Count; i++)
            {
                PointerViewAR p = new PointerViewAR();
                p.Id = li[i].Id;
                p.lat = li[i].lat;
                p.lon = li[i].lon;
                p.Pitch = li[i].Pitch;
                p.Title = li[i].Title;
                p.Yaw = li[i].Yaw;
                p.Desc = li[i].Desc;
                p.Media = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(li[i].Media_Url));
                li2.Add(p);
            }
            
            myBool = true;

        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await CleanupCameraAsync();
        }
        private async void Application_Suspending(object sender, SuspendingEventArgs e)
        {
            // Handle global application events only if this page is active
            if (Frame.CurrentSourcePageType == typeof(MainPage))
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                await CleanupCameraAsync();
                deferral.Complete();
            }
        }

        private async Task StartPreviewAsync()
        {
            try
            {

                _mediaCapture = new MediaCapture();
                await _mediaCapture.InitializeAsync();
                PreviewControl.Source = _mediaCapture;

                await _mediaCapture.StartPreviewAsync();
                _isPreviewing = true;

                _displayRequest.RequestActive();
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
            }
            catch (UnauthorizedAccessException)
            {
                // This will be thrown if the user denied access to the camera in privacy settings
                System.Diagnostics.Debug.WriteLine("The app was denied access to the camera");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("MediaCapture initialization failed. {0}", ex.Message);
            }
        }
        private async Task CleanupCameraAsync()
        {
            if (_mediaCapture != null)
            {
                if (_isPreviewing)
                {
                    await _mediaCapture.StopPreviewAsync();
                }

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    PreviewControl.Source = null;
                    if (_displayRequest != null)
                    {
                        _displayRequest.RequestRelease();
                    }

                    _mediaCapture.Dispose();
                    _mediaCapture = null;
                });
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
