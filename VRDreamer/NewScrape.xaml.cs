using System;
using System.Collections.Generic;
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
    public sealed partial class NewScrape : Page
    {
        MediaCapture _mediaCapture;
        bool _isPreviewing;
        OrientationSensor or;
        Compass c;
        DisplayRequest _displayRequest;
        List<Pointer> li;
        Geoposition pos;
        int i = 0;
        public NewScrape()
        {
            li = new List<Pointer>();
            or = OrientationSensor.GetDefault();
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
            this.InitializeComponent();
            Application.Current.Suspending += Application_Suspending;
            Loaded += NewScrape_Loaded;
        }

        private async void NewScrape_Loaded(object sender, RoutedEventArgs e)
        {
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
                    Tags.Text = "0 Point(s) Tagged";
                    break;
            }
        }

        private void Or_ReadingChanged(OrientationSensor sender, OrientationSensorReadingChangedEventArgs args)
        {
          
        }

        private void C_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
           
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await StartPreviewAsync();
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

        private void PreviewControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
           
            Pointer po = new Pointer();
            if (pos != null)
            {
                i++;
                po.lat = pos.Coordinate.Latitude;
                po.lon = pos.Coordinate.Latitude;
                po.Yaw = c.GetCurrentReading().HeadingMagneticNorth;
               
                OrientationSensorReading reading2 = or.GetCurrentReading();
                SensorQuaternion q = reading2.Quaternion;   // get a reference to the object to avoid re-creating it for each access
                double ysqr = q.Y * q.Y;
                // roll (x-axis rotation)
                double t0 = +2.0 * (q.W * q.X + q.Y * q.Z);
                double t1 = +1.0 - 2.0 * (q.X * q.X + ysqr);

                // pitch (y-axis rotati)
                double t2 = +2.0 * (q.W * q.Y - q.Z * q.X);
                t2 = t2 > 1.0 ? 1.0 : t2;
                t2 = t2 < -1.0 ? -1.0 : t2;
                double pitch = Math.Asin(t2);
                pitch = pitch * 180 / Math.PI;

                if (pitch < 0)
                    pitch += 360;
                po.Pitch = pitch;

                li.Add(po);
                Tags.Text = i.ToString() + " Point(s) Tagged";
            }
        }

        private void NextBar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ScrapeForm), li);
        }
    }
}
