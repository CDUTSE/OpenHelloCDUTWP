using DataHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using 你好理工.DataHelper.Helper;
using 你好理工.DataHelper.Model;
using 你好理工.View.CustomContentDialog;
using 你好理工.View.Me;
using 你好理工.View.School;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.School
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CampusCard : Page
    {
        public CampusCard()
        {
            this.InitializeComponent();
            
            consumeInfos = new ObservableCollection<ConsumeInfo>();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            
        }

        
        private ScrollViewer scrollViewer;
        private int recordIndex = 1;   //消费记录翻页
        private int recordTotalIndex = 1;
        
        private CampusCardCostInfo cardCostInfo;
        
        private ObservableCollection<ConsumeInfo> consumeInfos { get; set; }  //消费记录

        private bool IsRecordLoaded = false;    //消费记录是否已加载
        private bool IsStatisticLoaded = false;  //消费统计是否已加载

        private List<Item> campusCardInfoItems;
        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);
            //CampusCardContentDialog contentDialog = new CampusCardContentDialog();
            //ContentDialogResult result = await contentDialog.ShowAsync();
        }

        //绑定
        private async void bindBtn_Click(object sender, RoutedEventArgs e)
        {
            string user_name = (Application.Current as App).user_name;
            string user_login_token = (Application.Current as App).user_login_token;

            string account = String.Empty;   //一卡通账号
            string password = String.Empty; //一卡通密码
            string captcha = String.Empty;   //验证码
            string flag = String.Empty;         //标识位
            HttpResponseMessage response = await APIHelper.BindCampus(user_name, user_login_token, account, password, "true", flag);
        }
        //取消
        private void cancleBtn_Click(object sender, RoutedEventArgs e)
        {
            bindCampusPopup.IsOpen = false;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsStatisticLoaded)
            {
                await ShowAuthCode();
            }
        }

        /// <summary>
        /// 显示验证码
        /// </summary>
        /// <returns></returns>
        private async Task ShowAuthCode()
        {
            authGridBorder.Width = this.ActualWidth;
            authGridBorder.Height = this.ActualHeight;
            getAuthGrid.Width = this.ActualWidth;
            getAuthCodePopup.IsOpen = true;
            //获取一卡通登录验证码
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

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            pivot.SelectedIndex = ((e.OriginalSource as RadioButton).TabIndex);
            
        }

        private async void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(pivot.SelectedIndex)
            {
                case 0:     //消费统计 用户信息
                    appbar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    //radioButton0.IsChecked = true;
                    break;
                case 1:
                    appbar.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    if (!IsRecordLoaded)
                    {
                        await QueryConsumeInfo();
                        //radioButton1.IsChecked = true;
                        IsRecordLoaded = true ;
                    }
                    break;
                case 2:
                    appbar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    //radioButton2.IsChecked = true;
                    break;
            }
        }

        /// <summary>
        /// 刷新验证码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await GetCheckCode();
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

        /// <summary>
        /// 消费查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void search_Click(object sender, RoutedEventArgs e)
        {
            //获取开始和结束时间
            string start_time = startDatePicker.Date.ToString("yyyy-MM-dd");
            string end_time = endDatePicker.Date.ToString("yyyy-MM-dd");
            string jump_page = string.Empty;

            string search_type = string.Empty;

            switch(typeCbBox.SelectedIndex)
            {
                case 0:     //存款信息
                    search_type = "0";       
                    break;
                case 1:     //消费信息
                    search_type = "1";
                    break;
                case 2:     //交易汇总
                    search_type = "2";
                    break;
            }
            this.Frame.Navigate(typeof(CampusCardSearch), new NavigateParameter()
            {
              
                start_time = start_time,
                end_time = end_time,
                search_type = search_type
            });
        }

        /// <summary>
        /// 查询所有的一卡通消费记录
        /// </summary>
        private async Task<bool> QueryConsumeInfo()
        {
            HttpResponseMessage response = await APIHelper.QueryConsumeInfo((App.Current as App).user_name, (App.Current as App).user_login_token,
                "2013-9-10", DateTime.Now.ToString("yyyy-MM-dd"), recordIndex.ToString());
            if (response == null || response.Content == null)
            {
                progressRing1.IsActive = false;
                return false;
            }
            CampusCardConsumInfo cardConsumInfo = Functions.Deserlialize<CampusCardConsumInfo>(response.Content.ToString());
            if(cardConsumInfo==null)
            {
                progressRing1.IsActive = false;
                return false;
            }
            if(cardConsumInfo.result.Equals("false") && cardConsumInfo.message.Equals("time_out"))
            {
                await ShowAuthCode();
                return false;
            }
            if(cardConsumInfo.consume_info!=null && cardConsumInfo.consume_info.Length>0)
            {
                recordTotalIndex = cardConsumInfo.total_page;
                foreach (ConsumeInfo item in cardConsumInfo.consume_info)
                {
                    consumeInfos.Add(item);
                }
            }

            recordListView.ItemsSource = consumeInfos;
            progressRing1.IsActive = false;
            return true;
            
            
             
        }

        /// <summary>
        /// 查询一卡通交易汇总信息
        /// </summary>
        private async void QueryCustStateInfo()
        {
            HttpResponseMessage response = await APIHelper.QueryCustStateInfo((App.Current as App).user_name, (App.Current as App).user_login_token,
                "2013-9-10", DateTime.Now.ToString("yyyy-MM-dd"));
            if (response != null && response.Content != null)
            {
                cardCostInfo = Functions.Deserlialize<CampusCardCostInfo>(response.Content.ToString());
                if (cardCostInfo == null || cardCostInfo.result.Equals("false"))
                {
                    Functions.ShowMessage(cardCostInfo.message);
                    return;
                }
                campusCardInfoItems.Add(new Item() { Title = "餐费支出", Desc = cardCostInfo.wallet_deals_amount.ToString() });
                campusCardInfoItems.Add(new Item() { Title = "沐浴支出", Desc = cardCostInfo.shower_amount.ToString() });
                campusCardInfoItems.Add(new Item() { Title = "购物支出", Desc = cardCostInfo.shopping_amount.ToString() });
                campusCardInfoItems.Add(new Item() { Title = "公交支出", Desc = cardCostInfo.bus_amount.ToString() });
                campusCardInfoItems.Add(new Item() { Title = "总计", Desc = cardCostInfo.total_amount.ToString() });
                statisticListView.ItemsSource = campusCardInfoItems;
                //dealsTextBlock.Text = cardCostInfo.wallet_deals_amount.ToString();
                //showerTextBlock.Text = cardCostInfo.shower_amount.ToString();
                //shoppingTextBlock.Text = cardCostInfo.shower_amount.ToString();
                //busTextBlock.Text = cardCostInfo.bus_amount.ToString();
                //totalTextBlock.Text = cardCostInfo.total_amount.ToString();
            }
        }

     
         
        /// <summary>
        /// 验证码输入确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void captchaBtn_Click(object sender, RoutedEventArgs e)
        {
            string captcha = captchaTextBlock.Text.Trim();
            if(string.IsNullOrEmpty(captcha))
            {
                captchaTextBlock.Focus(FocusState.Programmatic);

                return;
            }
            progressRing0.IsActive = true;

            HttpResponseMessage response = await APIHelper.CampusUserLogin((App.Current as App).user_name, (App.Current as App).user_login_token, captcha);
            if(response!=null && response.Content!=null)
            {
                CampusCardInfo cardInfo = Functions.Deserlialize<CampusCardInfo>(response.Content.ToString());
                if(cardInfo!=null && cardInfo.result.Equals("true"))
                {
                    campusCardInfoItems = new List<Item>();
                    campusCardInfoItems.Add(new Item() { Title = "用户名", Desc = cardInfo.user_real_name });
                    campusCardInfoItems.Add(new Item() { Title = "卡号", Desc = cardInfo.user_stu_num });
                    campusCardInfoItems.Add(new Item() { Title = "一卡通状态", Desc = cardInfo.user_card_status });
                    campusCardInfoItems.Add(new Item() { Title = "性别", Desc = cardInfo.user_gender });
                    campusCardInfoItems.Add(new Item() { Title = "主钱包余额", Desc = cardInfo.user_card_balance });
                    campusCardInfoItems.Add(new Item() { Title = "补助余额", Desc = cardInfo.user_subsidy_balance });

                    
                    //realNameTextBlock.Text = cardInfo.user_real_name;
                    //stuNumTextBlock.Text = cardInfo.user_stu_num;
                    //statusTextBlock.Text = cardInfo.user_card_status;
                    //genderTextBlock.Text = cardInfo.user_gender;
                    //cardBalanceTextBlock.Text = cardInfo.user_card_balance;
                    //subsidyTextBlock.Text = cardInfo.user_subsidy_balance;
                    
                    QueryCustStateInfo();
                    getAuthCodePopup.IsOpen = false;

                    IsStatisticLoaded = true;  
                }
                else if(cardInfo.result.Equals("false"))    //超时，重新输入验证码
                {
                    Functions.ShowMessage(cardInfo.message);
                     
                    await GetCheckCode();
                }
            }
           
            progressRing0.IsActive = false;
        }

        private void recordListView_Loaded(object sender, RoutedEventArgs e)
        {
            scrollViewer = Functions.FindChildOfType<ScrollViewer>(recordListView);
            scrollViewer.ViewChanged += scrollViewer_ViewChanged;
        }

        async void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (scrollViewer != null)
            {
                if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight)  //ListView滚动到底
                {
                    var statusBar = StatusBar.GetForCurrentView();
                    statusBar.ProgressIndicator.Text = "正在加载更多";
                    await statusBar.ProgressIndicator.ShowAsync();
                    progressRing1.IsActive = true;

                    if (recordIndex > recordTotalIndex)
                    {
                        statusBar.ProgressIndicator.Text = "没有更多数据";
                        await Task.Delay(1000);
                        await statusBar.ProgressIndicator.HideAsync();
                        progressRing1.IsActive = false;
                        return;
                    }
                    recordIndex++;

                    if (await QueryConsumeInfo())
                    {
                        await statusBar.ProgressIndicator.HideAsync();
                    }
                    else
                    {
                        statusBar.ProgressIndicator.Text = "没有更多数据";
                        statusBar.ProgressIndicator.ProgressValue = 0;
                        await Task.Delay(1000);
                        await statusBar.ProgressIndicator.HideAsync();
                    }
                    progressRing1.IsActive = false;
                }
            }
        }

        private void recordListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            Debug.WriteLine("ContainerContentChanging " + "已调用此容器和数据项的次数 " + args.Phase + "ItemsSource中的索引 " + args.ItemIndex);
      
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void refreshAppbar_Click(object sender, RoutedEventArgs e)
        {
            switch(pivot.SelectedIndex)
            {
                case 0:     //消费统计 用户信息

                   
                    break;
                case 1:
                    progressRing1.IsActive = true;
                    consumeInfos.Clear();
                    recordIndex = 1;
                    await QueryConsumeInfo();
                    progressRing1.IsActive = false;
                    break;
                case 2:

                    break;
            }
        }
    }
}
