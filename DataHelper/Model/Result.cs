using DataHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 你好理工.DataHelper.Model
{
    /// <summary>
    /// 返回的最简单信息的model
    /// </summary>
    public class Result:ResultBase
    {
        //绑定教务处时返回的信息
        public string user_real_name { get; set; }
        /// <summary>
        /// 性别 1是男 0是女
        /// </summary>
        public string user_gender { get; set; }
        public string user_birthdate { get; set; }
        public string user_stu_id { get; set; }
        /// <summary>
        /// 学院
        /// </summary>
        public string user_institute { get; set; }
        public string user_major { get; set; }
        public string user_class_id { get; set; }
        /// <summary>
        /// 入学年份
        /// </summary>
        public string user_entrance_year { get; set; }

        /// <summary>
        /// 一卡通登录验证码
        /// </summary>
        public string captcha { get; set; }

        #region pdf

        public string id { get; set; }
        public string name { get; set; }
        public string major { get; set; }
        public string make_date { get; set; }
        /// <summary>
        /// pdf文件路径
        /// </summary>
        public string url { get; set; }

#endregion
    }
}
