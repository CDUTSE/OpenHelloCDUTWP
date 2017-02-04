using 你好理工.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using 你好理工.Model;
using Windows.Storage;
using Windows.Data.Json;
using System.Threading.Tasks;
using 你好理工.ViewModel;
using 你好理工.SampleData;
using System.Diagnostics;
using Windows.UI;
using System.Collections.ObjectModel;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工
{
    /// <summary>
    /// 可独立使用或用于导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DutyPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        
        public DutyPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            //GetSchedule(cmb.SelectedIndex+1);
            
           
        }

        private static IEnumerable<T> FindChildren<T>(DependencyObject parent) where T:class
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            if(count>0)
            {
                for(var i=0;i<count;i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    var t = child as T;
                    if(t!=null)
                    {
                        yield return t;
                    }
                    var children = FindChildren<T>(child);
                    foreach (var item in children)
                    {
                        yield return item;
                    }
                }
            }
        }

        //获取Template里的Grid
        private static Grid GetGrid(DependencyObject depObj)
        {
            if(depObj is Grid)
            {
                return depObj as Grid;
            }
            for(int i=0;i<VisualTreeHelper.GetChildrenCount(depObj);i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                var result = GetGrid(child);
                if(result !=null)
                {
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// 查看可视化树的结构
        /// </summary>
        /// <param name="parent">控件</param>
        /// <param name="indent">初始值设置为0</param>
        void DumpVisualTree(DependencyObject parent, int indent)
        {
            TextBlock txtblk = new TextBlock();
            // 模拟树层次结构显示结果.
            txtblk.Text = String.Format("{0}{1}", new string(' ', 4 * indent), parent.GetType().Name);

            Debug.WriteLine(txtblk);

            int numbChild = VisualTreeHelper.GetChildrenCount(parent);

            for (int childIndex = 0; childIndex < numbChild; childIndex++)
            {
                // 递归查找该控件的Child.
                DependencyObject child = VisualTreeHelper.GetChild(parent, childIndex);
                DumpVisualTree(child, indent + 1);
            }
        }
        string visualTreeStr;
        #region 遍历可视化树
        private void GetChildType(DependencyObject reference)
        {
            visualTreeStr = String.Empty;
            int count = VisualTreeHelper.GetChildrenCount(reference);
            if(count>0)
            {
                for(int i=0;i<=count-1;i++)
                {
                    var child = VisualTreeHelper.GetChild(reference, i);
                    visualTreeStr += child.GetType().ToString() + count + " ";
                    GetChildType(child);
                }
            }
        }
        #endregion

        /// <summary>
       /// 获取课程数据并加载到页面
       /// </summary>
        private async void GetSchedule(int weeknum)
        {
           if(ScheduleGrid !=null)
           {
               if(ScheduleGrid.Children !=null)
               {
                   //ScheduleGrid.Children.Clear();
               }
           }
            
            //获取课程数据
           IEnumerable<Schedule> x= await ScheduleDataSource.GetGroupsAsync();
            //ScheduleControl.ScheduleButton sb = null;
           
            Button sb;
            foreach (var item in x)
            {  
                foreach (var classItem in item.@class)
                {
                    if (classItem.week_num.Contains(weeknum.ToString()))
                    {
                        Debug.WriteLine(classItem.full_name + "\t" + classItem.begin + "\t" + classItem.section + "\t" + classItem.what_day);

                        sb = new Button();
                        sb.Margin = new Thickness(2);
                        sb.Padding = new Thickness(2);
                        sb.BorderThickness = new Thickness(0);
                        sb.Foreground = new SolidColorBrush(Colors.White);
                        sb.IsTextScaleFactorEnabled = true;

                        sb.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
                        sb.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                        //Grid.Row为课程开始的时间 begin为0是第一节课
                        Grid.SetRow(sb, Convert.ToInt32(classItem.begin));
                        Grid.SetRowSpan(sb, Convert.ToInt32(classItem.section));
                        Grid.SetColumn(sb, Convert.ToInt32(classItem.what_day));
                        sb.Background = new SolidColorBrush(Colors.Red);
                        sb.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                        sb.Content = new TextBlock()
                        {
                            Text = classItem.full_name + classItem.room,
                            FontSize = 12,
                            TextWrapping = TextWrapping.Wrap,
                            HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                            VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center,
                        };
                        ScheduleGrid.Children.Add(sb);
                        //GetChildType(this.schedulePivot);

                        //await  new Windows.UI.Popups.MessageDialog(visualTreeStr).ShowAsync();
                        //DumpVisualTree(this.LayoutRoot, 0);
                        //if(VisualTreeHelper.GetChildrenCount(this.schedulePivot)>0)
                        //{
                        //    IEnumerable<Grid> AllGrids = FindChildren<Grid>(this.schedulePivot);    //得到所有的GRID
                        //    foreach (var grid in AllGrids.ToList())
                        //    {

                        //    }
                        //}
                        //scheduleGrid.Children.Add(sb);
                    }
                }
            }
        }


        /// <summary>
        /// 根据周数取课程
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="week_num"></param>
        /// <returns></returns>
        private async void GetScheduleByWeeknum(string week_num)
        {
            IEnumerable<Schedule> scheduleCollection =  await ScheduleDataSource.GetGroupsAsync();

            ScheduleViewModel vm = new ScheduleViewModel();
            var ss = vm.ScheduleCollection;
            //按周取课程
            Button sb;
            //List<classItem> csList = collection.SelectMany(sd => sd.@class.Where(c => c.week_num.Contains(week_num))).ToList();

           
               //if(collection !=null)
               {
                   
                       
                   
                   //sb = new Button();
                   //sb.Margin = new Thickness(2);
                   //sb.Padding = new Thickness(2);
                   //sb.BorderThickness = new Thickness(0);
                   //sb.Foreground = new SolidColorBrush(Colors.White);
                   //sb.IsTextScaleFactorEnabled = true;
                   //sb.Width = 10;
                   //sb.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
                   //sb.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                   ////Grid.Row为课程开始的时间 begin为0是第一节课
                   //Grid.SetRow(sb, Convert.ToInt32(item.begin));
                   //Grid.SetRowSpan(sb, 2);
                   //Grid.SetColumn(sb, Convert.ToInt32(item.what_day));
                   //sb.Background = new SolidColorBrush(Colors.Red);

                   //sb.Content = new TextBlock() { Text = item.full_name + item.room, FontSize = 20, TextWrapping = TextWrapping.Wrap, HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch, VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch };

                   //ScheduleGrid.Children.Add(sb);
     
               }
                //Debug.WriteLine(item.full_name + "\t" + item.begin + "\t" + item.section + "\t" + item.what_day);

                
            //Button sb;
            //foreach (var item in csList)
            //{
            //    Debug.WriteLine(item.full_name + "\t" + item.begin + "\t" + item.section + "\t" + item.what_day);

            //    sb = new Button();
            //    sb.Margin = new Thickness(2);
            //    sb.Padding = new Thickness(2);
            //    sb.BorderThickness = new Thickness(0);
            //    sb.Foreground = new SolidColorBrush(Colors.White);
            //    sb.IsTextScaleFactorEnabled = true;
            //    sb.Width = 10;
            //    sb.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            //    sb.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            //    //Grid.Row为课程开始的时间 begin为0是第一节课
            //    Grid.SetRow(sb, Convert.ToInt32(item.begin));
            //    Grid.SetRowSpan(sb, 2);
            //    Grid.SetColumn(sb, Convert.ToInt32(item.what_day));
            //    sb.Background = new SolidColorBrush(Colors.Red);

            //    sb.Content = new TextBlock() { Text = item.full_name + item.room, FontSize = 20, TextWrapping = TextWrapping.Wrap, HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch, VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch };

            //    ScheduleGrid.Children.Add(sb);
            //}
        }

        /// <summary>
        /// 获取与此 <see cref="Page"/> 关联的 <see cref="NavigationHelper"/>。
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// 获取此 <see cref="Page"/> 的视图模型。
        /// 可将其更改为强类型视图模型。
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。  在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="sender">
        /// 事件的来源; 通常为 <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">事件数据，其中既提供在最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的导航参数，又提供
        /// 此页在以前会话期间保留的状态的
        /// 字典。 首次访问页面时，该状态将为 null。</param>
        private async  void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var scheduleDataGroups = await ScheduleDataSource.GetGroupsAsync();
            this.DefaultViewModel["Groups"] = scheduleDataGroups;
           
        }

        /// <summary>
        /// 保留与此页关联的状态，以防挂起应用程序或
        /// 从导航缓存中放弃此页。值必须符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化要求。
        /// </summary>
        /// <param name="sender">事件的来源；通常为 <see cref="NavigationHelper"/></param>
        ///<param name="e">提供要使用可序列化状态填充的空字典
        ///的事件数据。</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper 注册

        /// <summary>
        /// 此部分中提供的方法只是用于使
        /// NavigationHelper 可响应页面的导航方法。
        /// <para>
        /// 应将页面特有的逻辑放入用于
        /// <see cref="NavigationHelper.LoadState"/>
        /// 和 <see cref="NavigationHelper.SaveState"/> 的事件处理程序中。
        /// 除了在会话期间保留的页面状态之外
        /// LoadState 方法中还提供导航参数。
        /// </para>
        /// </summary>
        /// <param name="e">提供导航方法数据和
        /// 无法取消导航请求的事件处理程序。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter !=null)
            {
                DutyModel dm = e.Parameter as DutyModel;
                txtTitle.Text = dm.DutyName;
            }
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            classItem sd= e.ClickedItem as classItem;

            this.Frame.Navigate(typeof(ScheduleDetile), sd);
        }

        
        private void schedulePivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //周数改变事件
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if(cmb!=null)
            {
                ComboBoxItem cmbItem = cmb.SelectedItem as ComboBoxItem;
                Debug.WriteLine(cmbItem.Content);
                GetSchedule(cmb.SelectedIndex + 1);
                Debug.WriteLine(cmb.SelectedIndex);
            }
        }

      
    }
}
