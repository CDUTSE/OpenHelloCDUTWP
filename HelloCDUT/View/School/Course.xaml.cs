using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Storage;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using 你好理工.Common;
using 你好理工.DataHelper.Helper;
using 你好理工.DataHelper.Model;
using 你好理工.DataTemplate.Data;
using DataHelper.Helper;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.View.Scholl
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Course : Page
    {
        ////学期开始的月份
        //private int term_beginMonth = 0;
        ////学期开始的日期
        //private int term_beginDay = 0;
        //json文件的名字
        private const string SCHEDULE_FILE_NAME = "Schedule.json";
        //装载课程数据的字典
        private ObservableDictionary scheduleDictionary;
        //当前周
        private int currentWeeknum { get; set; }
        /// <summary>
        /// 承载所有课程的Grid
        /// </summary>
        private Grid schedulesGrid;
        private SolidColorBrush[] scheduleColorBrush = new SolidColorBrush[20];
       
        public Course()
        {
            this.InitializeComponent();
            
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;

           
        }

        /// <summary>
        /// 初始化颜色
        /// </summary>
        private void InitializeColor()
        {
            scheduleColorBrush[0] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)38, (byte)61, (byte)150));
            scheduleColorBrush[1] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)255, (byte)102, (byte)0));
            scheduleColorBrush[2] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)236, (byte)0, (byte)140));
            scheduleColorBrush[3] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)18, (byte)171, (byte)74));
            scheduleColorBrush[4] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)163, (byte)44, (byte)155));
            scheduleColorBrush[5] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)182, (byte)114, (byte)47));
            scheduleColorBrush[6] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)102, (byte)207, (byte)230));
            scheduleColorBrush[7] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)182, (byte)211, (byte)0));
            scheduleColorBrush[8] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)241, (byte)171, (byte)0));
            scheduleColorBrush[9] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)0, (byte)71, (byte)182));
            scheduleColorBrush[10] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)131, (byte)110, (byte)255));
            scheduleColorBrush[11] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)108, (byte)27, (byte)114));
            scheduleColorBrush[12] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)202, (byte)173, (byte)0));
            scheduleColorBrush[13] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)126, (byte)43, (byte)66));
            scheduleColorBrush[14] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)235, (byte)173, (byte)205));
            scheduleColorBrush[15] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)4, (byte)145, (byte)209));
            scheduleColorBrush[16] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)106, (byte)220, (byte)162));
            scheduleColorBrush[17] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)0, (byte)98, (byte)110));
            scheduleColorBrush[18] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)148, (byte)161, (byte)226));
            scheduleColorBrush[19] = new SolidColorBrush(Color.FromArgb((byte)110, (byte)183, (byte)83, (byte)18));
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);
            InitialPage();
            if(this.RequestedTheme== ElementTheme.Light)
            {
                cmdBar.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                cmdBar.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        /// <summary>
        /// 初始化页面数据 选择数据获取方式
        /// </summary>
        private async void InitialPage()
        {
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                //本地不存在json  连网获取json并缓存到本地
                if ((!await Functions.IsFileExists(SCHEDULE_FILE_NAME)))
                {
                    try
                    {
                        await LoadScheduleFromNet();
                        TileHelper.UpdateScheduleTile();
                    }
                    catch(Exception e)
                    {
                        Functions.ShowMessage(e.Message);
                    }
                }
                else  //json存在 从本地读取json
                {
                    scheduleDictionary = await ScheduleDataSource.GetScheduleDictionary();

                    //当前周
                    currentWeeknum = GetCurrentWeekNum();
                    txtWeeknum.Text = currentWeeknum.ToString();
                    txtIsCurrentWeek.Opacity = 1;
                    searchNum = currentWeeknum;
                    pivot.SelectedIndex = 0;

                    //LoadScheduleByWeekNum(currentWeeknum, 0);
                    LoadSchedule2Grid(currentWeeknum, 0);

                    TileHelper.UpdateScheduleTile();
                    Debug.WriteLine("从本地读取json");
                }
            }
            loadingProgressRing.IsActive = false;
        }

        /// <summary>
        /// 连网获取课程数据
        /// </summary>
        /// <returns></returns>
        private async Task LoadScheduleFromNet()
        {
            HttpResponseMessage response = await APIHelper.QuerySchedule((Application.Current as App).user_name, (Application.Current as App).user_login_token);
            if (response != null)
            {
                Schedule schedule = Functions.Deserlialize<Schedule>(response.Content.ToString());
                if (schedule == null) return;
                if(schedule.result.Equals("false"))
                {
                    Functions.ShowMessage(schedule.message);
                    return;
                }
                if(schedule.@class.Count==0)
                {
                    Functions.ShowMessage("未获取到课表数据，请重试");
                    return;
                }
                if (schedule.result.Equals("true") && schedule.term_begin!=null ) //查询课表成功  保存json
                {
                     
                    scheduleDictionary = ScheduleDataSource.Schedule2Dictionary(schedule);

                    //当前周
                    currentWeeknum = GetCurrentWeekNum();
                    txtWeeknum.Text = currentWeeknum.ToString();
                    txtIsCurrentWeek.Opacity = 1;
                    searchNum = currentWeeknum;
                    //LoadScheduleByWeekNum(currentWeeknum, pivot.SelectedIndex); //将当前周课程数据加到当前的PivotItem中
                    LoadSchedule2Grid(currentWeeknum, pivot.SelectedIndex);
                    pivot.SelectedIndex = pivot.SelectedIndex;
                    Debug.WriteLine("连网获取json成功");
                    //缓存json到本地
                    await Task.Factory.StartNew(async () =>
                    {
                        await ScheduleDataSource.SaveScheduleJsonToLocalFolder(response.Content.ToString());
                    });
                }
                else   //查询课表失败
                {
                    Debug.WriteLine("连网获取json失败");
                    Functions.ShowMessage(schedule.message);

                }
            }
        }

        /// <summary>
        /// 获取当前周数
        /// </summary>
        private int GetCurrentWeekNum()
        {

            int month = (int)scheduleDictionary[(scheduleDictionary.Keys.Count - 2).ToString()];
            int day = (int)scheduleDictionary[(scheduleDictionary.Keys.Count -1).ToString()];
            //获取最大周数
            int max_weekNum = (int)scheduleDictionary[(scheduleDictionary.Keys.Count ).ToString()];
            (Application.Current as App).max_weeknum = max_weekNum;
            (Application.Current as App).term_beginMonth = month;
            (Application.Current as App).term_beginDay = day;

            
            int weekNum = (int) (new TimeSpan(DateTime.Now.Ticks).Subtract(new TimeSpan( new DateTime(2016,month,day).Ticks )).TotalDays )/7;

            return weekNum + 1;
        }

        /// <summary>
        /// 按周将课程数据加载到对应的PivotItem中
        /// </summary>
        /// <param name="weeknum">周数</param>
        /// <param name="pivotIndex">要加载到的PivotItem的index 0，1，2</param>
        //private  void LoadScheduleByWeekNum(int weeknum,int pivotIndex)
        //{
        //    switch (pivotIndex)
        //    {
        //        case 0:
        //            GetScheduleNum(grid0);
        //            break;
        //        case 1:
        //            GetScheduleNum(grid1);
        //            break;
        //        case 2:
        //            GetScheduleNum(grid2);
        //            break;
        //    }

        //    //从ScheduleDictionary中获取对应周的课程数据
        //    List<classItem> schedules = scheduleDictionary[weeknum.ToString()] as List<classItem>;

        //    foreach (classItem csItem in schedules)
        //    {
        //        //Debug.WriteLine("课程名：" + csItem.full_name + "\n" + "教师：" + csItem.teacher+csItem.start_time+"\n"+csItem.end_time);
        //        Button btn = new Button()
        //        {
                   
        //            Content = csItem.full_name + csItem.room,
        //            DataContext = csItem,
        //            FontSize = 13,
        //            MinHeight = 50,
        //            MinWidth = 40,
        //            Margin = new Thickness(1),
        //            Padding = new Thickness(2),
        //            BorderThickness = new Thickness(0),
        //            HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch,
                    
        //            VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch,
        //            Style = this.Resources["ScheduleButtonStyle"] as Style,
        //            HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
        //        };
        //        btn.Click+=btn_Click;
        //        Grid.SetRow(btn, csItem.begin);
        //        Grid.SetRowSpan(btn, csItem.section);
        //        Grid.SetColumn(btn, csItem.what_day);
                
        //        //添加到PivotItem中
        //        switch(pivotIndex)
        //        {
        //            case 0:     
        //                grid0.Children.Add(btn);
        //                break;
        //            case 1:
        //                grid1.Children.Add(btn);
        //                break;
        //            case 2:
        //                grid2.Children.Add(btn);
        //                break;
        //        }
        //    }
        //}

        private int magic = 0;
        /// <summary>
        /// 将课程数据填充到Grid中
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private Grid LoadSchedule2Grid(int weeknum,int pivotIndex)
        {
            schedulesGrid = new Grid();
            InitializeColor();
            //行
            for (int row = 0; row < 12; row++)//加一是为了在底部留出空隙
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.MinHeight = 55;
                schedulesGrid.RowDefinitions.Add(rowDefinition);
            }

            //课程编号列
            //ColumnDefinition columnDefinition1 = new ColumnDefinition();//左侧写课程数
            //GridLength width1 = new GridLength(50);
            //columnDefinition1.MinHeight = 55;
            //scheduleGrid.ColumnDefinitions.Add(columnDefinition1);

            //课程列
            for (int row = 0; row < 8; row++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                schedulesGrid.ColumnDefinitions.Add(columnDefinition);
            }

            LoadScheduleNum(schedulesGrid);
            LoadScheduleDate();
            //加课程编号
            //int i = 0;
            //for (; i < 12; i++)
            //{
            //    TextBlock textBlock = new TextBlock();
            //    textBlock.Text = (i + 1).ToString();
            //    textBlock.TextAlignment = TextAlignment.Center;
            //    textBlock.VerticalAlignment = VerticalAlignment.Center;
            //    SolidColorBrush brush = new SolidColorBrush(Color.FromArgb((byte)200, (byte)255, (byte)255, (byte)255));
            //    textBlock.Foreground = brush;
            //    FontSize = 20;

            //    Grid.SetRow(textBlock, i);
            //    Grid.SetColumn(textBlock, 0);
            //    schedulesGrid.Children.Add(textBlock);
            //}

         
            //加载当周课程
            List<classItem> schedules = scheduleDictionary[weeknum.ToString()] as List<classItem>;

            //课程加入Grid
            foreach (classItem csItem in schedules)
            {
                if (++magic > 19)
                {
                    magic = 0;
                }
                //Debug.WriteLine("课程名：" + csItem.full_name + "\n" + "教师：" + csItem.teacher + csItem.start_time + "\n" + csItem.end_time);
                Button btn = new Button()
                {
                    Content = csItem.full_name + csItem.room,
                    DataContext = csItem,
                    FontSize = 13,
                    MinHeight = 50,
                    MinWidth = 40,
                    Margin = new Thickness(1),
                    Padding = new Thickness(2),
                    BorderThickness = new Thickness(0),
                    HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch,
                    VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch,
                    Style = this.Resources["ScheduleButtonStyle"] as Style,
                    HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                    Background = scheduleColorBrush[magic],
                };
                btn.Click += btn_Click;
                Grid.SetRow(btn, csItem.begin);
                Grid.SetRowSpan(btn, csItem.section);
                Grid.SetColumn(btn, csItem.what_day);
                schedulesGrid.Children.Add(btn);
          
            }
      
 
            switch(pivotIndex)
            {
                case 0:
                    scrollViewer0.Content = schedulesGrid;
                    break;
                case 1:
                    scrollViewer1.Content = schedulesGrid;
                    break;
                case 2:
                    scrollViewer2.Content = schedulesGrid;
                    break;
            }
           
            return scheduleGrid;
        }

        //用于查询的周数 从1开始
        private int searchNum = 1;
        //记录前一个index
        private int preIndex = 0;
        /// <summary>
        /// Pivot左右滑动事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (scheduleDictionary != null)
            {
                
                //当前选中PivotItem的index   为 0，1，2，……
                int index = pivot.SelectedIndex;
                //最大周数
                int max_weeknum = (Application.Current as App).max_weeknum;
               
                //switch(index)
                //{
                //    case 0:
                //        grid1.Children.Clear();
                //        grid2.Children.Clear();
                //        break;
                //    case 1:
                //        grid0.Children.Clear();
                //        grid2.Children.Clear();
                //        break;
                //    case 2:
                //        grid0.Children.Clear();
                //        grid1.Children.Clear();
                //        break;
                //}
               
                //判断左右滑
                //左滑 searchNum+1
                if ((preIndex == 0 && index == 1) || (preIndex == 1 && index == 2) || (preIndex == 2 && index == 0))
                {
                    searchNum = (searchNum % max_weeknum)+1;
                    if(searchNum==0)
                    {
                        searchNum = max_weeknum;
                    }
      
                    Debug.WriteLine("左滑");
                    preIndex = index;
                }
                else if ((preIndex == 0 && index == 2) || (preIndex == 2 && index == 1) || (preIndex == 1 && index == 0))
                {
                    //右滑 searchNum-1
                    if (searchNum == 1)
                    {
                        searchNum = (Application.Current as App).max_weeknum;

                    }
                    else
                    {
                        searchNum = Math.Abs((searchNum - 1) % (Application.Current as App).max_weeknum);
                    }
                        preIndex = index;
                }
                //当前周显示本周
                if (currentWeeknum.Equals(searchNum))
                {
                    txtIsCurrentWeek.Opacity = 1;
                }
                else
                {
                    txtIsCurrentWeek.Opacity = 0;
                }
                txtWeeknum.Text = (searchNum ).ToString();
                //LoadScheduleByWeekNum(searchNum , index);
                LoadSchedule2Grid(searchNum, index);
            }
        }
         
        /// <summary>
        /// 点击课程弹出课程详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                classItem csItem = btn.DataContext as classItem;
                if (csItem != null)
                {
                    runCourseName.Text = csItem.full_name;
                    runRoom.Text = csItem.room;
                    runTeacher.Text = csItem.teacher;
                    runCredit.Text = csItem.credits;
                    runNote.Text = (string.IsNullOrEmpty(csItem.note))?string.Empty: csItem.note.Replace("备注:", "").Replace("\r\n", "");
                    runPeriod.Text = csItem.period;

                    popBorder.Width = this.ActualWidth;
                    popBorder.Height = this.ActualHeight + 30;
  
                    schedulePopup.IsOpen = true;
                }
            }
        }
         
        /// <summary>
        /// 加载顶栏日期
        /// </summary>
        /// <param name="grid"></param>
        private void LoadScheduleDate()
        {
            DateTime time = (new DateTime(2015, (Application.Current as App).term_beginMonth, (Application.Current as App).term_beginDay))
                .AddDays((searchNum-1) * 7);

            txtMonth.Text = time.Month.ToString();

            txtDay1.Text = time.Day.ToString();
            if(time.Date==DateTime.Now.Date)
            {
                txtDay1.Opacity = 1;
            }
            else
            {
                txtDay1.Opacity = 0.5;
            }
            time = time.AddDays(1);

            txtDay2.Text = time.Day.ToString();
            if (time.Date == DateTime.Now.Date)
            {
                txtDay2.Opacity = 1;
            }
            else
            {
                txtDay2.Opacity = 0.5;
            }
            time = time.AddDays(1);

            txtDay3.Text = time.Day.ToString();
            if (time.Date == DateTime.Now.Date)
            {
                txtDay3.Opacity = 1;
            }
            else
            {
                txtDay3.Opacity = 0.5;
            }
            time = time.AddDays(1);

            txtDay4.Text = time.Day.ToString();
            if (time.Date == DateTime.Now.Date)
            {
                txtDay4.Opacity = 1;
            }
            else
            {
                txtDay4.Opacity = 0.5;
            }
            time = time.AddDays(1);

            txtDay5.Text = time.Day.ToString();
            if (time.Date == DateTime.Now.Date)
            {
                txtDay5.Opacity = 1;
            }
            else
            {
                txtDay5.Opacity = 0.5;
            }
            time = time.AddDays(1);

            txtDay6.Text = time.Day.ToString();
            if (time.Date == DateTime.Now.Date)
            {
                txtDay6.Opacity = 1;
            }
            else
            {
                txtDay6.Opacity = 0.5;
            }
            time = time.AddDays(1);

            txtDay7.Text = time.Day.ToString();
            if (time.Date == DateTime.Now.Date)
            {
                txtDay7.Opacity = 1;
            }
            else
            {
                txtDay7.Opacity = 0.5;
            }
            time = time.AddDays(1);
            
        }

        /// <summary>
        /// 加载课程编号
        /// </summary>
        private void LoadScheduleNum(Grid grid)
        {
            TextBlock textBlock;
            for (int i = 0; i < 12; i++)
            {
                textBlock = new TextBlock() { Text = i.ToString() };
                textBlock.Style = this.Resources["ScheduleNumber"] as Style;
                if (i == 4)
                {
                    textBlock.Text = "午";
                }
                else if (i < 4)
                {
                    textBlock.Text = (i + 1).ToString();
                }
                else
                {
                    textBlock.Text = i.ToString();
                }

                Grid.SetRow(textBlock, i);
                Grid.SetColumn(textBlock, 0);
                if (grid != null)
                {
                    grid.Children.Add(textBlock);
                }
            }
        }
        
      
        /// <summary>
        /// 重新导入课表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void reLoadScheduleAppbar_Click(object sender, RoutedEventArgs e)
        {
            loadingProgressRing.IsActive = true;
            //grid0.Children.Clear();
            //grid1.Children.Clear();
            //grid2.Children.Clear();
            scheduleGrid.Children.Clear();
            await LoadScheduleFromNet();
            loadingProgressRing.IsActive = false;
        }
         
        private void cmdBar_Closed(object sender, object e)
        {
            cmdBar.Background = new SolidColorBrush(Colors.Transparent);
            if (this.RequestedTheme == ElementTheme.Light)
            {
                cmdBar.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                cmdBar.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void cmdBar_Opened(object sender, object e)
        {
            cmdBar.Background = new SolidColorBrush(Colors.Black);
            cmdBar.Foreground = new SolidColorBrush(Colors.White);
            cmdBar.Opacity = 0.5;
        }

        private void popBorder_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            schedulePopup.IsOpen = false;
        }
         
    }
}
