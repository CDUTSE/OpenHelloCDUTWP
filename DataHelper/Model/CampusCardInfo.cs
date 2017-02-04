using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHelper.Model
{
    /// <summary>
    /// 一卡通信息
    /// </summary>
    public class CampusCardInfo:ResultBase
    {
        public string user_stu_num { get; set; }
        /// <summary>
        /// 卡状态
        /// </summary>
        public string user_card_status { get; set; }
        public string user_real_name { get; set; }
        /// <summary>
        /// 主钱包余额
        /// </summary>
        public string user_card_balance { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string user_gender { get; set; }
        /// <summary>
        /// 补助余额
        /// </summary>
        public string user_subsidy_balance { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string user_degree { get; set; }
        public string user_institute { get; set; }
        public string user_major { get; set; }
    }
}
