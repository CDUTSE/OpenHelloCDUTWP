using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using 你好理工.DataHelper.Helper;
using 你好理工.DataHelper.Model;
using Windows.UI.Popups;
using System.Collections.Generic;
using 你好理工.View;
using Windows.UI.Xaml.Input;
using Windows.System;
using Windows.Phone.UI.Input;
using Windows.UI.ViewManagement;
using System.Diagnostics;
using DataHelper.Model;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.View.Auth
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
          
            
        }
        StatusBar statusBar;
        private bool IsBackClicked = false;
        private async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            DispatcherTimer dt = new DispatcherTimer();
            if(IsBackClicked)
            {
                App.Current.Exit();
                dt.Stop();
            }
            else  //未点击过退出 增加3秒的定时器
            {
                IsBackClicked = true;
                
                
                statusBar.ProgressIndicator.Text = "3秒内再次点击后退键退出";
                statusBar.ProgressIndicator.ProgressValue = 0;
                await statusBar.ProgressIndicator.ShowAsync();

                dt.Interval = new TimeSpan(0, 0, 3);
                //超过三秒后
                dt.Tick +=  async(send, args) =>
                    {
                        IsBackClicked = false;
                        await statusBar.ProgressIndicator.HideAsync();
                        dt.Stop();
                    };
                dt.Start();
            }
          
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            statusBar = StatusBar.GetForCurrentView();

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Functions.ApplyDayModel(this);

            loginLoadingPop.Width = Frame.ActualWidth;
            loginLoadingPop.Height = Frame.ActualHeight;

            //加载用户账号和密码
            txtAccount.Text = Settings.Instance.UserAccount;
            txtPwd.Password = Settings.Instance.UserPassword;
        }
 
          
          /// <summary>
          /// 回车键按下事件
          /// </summary>
          /// <param name="e"></param>
          protected override void OnKeyDown(KeyRoutedEventArgs e)
          {
              if (e.Key == VirtualKey.Enter)
              {
                  if (String.IsNullOrEmpty(txtAccount.Text.Trim()))
                  {
                      txtAccount.Focus(FocusState.Programmatic);
                  }
                  else if (String.IsNullOrEmpty(txtPwd.Password.Trim()))
                  {
                      txtPwd.Focus(FocusState.Programmatic);
                  }
              }
          }


          private async void AppBarButton_Click(object sender, RoutedEventArgs e)
          {
              //loginLoadingPop.Visibility = Windows.UI.Xaml.Visibility.Visible;

      
              //AppName.Focus(FocusState.Programmatic);
              //appBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
              
              //获取输入
              string account = string.Empty;
              account = txtAccount.Text.Trim();
              string password = string.Empty;
              password = txtPwd.Password.Trim();
              if(string.IsNullOrEmpty(account))
              {
                  Functions.ShowMessage("用户名不能为空");
                  txtAccount.Focus(FocusState.Programmatic);
                  return;
              }
              if(string.IsNullOrEmpty(password))
              {
                  Functions.ShowMessage("密码不能为空");
                  txtPwd.Focus(FocusState.Programmatic);
                  return;
              }
              if (!Functions.CheckNetWork())
              {
                  Functions.ShowMessage("网络未连接");
                  return;
              }
              loadingGrid.Width = this.ActualWidth;
              loadingGrid.Height = this.ActualHeight;
              loadingGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
              try
              {
                  await UserLogin(account, password);
              }
              catch(Exception ex)
              {
                  Debug.WriteLine("Login.xaml.cs  UserLogin"+ex.Message);
              }
                  //loginLoadingPop.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
              loadingGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            
          }

         

          private async System.Threading.Tasks.Task UserLogin(string account, string password)
          {
              HttpResponseMessage response = await APIHelper.UserLogin(account, password);

              if (response != null && response.Content!=null)
              {
                  string responseText = response.Content.ToString();
                  if (responseText.Contains("user_login_token"))   //登录成功……
                  {
                      Token token = Functions.Deserlialize<Token>(responseText);
                      //全局保存token
                      (Application.Current as App).user_name = token.user_name;
                      (Application.Current as App).user_login_token = token.user_login_token;
                      (Application.Current as App).user_chat_token = token.user_chat_token;
                      (Application.Current as App).user_aao_status = token.user_aao_status;
                      (Application.Current as App).user_campus_status = token.user_campus_status;
                      (Application.Current as App).user_lib_status = token.user_lib_status;
                      (Application.Current as App).user_email_status = token.user_email_status;
                      (Application.Current as App).user_nick_name = token.user_nick_name;
                      (Application.Current as App).user_password_hash = token.user_password_hash;


                      (Application.Current as App).user_stu_id = token.user_stu_id;
                      (Application.Current as App).user_email = token.user_email;
                      (Application.Current as App).user_gender = token.user_gender;
                      (Application.Current as App).user_motto = token.user_motto;
                      (Application.Current as App).user_love_status = token.user_love_status;
                      (Application.Current as App).user_sex_orientation = token.user_sex_orientation;
                      (Application.Current as App).user_real_name = token.user_real_name;
                      (Application.Current as App).user_birthdate = token.user_birthdate;
                      (Application.Current as App).user_institute = token.user_institute;
                      (Application.Current as App).user_major = token.user_major;
                      (Application.Current as App).user_class_id = token.user_class_id;
                      (Application.Current as App).user_entrance_year = token.user_entrance_year;
                      (Application.Current as App).user_avatar_url = token.user_avatar_url;
                      (Application.Current as App).user_love_status_permission = token.user_love_status_permission;
                      (Application.Current as App).user_sex_orientation_permission = token.user_love_status_permission;
                      (Application.Current as App).user_real_name_permission = token.user_real_name_permission;
                      (Application.Current as App).user_birthdate_permission = token.user_birthdate_permission;
                      (Application.Current as App).user_stu_num_permission = token.user_stu_num_permission;
                      (Application.Current as App).user_institute_id_permission = token.user_institute_id_permission;
                      (Application.Current as App).user_major_permission = token.user_major_permission;
                      (Application.Current as App).user_class_id_permission = token.user_class_id_permission;
                      (Application.Current as App).user_entrance_year_permission = token.user_entrance_year_permission;
                      (Application.Current as App).user_schedule_permission = token.user_schedule_permission;
                      (Application.Current as App).user_group_schedule_permission = token.user_schedule_permission;

                      //保存用户账号和密码
                      Settings.Instance.UserAccount = account;
                      Settings.Instance.UserPassword = password;

                      this.Frame.Navigate(typeof(MainPage));
                  }
                  else
                  {
                      ResultBase result = Functions.Deserlialize<ResultBase>(responseText);
                      if (result != null && result.message!=null)
                      {
                          Functions.ShowMessage(result.message);
                      }
                      //this.Frame.Navigate(typeof(MainPage));

                      loadingGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                  }
              }
          }

        /// <summary>
        /// 新用户注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
          private void registerHyperLink_Click(object sender, RoutedEventArgs e)
          {
              this.Frame.Navigate(typeof(Register));
          }

        /// <summary>
        /// 无法登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
          private void notLoginHyperLink_Click(object sender, RoutedEventArgs e)
          {
              this.Frame.Navigate(typeof(OtherLogin));
          }
    }
}
