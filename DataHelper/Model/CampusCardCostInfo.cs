using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHelper.Model
{
    /// <summary>
    /// 交易汇总
    /// </summary>
    public class CampusCardCostInfo:ResultBase
    {
        /// <summary>
        /// 餐费支出
        /// </summary>
        public double wallet_deals_amount { get; set; }
        /// <summary>
        /// 沐浴支出
        /// </summary>
        public double shower_amount { get; set; }
        /// <summary>
        /// 购物支出
        /// </summary>
        public double shopping_amount { get; set; }
        /// <summary>
        /// 公交支出
        /// </summary>
        public double bus_amount { get; set; }

        public double total_amount
        {
            get
            {
                return wallet_deals_amount + shower_amount + shopping_amount;
            }
        }
    }
}
