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

namespace 你好理工.View.Me.Setting
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ChangePassword : Page
    {
        public ChangePassword()
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
            string user_password = pwdTextBox.Text.Trim();
            string user_new_password = newPwdTextBox.Text.Trim();
            string surePwd = surePwdTextBox.Text.Trim();

            if(string.IsNullOrEmpty(pwdTextBox.Text.Trim()))
            {
                Functions.ShowMessage("原密码不能为空");
                pwdTextBox.Focus(FocusState.Programmatic);
                return;
            }
            if (!surePwd.Equals(user_new_password))
            {
                Functions.ShowMessage("两次输入的密码不一致");
                return;
            }
            HttpResponseMessage response = await  APIHelper.ModifyUserPassword((App.Current as App).user_name, (App.Current as App).user_login_token,
                user_new_password);
            if(response!=null && response.Content!=null)
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
