using DataHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Web.Http;
using 你好理工.DataHelper.Helper;

namespace DataHelper.Service
{
    public class AAONewsService
    {
        private  const string NEWS_URL = "http://aao.cdut.edu.cn/aao/aao.php?sort=389&sorid=391&from=more";

        public ObservableCollection<News> newsCollection;

        public AAONewsService()
        {
            this.newsCollection = new ObservableCollection<News>();
            
        }

        /// <summary>
        /// 获取新闻
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetNews()
        {
            string content = string.Empty;
            try
            {
                content = await new HttpClient().GetStringAsync(new Uri(NEWS_URL));
            }
            catch(Exception e)
            {
                Functions.ShowMessage(e.Message);
            }
            HandleNewsContent(content);
            return true;
        }

        /// <summary>
        /// 处理返回的新闻Html
        /// </summary>
        /// <param name="content"></param>
        private void HandleNewsContent(string content)
        {
            string[] strs = content.Split(new char[] { '\r', '\n' });
            foreach (string item in strs)
            {
                if (item.Contains("images/1.gif"))
                {
                    News news = new News();
                    string regexUrl = "aao.+passg";
                    Regex regex = new Regex(regexUrl);
                    MatchCollection matchs = regex.Matches(item);
                    news.newsUrl = "http://www.aao.cdut.edu.cn/" + matchs[0];

                    string regexTitle = "k\">.+<span";
                    regex = new Regex(regexTitle);
                    matchs = regex.Matches(item);
                    if (matchs.Count > 0)
                    {
                        news.newsTitle = matchs[0].Value.Replace("k\">", "").Replace("<span", "");
                    }

                    string regexDate = "\\d{4}-\\d{2}-\\d{2}";
                    regex = new Regex(regexDate);
                    matchs = regex.Matches(item);
                    news.newsPostDate = matchs[0].Value;

                    newsCollection.Add(news);
                }
            }
        }

        /// <summary>
        /// 获取指定页的新闻
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<bool> GetNewsByPageNum(int page)
        {
            StringBuilder sb = new StringBuilder(NEWS_URL);
            sb.Append("&offset=").Append(page * 16);
            string content = string.Empty;
            try
            {
                content = await new HttpClient().GetStringAsync(new Uri(sb.ToString()));
            }
            catch (Exception e)
            {
                Functions.ShowMessage(e.Message);
                return false;
            }
            HandleNewsContent(content);
            return true;
        }
    }
}
