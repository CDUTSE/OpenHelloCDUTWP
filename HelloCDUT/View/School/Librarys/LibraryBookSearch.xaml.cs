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

namespace 你好理工.View.School.Librarys
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LibraryBookSearch : Page
    {
        public LibraryBookSearch()
        {
            this.InitializeComponent();
            
            searchBooks = new ObservableCollection<Book>();
        }

        private ObservableCollection<Book> searchBooks { get; set; }
        private ScrollViewer scrollViewer;
        private int searchIndex = 1;
        private int searchTotalIndex = 1;

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);
        }

        private async void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            string book_name = keywordTextBox.Text.Trim();
            if(string.IsNullOrEmpty(book_name))
            {
                keywordTextBox.Focus(FocusState.Programmatic);
                return;
            }
            string is_desc = string.Empty;
            //progressRing0.IsActive = true;
            loadingStackPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            HttpResponseMessage response =  await APIHelper.SearchBook((App.Current as App).user_name, (App.Current as App).user_login_token,
               book_name, is_desc);
            if(response!=null && response.Content!=null)
            {
                 
                SearchBook sBook = Functions.Deserlialize<SearchBook>(response.Content.ToString());
                if (sBook != null && sBook.books!=null)
                {
                    searchBooks.Clear();
                    searchTotalIndex = sBook.total_page;

                    foreach (Book item in sBook.books)
                    {
                        searchBooks.Add(item);
                    }
                    searchBookListView.ItemsSource = searchBooks;
                }
            }
            loadingStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //progressRing0.IsActive = false;
   
        }

        private void searchBookListView_Loaded(object sender, RoutedEventArgs e)
        {
            scrollViewer = Functions.FindChildOfType<ScrollViewer>(searchBookListView);
            if(scrollViewer!=null)
            {
                scrollViewer.ViewChanged += scrollViewer_ViewChanged;
            }
        }

        private async Task<bool> SearchBook()
        {
            HttpResponseMessage response = await APIHelper.BookJumpPage((App.Current as App).user_name, (App.Current as App).user_login_token, searchIndex.ToString());
            if (response == null || response.Content == null) return false;
            
            SearchBook sBook = Functions.Deserlialize<SearchBook>(response.Content.ToString());
            if(sBook==null)
            {
                loadingStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                return false;
            }
            if (sBook != null && sBook.books != null && sBook.books.Length > 0)
            {
                foreach (Book item in sBook.books)
                {
                    searchBooks.Add(item);
                }
                searchBookListView.ItemsSource = searchBooks;
                return true;
            }
            //progressRing0.IsActive = false;
            loadingStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            return false;
        }

        private async void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if(scrollViewer==null)
            {
                return;
            }
            if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight)  //ListView滚动到底
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.ProgressIndicator.Text = "正在加载更多";
                await statusBar.ProgressIndicator.ShowAsync();
                //progressRing0.IsActive = true;
                loadingStackPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;

                if (searchIndex >= searchTotalIndex)
                {
                    statusBar.ProgressIndicator.Text = "没有更多数据";
                    await Task.Delay(1000);
                    await statusBar.ProgressIndicator.HideAsync();
                    loadingStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    //progressRing0.IsActive = false;
                    return;
                }
                searchIndex++;

                if (await SearchBook())
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
                //progressRing0.IsActive = false;
                loadingStackPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private async void searchBookListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var app = App.Current as App;
            if(app==null)return;
            Book book = e.ClickedItem as Book;
            if (book == null) return;
            string book_detail_index = book.href_index;
            if (string.IsNullOrEmpty(book_detail_index)) return;
            HttpResponseMessage response = await APIHelper.QueryBookDetail(app.user_name, app.user_login_token, book_detail_index);
            if (response == null || response.Content == null) return;
            string content = response.Content.ToString();
            var bk = Functions.Deserlialize<Book>(content);
            book.location = bk.location;
            book.available = bk.available;
        }
    }
}
