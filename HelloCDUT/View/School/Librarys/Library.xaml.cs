using DataHelper.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using 你好理工.DataHelper.Helper;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.School.Librarys
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Library : Page
    {
        public Library()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;

            currentBooks = new ObservableCollection<BorrowedBook>();
            borrowedBooks = new ObservableCollection<book>();
        }

        private ScrollViewer currentBorrowedScrollViewer;
        private ScrollViewer historyListScrollViewer;

        public ObservableCollection<book> borrowedBooks;    //借阅历史
        public ObservableCollection<BorrowedBook> currentBooks;   //当前借阅
        public ObservableCollection<Item> userInfoItems { get; set; }

        private int historyIndex = 1;   //借阅历史翻页数
        private int historyTotalPage = 1;  //借阅历史总页数

        private int currentBorrowedIndex = 1;   //当前借阅翻页数
        private int currentBorrowedTotalPage = 1;   //当前借阅总页数

       

        private bool IsBorrowedBooksLoaded = false; //借阅历史是否已加载
        private bool IsBorrowingBooksLoaded = false; //当前借阅是否已加载
        private bool IsReaderInfoLoaded = false;        //读者个人信息是否已加载

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
        /// 获取读者个人信息
        /// </summary>
        private async Task LoadReaderInfo()
        {
            HttpResponseMessage response = await APIHelper.QueryLibInfo((App.Current as App).user_name, (App.Current as App).user_login_token, "1",
                "", "");
            if(response!=null && response.Content!=null)
            {
                ReaderInfo readerInfo = Functions.Deserlialize<ReaderInfo>(response.Content.ToString());
                if(readerInfo!=null)
                {
                    if(readerInfo.result!=null && readerInfo.result.Equals("true"))
                    {
                        userInfoItems = new ObservableCollection<Item>();
                        userInfoItems.Add(new Item() { Title = "读者姓名", Desc = readerInfo.reader.readerName });
                        userInfoItems.Add(new Item() { Title = "借阅证号", Desc = readerInfo.reader.readerNum });
                        userInfoItems.Add(new Item() { Title = "学位", Desc = readerInfo.reader.readerDegree });
                        userInfoItems.Add(new Item() { Title = "所属学院", Desc = readerInfo.reader.readerUnit });

                        userInfoItems.Add(new Item() { Title = "欠赔款", Desc = readerInfo.reader.readerReparation.ToString() });
                        userInfoItems.Add(new Item() { Title = "欠罚款", Desc = readerInfo.reader.readerFine.ToString() });

                        userInfoItems.Add(new Item() { Title = "借阅数量", Desc = readerInfo.reader.readerBookCount.ToString() });
                        listView.ItemsSource = userInfoItems;
                    }
                }
            }
           
        }

        /// <summary>
        /// 获取当前借阅
        /// </summary>
        private async Task<bool> GetCurrentBook()
        {
            HttpResponseMessage response = await APIHelper.QueryLibInfo((App.Current as App).user_name, (App.Current as App).user_login_token, "2",
                "", "");
            if (response != null && response.Content != null)
            {
                CurrentBorrowedBook cbBooks = Functions.Deserlialize<CurrentBorrowedBook>(response.Content.ToString());
                if(cbBooks!=null && cbBooks.result.Equals("true"))
                {
                    foreach(BorrowedBook bBook in cbBooks.borrowed_book)
                    {
                        currentBooks.Add(bBook);
                    }
                    currentBorrowedListView.ItemsSource = currentBooks;
                    progressRing0.IsActive = false;
                    return true;
                }
                else
                { 
                    return false;
                }
            }
            progressRing0.IsActive = false;
            return false;
        }

        /// <summary>
        /// 获取借阅历史
        /// </summary>
        private async Task<bool> GetBorrowHistory()
        {
            HttpResponseMessage response = await APIHelper.QueryLibInfo((App.Current as App).user_name, (App.Current as App).user_login_token, "3",
                historyIndex.ToString(), "");
            if (response != null && response.Content != null)
            {
                BorrowHistory borrowHistory = Functions.Deserlialize<BorrowHistory>(response.Content.ToString());
                if(borrowHistory!=null && borrowHistory.history.borrow_history.Length>0)
                {
                    historyTotalPage =  borrowHistory.history.total_page;
                    foreach( book b in borrowHistory.history.borrow_history)
                    {
                        borrowedBooks.Add(b);
                    }

                    historyListView.ItemsSource = borrowedBooks;
                    progressRing1.IsActive = false;
                    return true;
                }
                else    //没有新数据了
                {
                    return false;
                }
            }
            progressRing1.IsActive = false;
            return false;
           
        }
         
        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(pivot!=null)
            {
                switch(pivot.SelectedIndex)
                {
                    case 0:
                        if (!IsBorrowingBooksLoaded)    //没加载则加载
                        {
                            await GetCurrentBook();
                            IsBorrowingBooksLoaded = true;
                            progressRing0.IsActive = false;
                        }
                        break;
                    case 1:
                        if (!IsBorrowedBooksLoaded)
                        {
                            await GetBorrowHistory();
                            IsBorrowedBooksLoaded = true;
                            progressRing1.IsActive = false;
                        }
                        break;
                    case 2:
                        if (!IsReaderInfoLoaded)
                        {
                            await LoadReaderInfo();
                            progressRing2.IsActive = false;
                            IsReaderInfoLoaded = true;
                        }
                        break;
                }
            }
        }

        private double oldVerticalOffset = 0.0;
        /// <summary>
        /// 历史借阅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void historyListScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (historyListScrollViewer != null)
            {
                oldVerticalOffset = historyListScrollViewer.VerticalOffset; 

                if (historyListScrollViewer.VerticalOffset >= historyListScrollViewer.ScrollableHeight)  //ListView滚动到底
                {
                    var statusBar = StatusBar.GetForCurrentView();
                    statusBar.ProgressIndicator.Text = "正在加载更多";
                    await statusBar.ProgressIndicator.ShowAsync();
                    progressRing1.IsActive = true;
                    
                    if(historyIndex>historyTotalPage)
                    {
                        statusBar.ProgressIndicator.Text = "没有更多数据";
                        await Task.Delay(1000);
                        await statusBar.ProgressIndicator.HideAsync();
                        progressRing1.IsActive = false;
                        return;
                    }
                    historyIndex++;
                    
                    if(await GetBorrowHistory())   
                    {
                        await statusBar.ProgressIndicator.HideAsync();
                    }
                    else
                    {
                        statusBar.ProgressIndicator.Text = "没有更多数据";
                        statusBar.ProgressIndicator.ProgressValue =0;
                        await Task.Delay(1000);
                        await statusBar.ProgressIndicator.HideAsync();
                    }
                    
                    progressRing1.IsActive = false;
                    
                }
            }
        }

        private void currentBorrowedListView_Loaded(object sender, RoutedEventArgs e)
        {
            //currentBorrowedScrollViewer = Functions.FindChildOfType<ScrollViewer>(currentBorrowedListView);
            //currentBorrowedScrollViewer.ViewChanged += currentBorrowedScrollViewer_ViewChanged;
        }

        private async void currentBorrowedScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (currentBorrowedScrollViewer != null)
            {
             
                if (currentBorrowedScrollViewer.VerticalOffset >= currentBorrowedScrollViewer.ScrollableHeight)  //ListView滚动到底
                {
                    var statusBar = StatusBar.GetForCurrentView();
                    statusBar.ProgressIndicator.Text = "正在加载更多";
                    await statusBar.ProgressIndicator.ShowAsync();
                    progressRing1.IsActive = true;

                    if (currentBorrowedIndex > currentBorrowedTotalPage)
                    {
                        statusBar.ProgressIndicator.Text = "没有更多数据";
                        await Task.Delay(1000);
                        await statusBar.ProgressIndicator.HideAsync();
                        progressRing1.IsActive = false;
                        return;
                    }
                    currentBorrowedIndex++;

                    if (await GetCurrentBook())
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

        private void historyListView_Loaded(object sender, RoutedEventArgs e)
        {
            historyListScrollViewer = Functions.FindChildOfType<ScrollViewer>(historyListView);
             
            historyListScrollViewer.ViewChanged += historyListScrollViewer_ViewChanged;

           
        }

        private async void refreshAppbar_Click(object sender, RoutedEventArgs e)
        {
            switch(pivot.SelectedIndex)
            {
                case 0:             //当前借阅
                    progressRing0.IsActive = true;
                    currentBooks.Clear();
                    currentBorrowedIndex = 1;
                    await GetCurrentBook();
                    progressRing0.IsActive = false;
                    break;
                case 1:             //借阅历史
                    progressRing1.IsActive = true;
                    borrowedBooks.Clear();
                    historyIndex = 1;
                    await GetBorrowHistory();
                    progressRing1.IsActive = false;
                    break;
                case 2:             //读者信息
                    progressRing2.IsActive = true;
                    userInfoItems.Clear();
                    await LoadReaderInfo();
                    progressRing2.IsActive = false;
                    break;
            }
        }

    }
}
