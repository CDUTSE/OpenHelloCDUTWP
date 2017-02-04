using DataHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 你好理工.DataHelper.Model
{
    /// <summary>
    /// 课程
    /// </summary>
    public class Schedule:ResultBase
    {
        //开学时间
        public List<int> term_begin { get; set; }
        //课程集合
        public ObservableCollection<classItem> @class { get; set; }  
    }
    
    public class classItem
    {
 
        /// <summary>
        /// 有课的周数（第一周为1)
        /// </summary>
        public List<byte> week_num { get; set; }
        /// <summary>
        /// 在每周的第几天（周一为1）
        /// </summary>
        public int what_day { get; set; }
        /// <summary>
        /// 从第几节课开始（起始点为0）
        /// </summary>
        public int begin { get; set; }
        /// <summary>
        /// 课的节数
        /// </summary>
        public int section { get; set; }
        /// <summary>
        /// 课程名
        /// </summary>
        public string full_name { get; set; }
        /// <summary>
        /// 文理类别
        /// </summary>
        public string cateory { get; set; }
        /// <summary>
        /// 是否重课
        /// </summary>
        public string is_OverLap_Class { get; set; }
        /// <summary>
        /// 学分
        /// </summary>
        public string credits { get; set; }
        /// <summary>
        /// 教师名
        /// </summary>
        public string teacher { get; set; }
        /// <summary>
        /// 学时
        /// </summary>
        public string period { get; set; }
        /// <summary>
        /// 教室
        /// </summary>
        public string room { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string note { get; set; }

        string[] startTimeofCourse = new string[]
                        {
                            "8:10:0", "9:0:0", "10:15:0", "11:05:0", "12:40:0", "14:30", "15:20:0", "16:25:0", "17:15:0",
                            "19:10:0", "20:0:0", "20:50:0"
                        };

        string[] endTimeofCourse = new[]
                        {
                            "8:55:0", "9:45:0", "11:0:0", "11:50:0", "14:0:0", "15:15:0", "16:05:0", "17:10:0", "18:00",
                            "19:55:0", "20:45:0", "21:35:0"
                        };
        

        /// <summary>
        /// 课程开始时间
        /// </summary>
        public DateTime start_time 
        {
            get
            {
                return DateTime.Parse(startTimeofCourse[begin]);
            }
        }
        /// <summary>
        /// 课程结束时间
        /// </summary>
        public DateTime end_time 
        {
            get
            {
                return DateTime.Parse(endTimeofCourse[begin + section-1]);
            }
        }

    }


}
