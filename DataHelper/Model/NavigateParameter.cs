using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHelper.Model
{
    public class NavigateParameter
    {
        
        public string start_time { get; set; }
        public string end_time { get; set; }
       
        /// <summary>
        /// 0代表存款 1代表消费 2交易汇总
        /// </summary>
        public string search_type { get; set; }

    }
}
