﻿using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using 你好理工.DataHelper.Helper;
using 你好理工.DataHelper.Model;
using 你好理工.View.Auth;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WelComePage : Page
    {
        public WelComePage()
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
            Settings.Instance.IsFirstLaunch = false;
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if(this.Frame.CanGoBack)    //从关于页面进入
            {
                this.Frame.GoBack();
            }
            else  //从程序第一次启动的欢迎页打开
            {
                this.Frame.Navigate(typeof(Login));
            }
        }
    }
}
