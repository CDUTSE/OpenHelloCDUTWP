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
    public sealed partial class AssociateAccounts : Page
    {
        public AssociateAccounts()
        {
            this.InitializeComponent();
          
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
        }

        private string aaoBindStatus;
        private string libBindStatus;
        private string emailBindStatus;
        private string campusCardStatus;

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            

            //更改Button的绑定状态
            aaoBindStatus = (Application.Current as App).user_aao_status;
            libBindStatus = (Application.Current as App).user_lib_status;
            emailBindStatus = (Application.Current as App).user_email_status;
            campusCardStatus = (Application.Current as App).user_campus_status;
            ChangeButtonStyle(aaoBtn, aaoBindStatus);
            ChangeButtonStyle(libBtn, libBindStatus);
            ChangeEmailButtonStyle(emailBtn, emailBindStatus);
            ChangeButtonStyle(campusCardBtn, campusCardStatus);

            Functions.ApplyDayModel(this);
            if (aaoBindStatus.Equals("1"))
            {
                stdIdTextBlock.Text = (Application.Current as App).user_stu_id==null?"":(App.Current as App).user_stu_id;
            }
            if(emailBindStatus.Equals("1"))
            {
                emailTextBlock.Text = (Application.Current as App).user_email==null?"":(App.Current as App).user_email;
                
            }
          
            
        }
        private void ChangeEmailButtonStyle(Button btn,string status)
        {

            if (status.Equals("1"))     //已绑定
            {
                btn.Style = this.Resources["BindStyle"] as Style;
                btn.Content = "解绑";
                btn.IsEnabled = false;
            }
            else
            {
                btn.Style = this.Resources["UnBindStyle"] as Style;
                btn.Content = "绑定";
            }
        }

        private void ChangeButtonStyle(Button btn,string status)
        {
            if (status.Equals("1"))     //已绑定
            {
                btn.Style = this.Resources["BindStyle"] as Style;
                btn.Content = "解绑";
            }
            else
            {
                btn.Style = this.Resources["UnBindStyle"] as Style;
                btn.Content = "绑定";
            }
        }

        /// <summary>
        /// 图书馆的绑定和解绑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void libBtn_Click(object sender, RoutedEventArgs e)
        {
            if( (Application.Current as App).user_lib_status.Equals("1"))   //已绑定图书馆
            {
                HttpResponseMessage response = await APIHelper.UnbindLib((Application.Current as App).user_name, (Application.Current as App).user_login_token);
                if(response!=null && response.Content!=null)
                {
                    Result result = Functions.Deserlialize<Result>(response.Content.ToString());
                    if(result!=null)
                    {
                        if(result.result.Equals("true"))
                        {
                            (App.Current as App).user_lib_status = "0";
                        }
                        Functions.ShowMessage(result.message);
                    }
                }
            }
            else
            {
                this.Frame.Navigate(typeof(BindLib));
            }
        }

        /// <summary>
        /// 一卡通的绑定与解绑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void campusCardBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((Application.Current as App).user_campus_status.Equals("1"))   //已绑定一卡通
            {
                HttpResponseMessage response = await APIHelper.UnbindCampus((Application.Current as App).user_name, (Application.Current as App).user_login_token);
                if(response!=null && response.Content!=null)
                {
                    Result result = Functions.Deserlialize<Result>(response.Content.ToString());
                    if(result!=null)
                    {
                        if(result.result.Equals("true"))
                        {
                            (App.Current as App).user_campus_status = "0";
                        }
                        Functions.ShowMessage(result.message);
                    }

                }
            }
            else
            {
                this.Frame.Navigate(typeof(BindCampus));
            }
        }

        /// <summary>
        /// 邮箱的绑定与解绑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void emailBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((Application.Current as App).user_email_status.Equals("1"))   //已绑定邮箱
            {
                //邮箱解绑
                popGrid.Width = this.ActualWidth;
                emailPopup.IsOpen = true;
               
            }
            else
            {
                this.Frame.Navigate(typeof(BindEmail));
            }
        }

        /// <summary>
        /// 学号的绑定和解绑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void aaoBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((Application.Current as App).user_aao_status.Equals("1"))   //已绑定教务处
            {
                HttpResponseMessage response = await APIHelper.UnbindAAO((Application.Current as App).user_name, (Application.Current as App).user_login_token);
                if(response!=null && response.Content!=null)
                {
                    Result result = Functions.Deserlialize<Result>(response.Content.ToString());
                    if(result!=null)
                    {
                        if(result.result.Equals("true"))
                        {
                            (App.Current as App).user_aao_status = "0";
                        }
                    }
                    Functions.ShowMessage(result.message);

                }
            }
            else
            {
                this.Frame.Navigate(typeof(BindAAO));
            }
        }


        private void cancleBtn_Click(object sender, RoutedEventArgs e)
        {
            emailPopup.IsOpen = false;
        }

        /// <summary>
        /// 邮箱解绑确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void yesBtn_Click(object sender, RoutedEventArgs e)
        {
            string user_email = emailTextBox.Text.Trim();
            if(string.IsNullOrEmpty(user_email))
            {
                emailTextBox.Focus(FocusState.Programmatic);
                return;
            }
            HttpResponseMessage response = await APIHelper.UnbindEmail((Application.Current as App).user_name, (Application.Current as App).user_login_token, user_email);
            if (response != null)
            {
                Result result = Functions.Deserlialize<Result>(response.Content.ToString());
                if (result != null)
                {
                    if(result.result.Equals("true"))   //解绑成功
                    {
                        (App.Current as App).user_email_status = "0";
                    }
                    Functions.ShowMessage(result.message);
                }
            }
        }
    }
}
