using DataHelper.Model;
using DataHelper.Service;
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
using 你好理工.DataHelper.Helper;
using 你好理工.View.Auth;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AAONewsPage : Page
    {
        public AAONewsPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
            news = new ObservableCollection<News>();
        }

        private ScrollViewer scrollViewer;
        private AAONewsService service;
        private int currentPage = 1;
        private bool IsNewsLoaded = false;  //新闻是否已加载
        private ObservableCollection<News> news;

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if(!IsNewsLoaded)
            {
                Functions.ApplyDayModel(this);
                await LoadData();
                IsNewsLoaded = true;
            }
        }

        private async Task LoadData()
        {
            service = new AAONewsService();
            await service.GetNews();
            news = service.newsCollection;
            newsListView.ItemsSource = news;
            progressRing0.IsActive = false;
        }

        private void newsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            News news = e.ClickedItem as News;
            if (news == null) return;
            this.Frame.Navigate(typeof(Agreement), news);
        }

        private void newsListView_Loaded(object sender, RoutedEventArgs e)
        {
            scrollViewer = Functions.FindChildOfType<ScrollViewer>(newsListView);
            if(scrollViewer!=null)
            {
                scrollViewer.ViewChanged += scrollViewer_ViewChanged;
            }
        }

        private async void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (scrollViewer != null)
            {

                if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight)  //ListView滚动到底
                {
                    var statusBar = StatusBar.GetForCurrentView();
                    statusBar.ProgressIndicator.Text = "正在加载更多";
                    await statusBar.ProgressIndicator.ShowAsync();
                    progressRing0.IsActive = true;

                    //if (currentBorrowedIndex > currentBorrowedTotalPage)
                    //{
                    //    statusBar.ProgressIndicator.Text = "没有更多数据";
                    //    await Task.Delay(1000);
                    //    await statusBar.ProgressIndicator.HideAsync();
                    //    progressRing1.IsActive = false;
                    //    return;
                    //}
                   
                    //if (await GetCurrentBook())
                    //{
                    //    await statusBar.ProgressIndicator.HideAsync();
                    //}
                    //else
                    //{
                    //    statusBar.ProgressIndicator.Text = "没有更多数据";
                    //    statusBar.ProgressIndicator.ProgressValue = 0;
                    //    await Task.Delay(1000);
                    //    await statusBar.ProgressIndicator.HideAsync();
                    //}
                    currentPage++;
                    await service.GetNewsByPageNum(currentPage);
                    await statusBar.ProgressIndicator.HideAsync();
                    progressRing0.IsActive = false;

                }
            }
        }

        
        private async void refreshAppbar_Click(object sender, RoutedEventArgs e)
        {
            news.Clear();
            progressRing0.IsActive = true;
            await LoadData();
            progressRing0.IsActive = false;
        }
    }
}
