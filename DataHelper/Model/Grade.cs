using DataHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 你好理工.DataHelper.Model
{
    /// <summary>
    /// 成绩
    /// </summary>
    public class Grade:ResultBase
    {
        public string cgpa { get; set; }
        public List<subjectItem> subject { get; set; }
        public string gpa { get; set; }
    }
    public struct subjectItem
    {
        /// <summary>
        /// 学期
        /// </summary>
        public string semester { get; set; }
        public string course_code { get; set; }
        public string course_name { get; set; }
        public string teacher { get; set; }
        /// <summary>
        /// 学分
        /// </summary>
        public string credits { get; set; }
        /// <summary>
        /// 成绩
        /// </summary>
        public string score { get; set; }
        public string type { get; set; }
        
        public string gpa { get; set; }
        public string admin { get; set; }
        /// <summary>
        /// 录入日期
        /// </summary>
        public string storage_time { get; set; }
    }
}
