using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHelper.Model
{
    /// <summary>
    /// 返回Json的基本信息
    /// </summary>
    public class ResultBase
    {
        public string result { get; set; }
        public string message { get; set; }
    }
}
