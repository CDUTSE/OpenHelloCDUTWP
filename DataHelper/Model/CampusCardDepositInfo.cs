using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHelper.Model
{
    /// <summary>
    /// 一卡通存款信息
    /// </summary>
    public class CampusCardDepositInfo:ResultBase
    {
        public int total_page { get; set; }
        public int current_page { get; set; }
        public DepositInfo[] deposit_info { get; set; }
    }
    public struct DepositInfo
    {
        public string date{get;set;}
        public string time{get;set;}
        public string terminal{get;set;}
        public string money{get;set;}
        public string balance{get;set;}
        public string workstation{get;set;}
        public string terminal_name{get;set;}
    }
}
