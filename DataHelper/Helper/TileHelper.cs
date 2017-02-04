using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using 你好理工.Common;
using 你好理工.DataHelper.Helper;
using 你好理工.DataHelper.Model;
using 你好理工.DataTemplate.Data;

namespace DataHelper.Helper
{
    /// <summary>
    /// 磁贴方法
    /// </summary>
    public static class TileHelper
    {
        private const string SCHEDULE_FILE_NAME = "Schedule.json"; //json文件的名字
        /// <summary>
        /// 更新课程磁贴
        /// </summary>
        public static async void UpdateScheduleTile()
        {
            Schedule schedule = new Schedule();
            try
            {
                StorageFile jsonFile = await ApplicationData.Current.LocalFolder.GetFileAsync(SCHEDULE_FILE_NAME);
                string jsonText = String.Empty;
                using (Stream stream = await jsonFile.OpenStreamForReadAsync())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        jsonText = reader.ReadToEnd();
                    }
                }
                schedule = Functions.Deserlialize<Schedule>(jsonText);
            }
            catch (Exception e)
            {
                Debug.WriteLine("TileBackground : " + e.Message);
            }


            if (schedule != null)
            {
                ObservableDictionary scheduleDictionary = ScheduleDataSource.Schedule2Dictionary(schedule);

                int month = (int)scheduleDictionary[(scheduleDictionary.Keys.Count - 2).ToString()];
                int day = (int)scheduleDictionary[(scheduleDictionary.Keys.Count - 1).ToString()];

                //获取当前周
                int currentWeekNum = (int)(new TimeSpan(DateTime.Now.Ticks).Subtract(new TimeSpan(new DateTime(2016, month, day).Ticks)).TotalDays) / 7 + 1;

                //获取当前周全部课程
                List<classItem> weekClass = scheduleDictionary[currentWeekNum.ToString()] as List<classItem>;
                //获取今天是星期几 1就是星期一
                int currentDay = (int)(DateTime.Now.DayOfWeek - 1 + 7) % 7 + 1;
                
                //获取当日课程
                List<classItem> ci = weekClass.Where(c => c.what_day.Equals(currentDay)).OrderBy(c => c.start_time).ToList<classItem>();

              
                //如果没课
                if (ci.Count == 0)
                {
                    ShowTileUltimate(null, 0);
                    return;
                }

                bool IsLastCourse = false;

                //今天的课上完了
                if (DateTime.Now > ci[ci.Count - 1].end_time)
                {
                    ShowTileUltimate(null, 0);
                    return;
                }
                //根据上课时间 获取下一节课
                for (int i = 0; i < ci.Count; i++)
                {
                    if (DateTime.Now < ci[i].start_time.AddMilliseconds(40))  //接下来有课
                    {
                        if (!IsLastCourse)
                        {
                            ShowTileUltimate(ci[i], ci.Count - i);
                            IsLastCourse = true;
                        }
                    }
                }
            }
        }

        //同时发送三种大小的磁贴
        private static void ShowTileUltimate(classItem csItem, int count)
        {
            XmlDocument xmlDocument = new XmlDocument();
            string xml = @"<tile>
                                       <visual version=""3"">
                                            <binding template=""TileSquare71x71IconWithBadge"">
                                                <image id=""1"" src=""{0}"" alt=""( ╯□╰ )""/>
                                            </binding>
                                            <binding template=""TileSquare150x150IconWithBadge"">
                                                <image id=""1"" src=""{1}"" alt=""( ╯□╰ )""/>
                                            </binding>
                                            <binding template=""TileWide310x150IconWithBadgeAndText"">
                                                <image id=""1"" src=""{2}""/>
                                                <text id=""1"">{3}</text>
                                                <text id=""2"">{4}</text>
                                                <text id=""3"">{5}</text>
                                            </binding>
                                        </visual>
                                    </tile>
                                    ";
            //
            string badgeXml = @"<badge version=""1"" value=""{0}""/>";

            if (csItem == null)
            {
                xml = string.Format(xml, "ms-appx:///Assets/Logo.scale_240_T.png", "ms-appx:///Assets/Logo.scale_240_T.png", "ms-appx:///Assets/Logo.scale_240_T.png",
                    "", "", "");
            }
            else
            {
                xml = string.Format(xml, "ms-appx:///Assets/Logo.scale_240_T.png", "ms-appx:///Assets/Logo.scale_240_T.png", "ms-appx:///Assets/Logo.scale_240_T.png",
                    csItem.full_name, "教室 : " + csItem.room, "时间 : " + csItem.start_time.ToString("H:mm") + "-" + csItem.end_time.ToString("H:mm"));
            }

            badgeXml = string.Format(badgeXml, count);
            xmlDocument.LoadXml(xml);
            TileNotification tileNotification = new TileNotification(xmlDocument);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            xmlDocument.LoadXml(badgeXml);
            BadgeNotification badgeNotification = new BadgeNotification(xmlDocument);
            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badgeNotification);
        }
    }
}
