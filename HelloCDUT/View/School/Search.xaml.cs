using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using 你好理工.View.Me;
using 你好理工.View.Scholl.Search;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.School
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Search : Page
    {
        public Search()
        {
            this.InitializeComponent();
         
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);
        }

        
        private  void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListViewItem item =  sender as ListViewItem;
            ListViewItem item1 = e.ClickedItem as ListViewItem;
        }

        /// <summary>
        /// 查成绩ListViewItem Tapped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void gradeListViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if ((App.Current as App).user_aao_status.Equals("0"))
            {
                if (await Functions.ShowMessageDialogWithChoose("教务系统未绑定，是否绑定？"))
                {
                    this.Frame.Navigate(typeof(BindAAO));
                }
                else
                {

                }
            }
            else
            {
                this.Frame.Navigate(typeof(GradeSearch));
            }
        }
    }
}
