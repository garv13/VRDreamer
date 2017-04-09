using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public sealed partial class TouristToolkit : Page
    {
        public TouristToolkit()
        {
            this.InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var add = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

            var httpRequestMessage = new Windows.Web.Http.HttpRequestMessage(Windows.Web.Http.HttpMethod.Get, new Uri("http://www.google.com/searchbyimage?site=search&sa=X&image_url=https://vrdreamer.blob.core.windows.net/first/qutubminar2.jpg"));
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
