using DataHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using 你好理工.DataHelper.Helper;
using 你好理工.DataHelper.Model;

// “内容对话框”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.View.CustomContentDialog
{
    public sealed partial class CampusCardContentDialog : ContentDialog
    {
        public CampusCardContentDialog()
        {
            this.InitializeComponent();
        }
        public CampusCardInfo campusCardInfo;
        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string captcha = captchaTextBlock.Text.Trim();
            if (string.IsNullOrEmpty(captcha))
            {
                captchaTextBlock.Focus(FocusState.Programmatic);

                return;
            }
            HttpResponseMessage response = await APIHelper.CampusUserLogin((App.Current as App).user_name, (App.Current as App).user_login_token, captcha);
            if (response != null && response.Content != null)
            {
                CampusCardInfo cardInfo = Functions.Deserlialize<CampusCardInfo>(response.Content.ToString());
                campusCardInfo = cardInfo;
                if (cardInfo != null)
                {
                    //realNameTextBlock.Text = cardInfo.user_real_name;
                    //stuNumTextBlock.Text = cardInfo.user_stu_num;
                    //statusTextBlock.Text = cardInfo.user_card_status;
                    //genderTextBlock.Text = cardInfo.user_gender;
                    //cardBalanceTextBlock.Text = cardInfo.user_card_balance;
                    //subsidyTextBlock.Text = cardInfo.user_subsidy_balance;
                }
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await GetCheckCode();
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
            using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(imgBytes);
                    await writer.StoreAsync();
                }
                bitmap = new BitmapImage();
                bitmap.SetSource(ms);
            }
            return bitmap;

        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        private async Task GetCheckCode()
        {
            HttpResponseMessage response = await APIHelper.GetAuthCode((App.Current as App).user_name, (App.Current as App).user_login_token);
            if (response != null)
            {
                Result result = Functions.Deserlialize<Result>(response.Content.ToString());
                if (result != null)
                {
                    authCodeImage.Source = Base64ToImage(result.captcha).Result;
                }
            }
        }

        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            await GetCheckCode();
        }
    }
}
