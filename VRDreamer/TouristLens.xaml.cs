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
    /// 


    public sealed partial class TouristLens : Page
    {
        MediaCapture _mediaCapture;
        bool _isPreviewing;
        OrientationSensor or;
        Compass c;
        DisplayRequest _displayRequest;
        List<Scrap> li;
        List<Point> li3;
        List<PointerViewAR> li2;
        Geoposition pos;
        bool myBool;
        int cou;
        double yaw;
        double wid, h, stepW, stepH, temppitch, tempyaw;
        double pitch;
        int i;
        private IMobileServiceTable<Tour> Table1 = App.MobileService.GetTable<Tour>();
        private MobileServiceCollection<Tour, Tour> items1;


        private IMobileServiceTable<Scrap> Table3 = App.MobileService.GetTable<Scrap>();
        private MobileServiceCollection<Scrap, Scrap> items3;
        private IMobileServiceTable<Pointer> Table2 = App.MobileService.GetTable<Pointer>();
        private MobileServiceCollection<Pointer, Pointer> items2;

        public TouristLens()
        {
            i = 0;

            cou = 0;
            li = new List<Scrap>();
            li2 = new List<PointerViewAR>();
            li3 = new List<Point>();
            or = OrientationSensor.GetDefault();
            myBool = false;

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

        //this loads all the scrpaes in list li
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Loading.Visibility = Visibility.Visible;


            string id = e.Parameter as string;
            items1 = await Table1.Where(s => s.Id == id).ToCollectionAsync();
            if (items1 != null)
            {
                Tour s = items1[0];
                string[] str = s.Scrap_List.Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    string po = str[i];
                    items3 = await Table3.Where(t => t.Id == po).ToCollectionAsync();
                    Scrap p;
                    if (items3.Count > 0)
                    {
                        p = items3[0];
                        li.Add(p);
                    }
                }//loaded all the scarps in tour in li
            }
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    // _rootPage.NotifyUser("Waiting for update...", NotifyType.StatusMessage);

                    // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                    Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 2, MovementThreshold = 2 };

                    // Subscribe to the StatusChanged event to get updates of location status changes.


                    // Carry out the operation.
                    pos = await geolocator.GetGeopositionAsync();
                    Tags.Text = "Destination is " + li[i].Title;
                    geolocator.PositionChanged += Geolocator_PositionChanged;
                    break;
            }


            await StartPreviewAsync();
            myBool = true;

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

                pitch = y;
                //    pitch = pitch * 180 / Math.PI;



                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    //for (; lol.Children.Count > 1;)
                    //{
                    //    lol.Children.RemoveAt(1);
                    //}

                    foreach (PointerViewAR n in li2)
                    {
                        Image img = new Image();
                        img.Width = 250;
                        img.Height = 250;
                        img.Source = n.Media;
                        TranslateTransform t = new TranslateTransform();

                        t.X = (n.Yaw - yaw) * stepW;
                        t.Y = (n.Pitch - pitch) * stepH;

                        img.RenderTransform = t;
                        //lol.Children.Add(img);
                    }


                });

            }
        }

        private void C_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            CompassReading reading = args.Reading;
            yaw = reading.HeadingTrueNorth.Value;

        }
        private void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            pos = args.Position;
        }
        private void PreviewControl_Loaded(object sender, RoutedEventArgs e)
        {
            wid = PreviewControl.ActualWidth;
            h = PreviewControl.ActualHeight;
            stepH = h / 0.5;
            stepW = wid / 90;
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (i < li.Count - 1)
                i++;
            Tags.Text = "Destination is " + li[i].Title;

            loadPoint();

        }

        private async void loadPoint()
        {

            for (int i = 0; i < li.Count; i++)
            {
                //PointerViewAR p = new PointerViewAR();
                //p.Id = li[i].Id;
                //p.lat = li[i].lat;
                //p.lon = li[i].lon;
                //p.Pitch = li[i].Pitch;
                //p.Title = li[i].Title;
                //p.Yaw = li[i].Yaw;
                //p.Desc = li[i].Desc;
                //p.Media = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(li[i].Media_Url));
                //li2.Add(p);
            }


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (i > 0)
                i--;
            Tags.Text = "Destination is " + li[i].Title;
            loadPoint();

        }
        private async void NextBar_Click(object sender, RoutedEventArgs e)
        {
            // Center on New York City
            var uriNewYork = new Uri(@"bingmaps:?rtp=pos." + pos.Coordinate.Latitude.ToString() + "_" + pos.Coordinate.Longitude.ToString() + "~pos."
                + li[i].lat.ToString() + "_" + li[i].lon.ToString() + "&trfc=1");

            // Launch the Windows Maps app
            var launcherOptions = new Windows.System.LauncherOptions();
            launcherOptions.TargetApplicationPackageFamilyName = "Microsoft.WindowsMaps_8wekyb3d8bbwe";
            var success = await Windows.System.Launcher.LaunchUriAsync(uriNewYork, launcherOptions);


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
