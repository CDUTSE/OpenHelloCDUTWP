using DataHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using 你好理工.DataHelper.Helper;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.View.Auth
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Agreement : Page
    {
        public Agreement()
        {
            this.InitializeComponent();
            
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);

            webView.FrameNavigationStarting += webView_FrameNavigationStarting;
            webView.FrameNavigationCompleted += webView_FrameNavigationCompleted;

            News news = e.Parameter as News;
            if (news !=null)
            {
                titleTextBlock.Text = news.newsTitle;
                webView.Navigate(new Uri( news.newsUrl ));
            }
            else
            {
                webView.Navigate(new Uri("http://www.hellocdut.com/agreement.html"));
            }
        }

        void webView_FrameNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            progressRing0.IsActive = false;
        }

        void webView_FrameNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            progressRing0.IsActive = true;
        }
    }
}
