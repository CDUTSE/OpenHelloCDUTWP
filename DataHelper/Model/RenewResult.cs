using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHelper.Model
{
    /// <summary>
    /// 续借
    /// </summary>
    public class RenewResult:ResultBase
    {
        /// <summary>
        /// 归还时间
        /// </summary>
        public string back_time { get; set; }
    }
}
