using DataHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 你好理工.DataHelper.Model
{
    public class Pdf:ResultBase
    {
        public List<pdfItem> pdf_list { get; set; }
    }
    public class pdfItem
    {
        public string id { get; set; }
        public string name { get; set; }
        public string major { get; set; }
        public string make_date { get; set; }
        /// <summary>
        /// pdf文件路径
        /// </summary>
        public string url { get; set; }
    }
}
