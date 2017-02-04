using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using 你好理工.View.Auth;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.View.School.Search
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ClassRoomSearch : Page
    {
        public ClassRoomSearch()
        {
            this.InitializeComponent();
          
        }
        //ListView的ScrollViewer
        private ScrollViewer scrollViewer;

        private Dictionary<string, string> _dic = GetDic();

        private static Dictionary<string, string> GetDic()
        {
            var dic = new Dictionary<string, string>();

            dic.Add("第一教学楼", "1");
            dic.Add("第二教学楼", "2");
            dic.Add("第三教学楼", "3");
            dic.Add("第四教学楼", "4");
            dic.Add("第五教学楼", "5");
            dic.Add("第六教学楼A区", "6a");
            dic.Add("第六教学楼B区", "6b");
            dic.Add("第六教学楼C区", "6c");
            dic.Add("第七教学楼", "7");
            dic.Add("第八教学楼", "8");
            dic.Add("第九教学楼", "9");
            dic.Add("东区第一教学楼", "东1");
            dic.Add("东区第二教学楼", "东2");
            dic.Add("艺术大楼", "art");
            return dic;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {
            //buildingCbBox.ItemsSource = _dic.Select(x => x.Key);
            Functions.ApplyDayModel(this);
            LoadRoomStatus();
           
        }

        double oldVerticalOffset = 0.0;
        //滚动面板的改变事件，当ListView滚动的时候会触发该事件
        void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (scrollViewer == null)
            {
                return;
            }
            else
            {
                double length = scrollViewer.VerticalOffset - oldVerticalOffset;  //滚动距离 +为向上滑动 
                oldVerticalOffset = scrollViewer.VerticalOffset;
                //判断当前滚动的高度是否大于或等于scrollViewer实际可滚动高度
                //大于或等于说明到底了
                if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight/2)  //滑动 1/2 隐藏搜索条件框
                {
                     
                    searchConditionGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else if(length<0)  //向下滑动时显示搜索条件框
                {
                    searchConditionGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
            }
            Debug.WriteLine(scrollViewer.VerticalOffset);

    
        }

        private async void LoadRoomStatus()
        {
            loadingGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (buildingCbBox.SelectedItem != null)
            {
                string buildNum = (buildingCbBox.SelectedItem as ComboBoxItem).Content.ToString();
                //string buildNum = buildingCbBox.SelectedItem as string;
                if (buildNum == null) return;
                string building_num = _dic[buildNum];
                //string building_num = (buildingCbBox.SelectedItem as ComboBoxItem).Tag.ToString();
                pageTitleTextBlock.Text = buildNum;

                string query_data = datePicker.Date.ToString("yyyy-MM-dd");
                HttpResponseMessage response = await APIHelper.QueryEmptyRoom((Application.Current as App).user_name, (Application.Current as App).user_login_token, building_num, query_data);
                if (response != null)
                {
                    RoomStatus roomStatus = Functions.Deserlialize<RoomStatus>(response.Content.ToString());
                    if (roomStatus.result.Equals("true"))
                    {
                        roomStatusListView.ItemsSource = roomStatus.rooms;
                        txtBlockLastUpdateTime.Text = DateTime.Now.ToLocalTime().ToString();
                    }
                    else 
                    {
                        //登录过期 返回登录界面
                        Functions.ShowMessage(roomStatus.message);
                        this.Frame.Navigate(typeof(Login));
                    }
                }
                loadingGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

 
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadRoomStatus();
        }

        private void roomStatusListView_Loaded(object sender, RoutedEventArgs e)
        {
            scrollViewer = Functions.FindChildOfType<ScrollViewer>(roomStatusListView);
            scrollViewer.ManipulationMode = ManipulationModes.System;
            scrollViewer.ViewChanged += scrollViewer_ViewChanged;
        }
    }
}
