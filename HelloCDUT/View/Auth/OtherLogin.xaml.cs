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

namespace 你好理工.View.Auth
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class OtherLogin : Page
    {

        private bool IsGetCodeByEmailClicked = false;   //是否已经点击了发送验证码

        public OtherLogin()
        {
            this.InitializeComponent();
             
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);
        }
        
        /// <summary>
        /// 通过邮箱获取重置密码验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void getCodeByEmail_Click(object sender, RoutedEventArgs e)
        {
            string user_email = emailTextBox.Text.Trim();
            if(string.IsNullOrEmpty(user_email))
            {
                Functions.ShowMessage("邮箱不能为空");
                emailTextBox.Focus(FocusState.Programmatic);
                return;
            }
            HttpResponseMessage response = await APIHelper.GetResetUserPasswordTokenByEmail(user_email);
            if(response!=null && response.Content!=null)
            {
                string responseContent = response.Content.ToString();
                Result result =  Functions.Deserlialize<Result>(responseContent);
                if(result!=null)
                {
                    Functions.ShowMessage(result.message);
                }
            }
        }

        /// <summary>
        /// 通过绑定的教务系统找回密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void resetPwdByAAO_Click(object sender, RoutedEventArgs e)
        {
            string aao_account = aaoAccountTextBox.Text.Trim();
            string aao_password = aaoPasswordPwdBox.Password.Trim();
            string new_password = newPasswordPwdBox.Password.Trim();
            if(String.IsNullOrEmpty(aao_account))
            {
                aaoAccountTextBox.Focus(FocusState.Programmatic);
                return;
            }
            else if(  String.IsNullOrEmpty(aao_password))
            {
                aaoPasswordPwdBox.Focus(FocusState.Programmatic);
                return;
            }
            else if( String.IsNullOrEmpty(new_password))
            {
                newPasswordPwdBox.Focus(FocusState.Programmatic);
                return;
            }
            HttpResponseMessage response = await APIHelper.ResetUserPasswordByAAO(aao_account,new_password,new_password);
            if(response!=null)
            {
                string responseContent = response.Content.ToString();
                Result result = Functions.Deserlialize<Result>(responseContent);
                if(result!=null)
                {
                    Functions.ShowMessage(result.message);
                }
            }

        }

       /// <summary>
       /// 通过绑定邮箱找回密码
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private async void findPwdByEmail_Click(object sender, RoutedEventArgs e)
        {
            string user_email = emailTextBox.Text.Trim();
            string validate_code = validateCodeTextBox.Text.Trim();
            string new_password = newPwdTextBox.Text.Trim();
            if(string.IsNullOrEmpty(user_email) )
            {
                emailTextBox.Focus(FocusState.Programmatic);
                return;
            }
            else if( string.IsNullOrEmpty(validate_code))
            {
                validateCodeTextBox.Focus(FocusState.Programmatic);
                return;
            }
            else if(string.IsNullOrEmpty(new_password))
            {
                newPasswordPwdBox.Focus(FocusState.Programmatic);
                return;
            }
            HttpResponseMessage response = await APIHelper.ResetUserPasswordByEmail(user_email, validate_code, new_password);
            if(response!=null && response.Content!=null)
            {
                Result result = Functions.Deserlialize<Result>(response.Content.ToString());
                if(result!=null)
                {
                    Functions.ShowMessage(result.message);
                }
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if((e.OriginalSource as RadioButton).TabIndex==0)   //教务
            {
                pivot.SelectedIndex = 0;
            }
            else
            {
                pivot.SelectedIndex = 1;
            }
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             switch(pivot.SelectedIndex)
             {
                 case 0:
                     //radioButton0.IsChecked = true;
                     break;
                 case 1:
                     //radioButton1.IsChecked = true;
                     break;
             }
        }
    }
}
