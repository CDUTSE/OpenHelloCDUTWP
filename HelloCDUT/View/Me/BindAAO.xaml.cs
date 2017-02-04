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

namespace 你好理工.View.Me
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BindAAO : Page
    {
        public BindAAO()
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

        private async void bindAAOBtn_Click(object sender, RoutedEventArgs e)
        {
            string aao_account = txtAAOAccount.Text.Trim();
            string aao_password = txtAAOPwdBox.Password.Trim();
            if (String.IsNullOrEmpty(aao_account) || String.IsNullOrEmpty(aao_password))
            {
                Functions.ShowMessage("教务处账号或密码不能为空");
                return;
            }
            HttpResponseMessage response = await APIHelper.BindAAO((Application.Current as App).user_name, (Application.Current as App).user_login_token,
                aao_account, aao_password);
            if (response != null)
            {
                Result result = Functions.Deserlialize<Result>(response.Content.ToString());

                if (result != null)
                {
                    if (result.result.Equals("true"))   //绑定成功,更新内存中绑定状态
                    {
                        (App.Current as App).user_aao_status = "1";
                    }
                }
                Functions.ShowMessage(result.message);
            }
        }
    }
}
