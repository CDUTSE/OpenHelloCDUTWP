using DataHelper.Service;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using 你好理工.DataHelper.Helper;
using 你好理工.DataHelper.Model;
using 你好理工.View;
using 你好理工.View.Me;
using 你好理工.ViewModel;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介绍

namespace 你好理工
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
      
        public MainPage()
        {
            this.InitializeComponent();
          
            this.DataContext = new DutyViewModel();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
            AAONewsService aooService = new AAONewsService();
          
        }
       
        
        private async void dutyGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DutyModel dm = e.ClickedItem as DutyModel;
            #region 判断是否绑定
            if (dm.DutyName.Equals("日程"))
            {
                if((App.Current as App).user_aao_status.Equals("0"))
                {
                    if(await Functions.ShowMessageDialogWithChoose("教务系统未绑定，是否绑定？"))
                    {
                        this.Frame.Navigate(typeof(BindAAO));
                    }
                    else
                    {
                        
                    }
                    //popTitleTextBlock.Text = "教务系统未绑定，是否绑定？";
                    //popGrid.Width = this.ActualWidth;
                    //bindPopup.IsOpen = true;
                    //return;
                }
                else
                {
                    this.Frame.Navigate(dm.SourcePageType);
                }
            }
            else if(dm.DutyName.Equals("图书馆"))
            {
                if((App.Current as App).user_lib_status.Equals("0"))
                {
                    if (await Functions.ShowMessageDialogWithChoose("图书馆未绑定，是否绑定？"))
                    {
                        this.Frame.Navigate(typeof(BindLib));
                    }
                    else
                    {

                    }
     
                }
                else
                {
                    this.Frame.Navigate(dm.SourcePageType);
                }
            }
            else if(dm.DutyName.Equals("一卡通"))
            {
                if((App.Current as App).user_campus_status.Equals("0"))
                {
                    if (await Functions.ShowMessageDialogWithChoose("一卡通未绑定，是否绑定？"))
                    {
                        this.Frame.Navigate(typeof(BindCampus));
                    }
                    else
                    {

                    }
                    //popTitleTextBlock.Text = "一卡通未绑定，是否绑定？";
                    //popGrid.Width = this.ActualWidth;
                    //bindPopup.IsOpen = true;

                }
                else
                {
                    this.Frame.Navigate(dm.SourcePageType);
                }
            }
            #endregion
            else if (dm != null)
            {
                this.Frame.Navigate(dm.SourcePageType);
            }
        }

        
        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pivot.SelectedIndex == 1)
            {

            }
            if (pivot.SelectedIndex == 0)
            {
            }
            if (pivot.SelectedIndex == 2)
            {

                //pivotBackAnimation.To = avatarGrid.ActualHeight;
                //pivotBackStoryboard.Begin();
            }
           
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);
            
        }

        /// <summary>
        /// 消息点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void messageListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //this.Frame.Navigate(typeof(MessageRoom));
        }

       
        /// <summary>
        /// 我 PivotItem加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userInfoPivotItem_Loaded(object sender, RoutedEventArgs e)
        {
            nickNameTextBlock.Text = (Application.Current as App).user_nick_name;
            //accountTextBlock.Text = (Application.Current as App).user_name;
            if ((Application.Current as App).user_avatar_url != null)
            {
                avatarImgBrush.ImageSource = new BitmapImage(new Uri((Application.Current as App).user_avatar_url));
            }
            mottoTextBlock.Text = (Application.Current as App).user_motto;
        }

        
        /// <summary>
        /// 头像点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void avatarBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserInfoPage));
        }

       
    }
}
