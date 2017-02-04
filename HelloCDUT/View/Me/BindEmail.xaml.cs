using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
    public sealed partial class BindEmail : Page
    {
        public BindEmail()
        {
            this.InitializeComponent();
            
        }

        private DispatcherTimer timer = new DispatcherTimer();
        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);
        }

        /// <summary>
        /// 获取验证码 60秒后重新获取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void getCheckBtn_Click(object sender, RoutedEventArgs e)
        {
            
            string user_email = emailTextBox.Text.Trim();
            if (string.IsNullOrEmpty(user_email))
            {
                Functions.ShowMessage("邮箱不能为空");
                emailTextBox.Focus(FocusState.Programmatic);
                return;
            }
            if(!Regex.IsMatch(user_email,@"(\w)+(\.\w+)*@(\w)+((\.\w{2,3}){1,3})"))
            {
                Functions.ShowMessage("邮箱格式不正确");
                emailTextBox.Focus(FocusState.Programmatic);
                return;
            }
            getCheckBtn.IsEnabled = false;

            HttpResponseMessage response = await APIHelper.GetBindEmailToken((Application.Current as App).user_name, (Application.Current as App).user_login_token,user_email);
            if (response != null)
            {
                Result result = Functions.Deserlialize<Result>(response.Content.ToString());
                if (result != null)
                {
                    Functions.ShowMessage(result.message);
                }
            }
            int status = 60;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += ((s,args) =>
                {
                    Debug.WriteLine("DispatcherTimer：" + status);
                    status--;
                    getCheckBtn.Content = status + "秒后重新获取";
                    if(status==0)
                    {
                        status = 0;
                        getCheckBtn.IsEnabled = true;
                        getCheckBtn.Content = "点击获取验证码";
                        timer.Stop();
                    }
                    
                    
                });
            timer.Start();
        }

        /// <summary>
        /// 绑定邮箱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void bindEmailBtn_Click(object sender, RoutedEventArgs e)
        {
            string active_token = checkCodeTextBox.Text.Trim();
            if(string.IsNullOrEmpty(active_token))
            {
                checkCodeTextBox.Focus(FocusState.Programmatic);
                return;
            }
            HttpResponseMessage response = await APIHelper.BindEmail( (Application.Current as App).user_name,(App.Current as App).user_login_token,active_token);
            if(response!=null)
            {
                Result result = Functions.Deserlialize<Result>(response.Content.ToString());
                if(result!=null)
                {
                    if(result.result.Equals("true"))
                    {
                        (App.Current as App).user_email_status = "1";
                    }
                    Functions.ShowMessage(result.message);
                }
            }
        }
    }
}
