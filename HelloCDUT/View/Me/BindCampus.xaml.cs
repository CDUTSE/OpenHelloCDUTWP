using System;
using System.Threading.Tasks;
using Windows.Phone.UI.Input;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
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
    public sealed partial class BindCampus : Page
    {
        public BindCampus()
        {
            this.InitializeComponent();

          

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// 页面加载时获取验证码
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);
            await GetCheckCode();
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        private async Task GetCheckCode()
        {
            HttpResponseMessage response = await APIHelper.BindCampus((Application.Current as App).user_name, (Application.Current as App).user_login_token,
                "", "", "", "");
            if (response != null)
            {
                Result result = Functions.Deserlialize<Result>(response.Content.ToString());
                if (result != null)
                {
                    checkImage.Source = Base64ToImage(result.captcha).Result;
                }
            }
            flag = "true";
        }

        /// <summary>
        /// Base64转图片
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        private async Task<BitmapImage> Base64ToImage(string base64)
        {
            byte[] imgBytes = Convert.FromBase64String(base64);
            BitmapImage bitmap = null;
            using(InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                using(DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(imgBytes);
                    await writer.StoreAsync();
                }
                bitmap = new BitmapImage();
                bitmap.SetSource(ms);
            }
            return bitmap;
            
        }

        string flag = "false";
        /// <summary>
        /// 绑定一卡通
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void bindCampus_Click(object sender, RoutedEventArgs e)
        {
            string account = txtAccount.Text.Trim();
            string password = txtPassword.Password.Trim();
            string captcha = txtCaptcha.Text.Trim();

            if(string.IsNullOrEmpty(account))
            {
                txtAccount.Focus(FocusState.Programmatic);
                return;
            }
            else if (string.IsNullOrEmpty(password))
            {
                txtPassword.Focus(FocusState.Programmatic);
                return;
            }
            else if (string.IsNullOrEmpty(captcha))
            {
                txtCaptcha.Focus(FocusState.Programmatic);
                return;
            }
            HttpResponseMessage response = await APIHelper.BindCampus((Application.Current as App).user_name, (Application.Current as App).user_login_token,
                account, password, captcha, flag);
            if(response!=null)
            {
                Result result = Functions.Deserlialize<Result>(response.Content.ToString());
                if(result!=null)
                {
                    if(result.result.Equals("true"))
                    {
                        (App.Current as App).user_campus_status = "1";
                    }
                    Functions.ShowMessage(result.message);
                }
            }
        }

        /// <summary>
        /// 验证码刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            flag = "true";
            await GetCheckCode();
        }
    }
}
