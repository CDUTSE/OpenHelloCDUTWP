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
using 你好理工.DataHelper.Model;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.View.Me.Setting
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Theme : Page
    {
        public Theme()
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

            //if(Settings.Instance.NightMode.Equals("True"))  //是夜间模式
            //{
                
            //    nightModelToggle.IsOn = true;
            //}
            //else
            //{
            //    nightModelToggle.IsOn = false;
            //}
        }

        private void nightModelToggle_Toggled(object sender, RoutedEventArgs e)
        {
            
            ////如果本来是Light,改成Dark
            //if (this.RequestedTheme == ElementTheme.Light)
            //{
            //}
            //if (Settings.Instance.NightMode.Equals("True"))
            //{
            //    this.RequestedTheme = ElementTheme.Light;
            //    Settings.Instance.NightMode = "False";
            //}
            //else
            //{
            //    this.RequestedTheme = ElementTheme.Dark;
            //    Settings.Instance.NightMode = "True";

            //}
            if (nightModelToggle == null) return;
            if (Settings.Instance.NightMode.Equals("True"))  //是夜间模式
            {
                this.RequestedTheme = ElementTheme.Dark;
            }
            else
            {
                this.RequestedTheme = ElementTheme.Light;
            }
        }


  
    }
}
