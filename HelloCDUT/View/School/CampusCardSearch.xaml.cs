using DataHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using 你好理工.DataHelper.Helper;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.View.School
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CampusCardSearch : Page
    {
        public CampusCardSearch()
        {
            this.InitializeComponent();
           
            depositInfos = new ObservableCollection<DepositInfo>();
            consumeInfos = new ObservableCollection<ConsumeInfo>();
            tradeStatisticInfos = new ObservableCollection<PieData>();
        }

        private int jump_page = 1;
        private int totalIndex = 1;
        private string start_time = "2013-9-10";
        private string end_time = "2013-9-10";
        //private int search_type = 0;
        private ScrollViewer consumeScrollViewer;
        private ScrollViewer depositScrollViewer;
        private ObservableCollection<DepositInfo> depositInfos;
        private ObservableCollection<ConsumeInfo> consumeInfos;
        private ObservableCollection<PieData> tradeStatisticInfos;

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);

            NavigateParameter np = e.Parameter as NavigateParameter;
            if (np == null)
            {
                this.Frame.GoBack();
                return;
            }
            LoadData(np);
        }

        private async void LoadData(NavigateParameter np)
        {
            start_time = np.start_time;
            end_time = np.end_time;
         
            HttpResponseMessage response = null;
            switch (np.search_type)
            {
                case "0":     //存款信息
                    pageTitleTextBlock.Text = "存款信息";
                    consumeListView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    tradeStatisticsListView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    await GetDepositInfo(start_time, end_time, jump_page);
                    progressRing0.IsActive = false;
                    break;
                case "1":     //消费信息
                    pageTitleTextBlock.Text = "消费信息";
                    depositListView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    tradeStatisticsListView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    await GetConsumeInfo();
                    progressRing0.IsActive = false;
                    break;
                case "2":     //交易汇总
                    pageTitleTextBlock.Text = "交易信息";
                    depositListView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    consumeListView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    response = await APIHelper.QueryCustStateInfo((App.Current as App).user_name,
                        (App.Current as App).user_login_token, start_time, end_time);
                    if (response == null || response.Content == null)
                    {
                        progressRing0.IsActive = false;
                        return;
                    }
                    CampusCardCostInfo cardCostInfo = Functions.Deserlialize<CampusCardCostInfo>(response.Content.ToString());
                    if (cardCostInfo == null)
                    {
                        progressRing0.IsActive = false;
                        return;
                    }
                    if(cardCostInfo.result.Equals("true"))
                    { 
                        dealsTextBlock.Text = cardCostInfo.wallet_deals_amount.ToString();
                        showerTextBlock.Text = cardCostInfo.shower_amount.ToString();
                        shoppingTextBlock.Text = cardCostInfo.shopping_amount.ToString();
                        busTextBlock.Text = cardCostInfo.bus_amount.ToString();
                        totalTextBlock.Text = cardCostInfo.total_amount.ToString();

                        pieChart.Visibility = Windows.UI.Xaml.Visibility.Visible;

                        tradeStatisticInfos.Add(new PieData() { Title = "餐费支出", Desc = cardCostInfo.wallet_deals_amount });
                        tradeStatisticInfos.Add(new PieData() { Title = "沐浴支出", Desc = cardCostInfo.shower_amount });
                        tradeStatisticInfos.Add(new PieData() { Title = "购物支出", Desc = cardCostInfo.shopping_amount });
                        tradeStatisticInfos.Add(new PieData() { Title = "公交支出", Desc = cardCostInfo.bus_amount });

                        pieChart.DataSource = tradeStatisticInfos;
                    }
                    else
                    {
                        Functions.ShowMessage(cardCostInfo.message);
                    }
                    progressRing0.IsActive = false;
                    break;
            }
        }

        public class PieData
        {
            public string Title { get; set; }
            public double Desc { get; set; }
        }

        /// <summary>
        /// 存款ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void depositListView_Loaded(object sender, RoutedEventArgs e)
        {

            depositScrollViewer = Functions.FindChildOfType<ScrollViewer>(depositListView);
            if (depositScrollViewer != null)
            {
                depositScrollViewer.ViewChanged +=depositScrollViewer_ViewChanged;
            }
        }

        /// <summary>
        /// 加载存款信息
        /// </summary>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <param name="jump_page"></param>
        /// <returns>是否加载到新数据</returns>
        private async Task<bool> GetDepositInfo(string start_time, string end_time, int jump_page)
        {
            HttpResponseMessage response = await APIHelper.QueryDepositInfo((App.Current as App).user_name, (App.Current as App).user_login_token,
                        start_time, end_time, jump_page.ToString());
            if (response == null || response.Content == null) return false;

            CampusCardDepositInfo cardDepositInfo = Functions.Deserlialize<CampusCardDepositInfo>(response.Content.ToString());
            if (cardDepositInfo == null)
            {
                return false;
            }
            if (cardDepositInfo.result.Equals("true"))   //成功获取数据
            {
                totalIndex = cardDepositInfo.total_page;
                if (cardDepositInfo.deposit_info.Length > 0)
                {
                    foreach (DepositInfo item in cardDepositInfo.deposit_info)
                    {
                        depositInfos.Add(item);

                    }
                    depositListView.ItemsSource = depositInfos;
                    return true;
                }
            }
            else if (cardDepositInfo.result.Equals("false"))
            {
                Functions.ShowMessage(cardDepositInfo.message);
            }

            return false;
        }
    

        /// <summary>
        /// 加载消费信息
        /// </summary>
        /// <returns></returns>
        private async Task<bool> GetConsumeInfo()
        {
            
            HttpResponseMessage response = await APIHelper.QueryConsumeInfo((App.Current as App).user_name,
                (App.Current as App).user_login_token, start_time, end_time, jump_page.ToString());
            if(response==null || response.Content==null)
            {
                return false;
            }
            
            CampusCardConsumInfo cardConsumInfo = Functions.Deserlialize<CampusCardConsumInfo>(response.Content.ToString());
            if(cardConsumInfo==null)
            {
                return false;
            }
            if (cardConsumInfo.result.Equals("true"))
            {
                if (cardConsumInfo.consume_info.Length > 0)
                {
                    totalIndex = cardConsumInfo.total_page;
                    foreach (ConsumeInfo item in cardConsumInfo.consume_info)
                    {
                        consumeInfos.Add(item);
                    }
                    consumeListView.ItemsSource = consumeInfos;

                    return true;

                }
            }
            else if(cardConsumInfo.result.Equals("false"))
            {
                Functions.ShowMessage(cardConsumInfo.message);
            }
            return false;
        }



        private async void depositScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (depositScrollViewer == null)
            {
                return;
            }
            if (depositScrollViewer.VerticalOffset >= depositScrollViewer.ScrollableHeight)  //ListView滚动到底
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.ProgressIndicator.Text = "正在加载更多";
                await statusBar.ProgressIndicator.ShowAsync();
                progressRing0.IsActive = true;

                if (jump_page > totalIndex)
                {
                    statusBar.ProgressIndicator.Text = "没有更多数据了";
                    await Task.Delay(1000);
                    await statusBar.ProgressIndicator.HideAsync();
                    progressRing0.IsActive = false;
                    return;
                }
                jump_page++;

                if (await GetDepositInfo(start_time,end_time,jump_page))
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

                progressRing0.IsActive = false;
            }
        }

        /// <summary>
        /// 消费ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recordListView_Loaded(object sender, RoutedEventArgs e)
        {

            consumeScrollViewer = Functions.FindChildOfType<ScrollViewer>(consumeListView);
            if (consumeScrollViewer != null)
            {
                consumeScrollViewer.ViewChanged += consumeScrollViewer_ViewChanged;
            }
        }

        private async void consumeScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (consumeScrollViewer == null)
            {
                return;
            }
            if (consumeScrollViewer.VerticalOffset >= consumeScrollViewer.ScrollableHeight)  //ListView滚动到底
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.ProgressIndicator.Text = "正在加载更多";
                await statusBar.ProgressIndicator.ShowAsync();
                progressRing0.IsActive = true;

                if (jump_page > totalIndex)
                {
                    statusBar.ProgressIndicator.Text = "没有更多数据了";
                    await Task.Delay(1000);
                    await statusBar.ProgressIndicator.HideAsync();
                    progressRing0.IsActive = false;
                    return;
                }
                jump_page++;

                if (await GetConsumeInfo())
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

                progressRing0.IsActive = false;
            }
        }
    }
}
