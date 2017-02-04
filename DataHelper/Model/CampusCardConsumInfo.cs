using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHelper.Model
{
    /// <summary>
    /// 一卡通消费信息
    /// </summary>
    public class CampusCardConsumInfo:ResultBase
    {
        public int current_page { get; set; }
        public int total_page { get; set; }

        public ConsumeInfo[] consume_info { get; set; }
    }

    public struct ConsumeInfo
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string date { get; set; }
        public string time { get; set; }
        /// <summary>
        /// 费用类型 餐费支出等
        /// </summary>
        public string item { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public string money { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public string balance { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string @operator { get; set; }
        /// <summary>
        /// 地点
        /// </summary>
        public string workstation { get; set; }
    }
}
