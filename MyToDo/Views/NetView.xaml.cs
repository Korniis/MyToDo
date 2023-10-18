using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyToDo.Views
{
    /// <summary>
    /// NetView.xaml 的交互逻辑
    /// </summary>
    public partial class NetView : UserControl
    {
        public NetView()
        {
            InitializeComponent();

            FMTEST_LoadAsync();
        }
        private async void FMTEST_LoadAsync()
        {

            await webView.EnsureCoreWebView2Async();
            /* webView.CoreWebView2.Settings.UserAgent = "Mozilla/5.0 (Linux; Android 11; Pixel 4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Mobile Safari/537.36";
 */
            webView.CoreWebView2.CookieManager.DeleteAllCookies();
            webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }
        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            //e.NewWindow = webView22.CoreWebView2;
            //在第二个webview2上面打开这个新窗口
            webView.Source = new Uri(e.Uri.ToString());
            e.Handled = true;//禁止弹窗

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            webView.Source = new Uri("https://www.baidu.com");
        }
    }
}
