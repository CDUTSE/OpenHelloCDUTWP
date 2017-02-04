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
    public sealed partial class BindLib : Page
    {
        public BindLib()
        {
            this.InitializeComponent();

     
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);
        }

        /// <summary>
        /// 绑定图书馆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void bindLibBtn_Click(object sender, RoutedEventArgs e)
        {
            string user_name = (Application.Current as App).user_name;
            string user_login_token = (Application.Current as App).user_login_token;
            string account = txtLibAccount.Text.Trim();
            string password = txtLibPwdBox.Text.Trim();
            if(string.IsNullOrEmpty(account))
            {
                txtLibAccount.Focus(FocusState.Programmatic);
                return;
            }
            if(string.IsNullOrEmpty(password))
            {
                txtLibPwdBox.Focus(FocusState.Programmatic);
                return;
            }
            HttpResponseMessage response = await APIHelper.BindLib(user_name, user_login_token, account, password);
            if (response != null)
            {
                Result result = Functions.Deserlialize<Result>(response.Content.ToString());
                if (result != null)
                {
                    if(result.result.Equals("true"))
                    {
                        (App.Current as App).user_lib_status = "1";
                    }
                    Functions.ShowMessage(result.message);
                }
            }
        }
    }
}
