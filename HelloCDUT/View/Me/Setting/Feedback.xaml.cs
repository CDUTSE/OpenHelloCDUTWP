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
using Windows.Web.Http;
using 你好理工.DataHelper.Helper;
using 你好理工.DataHelper.Model;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.View.Scholl.Setting
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Feedback : Page
    {
        public Feedback()
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
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string advice = adviceBox.Text;
            string version = Functions.GetVersionString();
            HttpResponseMessage response = await  APIHelper.FeedbackAdvice((Application.Current as App).user_name,
                (Application.Current as App).user_login_token,
                advice,
                version,
                "2"
                );
            if(response!=null)
            {
                Result result = Functions.Deserlialize<Result>(response.Content.ToString());
                if(result!=null)
                {
                    Functions.ShowMessage(result.message);
                }
            }
        }
    }
}
