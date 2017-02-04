using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.Web.Http;
using 你好理工.DataHelper.Helper;
using 你好理工.DataHelper.Model;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.View.Auth
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Register : Page
    {
        public Register()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //获取输入
            string user_name = txtAccount.Text.Trim();
            string user_password = txtPassword.Password.Trim();
            string surePassword = txtSurePassword.Password.Trim();
            if(string.IsNullOrEmpty(user_name))
            {
                txtAccount.Focus(FocusState.Programmatic);
                return;
            }
            else if (string.IsNullOrEmpty(user_password))
            {
                txtPassword.Focus(FocusState.Programmatic);
                return;
            }
            else if(!user_password.Equals(surePassword))
            {
                //txtSurePassword.Focus(FocusState.Programmatic);
                Functions.ShowMessage("两次输入的密码不一致");
                return;
            }
            else if(!Regex.IsMatch(user_name, "[a-z][a-z0-9]{5,15}"))
            {
                Functions.ShowMessage("用户名不合法！必须小写字母开头，6-16位");
                return;
            }
            else
            {
                
                string user_device_code = Functions.GetUniqueDeviceId();

                HttpResponseMessage response = await APIHelper.RegisterUser(user_name, user_password,user_device_code);

                if(response!=null && response.Content!=null)
                {
                    Result token = Functions.Deserlialize<Result>(response.Content.ToString());
                    if(token!=null)
                    {
                        Functions.ShowMessage(token.message);
                    }
                }
            }
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Agreement));
        }

        private void HyperlinkButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Agreement));
        }

        
    }
}
