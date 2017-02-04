using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHelper.Model
{

    /// <summary>
    /// 教务新闻实体类
    /// </summary>
    public class News
    {
        public string newsTitle { get; set; }
        public string newsUrl { get; set; }
        public string newsPostDate { get; set; }
        public bool isReaded { get; set; }

        public News(String newsTittle, String newsUrl, String newsPostDate,
                       bool isReaded)
        {
            this.newsTitle = newsTitle;
            this.newsUrl = newsUrl;
            this.newsPostDate = newsPostDate;
            this.isReaded = isReaded;
        }

        public News()
        {

        }
    }
}
 