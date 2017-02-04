using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using 你好理工.Common;
using 你好理工.DataHelper.Helper;
using 你好理工.DataHelper.Model;


namespace 你好理工.DataTemplate.Data
{
   
    /// <summary>
    /// 课程数据提供者
    /// </summary>
    public sealed class ScheduleDataSource
    {

        private const string SCHEDULE_FILE_NAME = "Schedule.json"; //json文件的名字
      

        private static ScheduleDataSource _scheduleDataSource = new ScheduleDataSource();

        private ObservableDictionary _schedulesDictionary = new ObservableDictionary();
        /// <summary>
        /// 用于保存 Dictionary<周数，课程集合>
        /// </summary>
        public ObservableDictionary SchedulesDictionary
        {
            get 
            {
               
                return this._schedulesDictionary; 
            }
        }

        /// <summary>
        /// 从LocalFolder获取有课程数据的字典
        /// </summary>
        /// <returns></returns>
        public   static async  Task<ObservableDictionary> GetScheduleDictionary()
        {
            if(_scheduleDataSource._schedulesDictionary.Count==0)
            {
                await _scheduleDataSource.GetSchedulesAsyncFromLocalFolder();
    
            }
            
            return _scheduleDataSource._schedulesDictionary;
        }

        

        /// <summary>
        /// 将本地的课程数据排序后加入字典 
        /// 并设置好开学日期
        /// </summary>
        private async Task GetSchedulesAsyncFromLocalFolder()
        {
            StorageFile jsonFile = await ApplicationData.Current.LocalFolder.GetFileAsync(SCHEDULE_FILE_NAME);
            Schedule schedule = new Schedule();
            string jsonText = String.Empty;
            using (Stream stream = await jsonFile.OpenStreamForReadAsync())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    jsonText = reader.ReadToEnd();
                }
            }
            schedule = Functions.Deserlialize<Schedule>(jsonText);
            if (schedule != null)
            {
                ////设置开学日期
                //(Application.Current as App).term_beginMonth = schedule.term_begin[0];
                //(Application.Current as App).term_beginDay = schedule.term_begin[1];
                
                //获取最大周数
                int max = GetMaxWeekNum(schedule);
                //(Application.Current as App).max_weeknum = max;
                //按周获取课程并加入字典
                for (byte i = 1; i <= max; i++)
                {
                    List<classItem> scheduleCollection = schedule.@class.Where(x => x.week_num.Contains(i)).ToList<classItem>();
                    _scheduleDataSource._schedulesDictionary.Add(i.ToString(), scheduleCollection);
                }
                //将开学日期加入字典
                _schedulesDictionary.Add( (_schedulesDictionary.Keys.Count+1).ToString(), schedule.term_begin[0]);
                _schedulesDictionary.Add((_schedulesDictionary.Keys.Count+1).ToString(), schedule.term_begin[1]);
                //将最大周数加入字典
                _schedulesDictionary.Add((_schedulesDictionary.Keys.Count+1).ToString(), max);
            }
            
        }

        /// <summary>
        /// 获取最大周数
        /// </summary>
        /// <param name="schedule"></param>
        public static int GetMaxWeekNum(Schedule schedule)
        {
            int max = 0;
            foreach (classItem item in schedule.@class)
            {
                if (item.week_num[item.week_num.Count - 1] > max)
                {
                    max = item.week_num[item.week_num.Count - 1];
                }
            }
            return max;
        }

        /// <summary>
        /// 将json保存到LocalFolder
        /// </summary>
        /// <param name="jsonText">要保存的json</param>
        public static async Task SaveScheduleJsonToLocalFolder(string jsonText)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile jsonFile = await localFolder.CreateFileAsync(SCHEDULE_FILE_NAME, CreationCollisionOption.ReplaceExisting);

            using (Stream stream = await jsonFile.OpenStreamForWriteAsync())
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonText);
                await stream.WriteAsync(jsonBytes, 0, jsonBytes.Length);
                stream.Flush();
            }
        }

        /// <summary>
        /// 将Schedule类的对象转成字典
        /// </summary>
        /// <param name="schedule"></param>
        public static ObservableDictionary Schedule2Dictionary(Schedule schedule)
        {
            int max = GetMaxWeekNum(schedule);
            ObservableDictionary schedulesDictionary = new ObservableDictionary();
            List<classItem> scheduleCollection = null;
            //按周获取课程并加入字典
            for (byte i = 1; i <= max; i++)
            {
                scheduleCollection = schedule.@class.Where(x => x.week_num.Contains(i)).ToList<classItem>();
                schedulesDictionary.Add(i.ToString(), scheduleCollection);
            }
            //将开学日期加入字典
            schedulesDictionary.Add((schedulesDictionary.Keys.Count+1).ToString(), schedule.term_begin[0]);
            schedulesDictionary.Add(  (schedulesDictionary.Keys.Count+1).ToString(), schedule.term_begin[1]);
            //将最大周数加入字典
            schedulesDictionary.Add( (schedulesDictionary.Keys.Count+1).ToString() , max);
            return schedulesDictionary;
        }
    }
}
