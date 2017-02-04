using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHelper.Model
{
    public class SearchBook:ResultBase
    {
        public int total_page { get; set; }
        public int current_page { get; set; }
        public Book[] books { get; set; }
    }
    public class Book
    {
        /// <summary>
        /// 书名
        /// </summary>
        public string book_name{get;set;}
        /// <summary>
        /// 书籍序列号
        /// </summary>
        public string book_index { get; set; }
        /// <summary>
        /// 出版社
        /// </summary>
        public string book_press { get; set; }
        /// <summary>
        /// 出版时间
        /// </summary>
        public string book_publish_time { get; set; }
        /// <summary>
        /// 书籍介绍
        /// </summary>
        public string book_brief_introduction { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string book_writer { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string institution { get; set; }
        /// <summary>
        /// 馆藏和可外借
        /// </summary>
        public string book_count { get; set; }
        /// <summary>
        /// 所属校区
        /// </summary>
        public string campus { get; set; }
        /// <summary>
        /// 书目数据库
        /// </summary>
        public string book_database { get; set; }
        /// <summary>
        /// 用于查询详情页的索引
        /// </summary>
        public string href_index { get; set; }
        /// <summary>
        /// 封面图片链接
        /// </summary>
        public string pic_href { get; set; }
        /// <summary>
        /// 存放位置
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 在架和不在架
        /// </summary>
        public string available { get; set; }
    }
}
