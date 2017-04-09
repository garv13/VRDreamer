using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
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
    public sealed partial class TouristToolkit : Page
    {
        OrientationSensor or;
        public TouristToolkit()
        {
            this.InitializeComponent();
            or = OrientationSensor.GetDefault();
            try
            {
               
                or.ReportInterval = 4;
                or.ReadingChanged += Or_ReadingChanged;
            }
            catch
            { }

        }

        private async void Or_ReadingChanged(OrientationSensor sender, OrientationSensorReadingChangedEventArgs args)
        {
            OrientationSensorReading reading = args.Reading;

            //double r31 = reading.RotationMatrix.M31;
            //double r32 = reading.RotationMatrix.M32;
            //double r33 = reading.RotationMatrix.M33;
            //r32 = r32 * r32;
            //r33 = r33 * r33;
            //double denomintor = r32 + r33;
            //denomintor = Math.Sqrt(denomintor);
            //double numerator = -1 * r31;
            //double angle = 0;
            //if (denomintor > 0)
            //{
            //     angle = Math.Atan(numerator/denomintor);
            //    angle = angle * 180 / Math.PI;

            //}
            //if (numerator >= 0 && denomintor < 0)
            //{
            //     angle = Math.Atan(numerator/denomintor);
            //    angle = angle * 180 / Math.PI;
            //    angle = angle + 180;
            //}
            //if (numerator < 0 && denomintor < 0)
            //{

            //     angle = Math.Atan(numerator / denomintor);
            //    angle = angle * 180 / Math.PI;
            //    angle = angle - 180;
            //}
            //if (numerator > 0 && denomintor == 0)
            //{

            //     angle = 90;
            //}
            //if (numerator < 0 && denomintor == 0)
            //{

            //     angle = -90;
            //}
            //if (angle < 0)
            //    angle += 360;





            //char sign, sign2;
            //if (numerator > 0)
            //    sign = '+';
            //else
            //    sign = '-';

            //if (denomintor > 0)
            //    sign2 = '+';
            //else
            //    sign2 = '-';
            double y = args.Reading.Quaternion.Y;
            if (y < 0)
                y = y + 2;
            Debug.WriteLine("Angle is " +y);

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                textBox.Text = "Angle is " + y.ToString();
            });

            
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var add = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

            var httpRequestMessage = new Windows.Web.Http.HttpRequestMessage(Windows.Web.Http.HttpMethod.Get, new Uri("http://www.google.com/searchbyimage?site=search&sa=X&image_url=https://vrdreamer.blob.core.windows.net/first/pp2.jpg"));
            httpRequestMessage.Headers.Add("User-Agent", add);

            web.NavigateWithHttpRequestMessage(httpRequestMessage);
            web.DOMContentLoaded += Web_DOMContentLoaded;
           // string s = await cl.GetStringAsync("http://www.google.com/searchbyimage?site=search&sa=X&image_url=https://vrdreamer.blob.core.windows.net/first/qutubminar2.jpg");

           // web.Source = new Uri("http://www.google.com/searchbyimage?site=search&sa=X&image_url=https://vrdreamer.blob.core.windows.net/first/qutubminar2.jpg");
            //web.Refresh();
           
            //int i = s.IndexOf("<a class=\"_gUb\" href=");
            //int j = s.IndexOf("</a>", i);
            //s = s.Substring(i, j - 1);

        }

        private async void Web_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            //HttpClient cl = new HttpClient();

            string s = await web.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });

            int i = s.IndexOf("<a class=\"_gUb");
            int j = s.IndexOf(">", i);
            i = s.IndexOf("<",j);
            s = s.Substring(j+1, i-j-1);

        }
    }
}
