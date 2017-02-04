using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace 你好理工.View.Scholl.Search
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class GradeSearch : Page
    {
        public GradeSearch()
        {
            this.InitializeComponent();
          
        }

        
        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);
            await LoadingGrade();
        }

        private Grade _grade = null;
        /// <summary>
        /// 加载课程数据
        /// </summary>
        /// <returns></returns>
        private async Task LoadingGrade()
        {
            loadingGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            HttpResponseMessage response = await APIHelper.QueryGrade((Application.Current as App).user_name, (Application.Current as App).user_login_token);
            if (response != null && response.Content != null)
            {
                Grade grade = Functions.Deserlialize<Grade>(response.Content.ToString());
                if (grade != null)
                {
                    _grade = grade; //引用赋值
                    gradeListView.ItemsSource = grade.subject;
                    gpaAppbar.Label = "gba："+grade.gpa;
                    cgpaAppbar.Label = "cgba："+grade.cgpa;
                }
            }
            loadingGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
        private Flyout sortFlyout;
        private void sortAppbar_Click(object sender, RoutedEventArgs e)
        {
            //From https://github.com/timgabrhel/WinRT.Flyouts
            if (BottomAppBar == null) return;

            var sortButton = sender as AppBarButton;
            if (sortButton == null) return;

            sortFlyout = (Flyout)Resources["SortFlyout"];
            if (sortFlyout == null) return;

            sortFlyout.Opened += delegate(object o, object o2)
            {
                if (BottomAppBar != null && BottomAppBar.Visibility == Windows.UI.Xaml.Visibility.Visible)
                {
                    BottomAppBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            };

            sortFlyout.Closed += delegate(object o, object o2)
            {
                if (BottomAppBar != null && BottomAppBar.Visibility == Windows.UI.Xaml.Visibility.Collapsed)
                {
                    BottomAppBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
            };

            var grid = sortFlyout.Content as Grid;
            if (grid == null) return;
            grid.Tapped += delegate(object o, TappedRoutedEventArgs args)
                {
                    var transparentGrid = args.OriginalSource as Grid;
                    if (transparentGrid != null)
                    {
                        sortFlyout.Hide();
                    }
                };
            sortFlyout.ShowAt(BottomAppBar);
            //sortListBox.ItemsSource = sortList;
        }

        //是否升序
        private bool IsTimeSortAscending = true;
        private bool IsGradeSortAscending = true;
        /// <summary>
        /// 按时间排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeSort_Click(object sender, RoutedEventArgs e)
        {
            if (_grade == null)
            {
                return;
            }
            List<subjectItem> subjects = _grade.subject;

            if (!IsTimeSortAscending)
            {
                var query = from s in subjects orderby s.storage_time ascending select s;
                gradeListView.ItemsSource = query.ToList<subjectItem>();
            }
            else
            {
                var query = from s in subjects orderby s.storage_time descending select s;
                gradeListView.ItemsSource = query.ToList<subjectItem>();
            }
           
            sortFlyout.Hide();
            IsTimeSortAscending = !IsTimeSortAscending;
        }

        /// <summary>
        /// 按成绩排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gradeSort_Click(object sender, RoutedEventArgs e)
        {
            if (_grade == null)
            {
                return;
            }
            List<subjectItem> subjects = _grade.subject;
            if(!IsGradeSortAscending)
            {
                var query = from s in subjects orderby s.score ascending select s;
                gradeListView.ItemsSource = query.ToList<subjectItem>();
            }else
            {
                var query = from s in subjects orderby s.score descending select s;
                gradeListView.ItemsSource = query.ToList<subjectItem>();
            }
            sortFlyout.Hide();
            IsGradeSortAscending = !IsGradeSortAscending;
        }

        
    }
}
