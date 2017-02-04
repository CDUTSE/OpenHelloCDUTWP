using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace 你好理工.DataHelper.Helper
{
    /// <summary>
    /// 承载所有的api
    /// </summary>
    public static class APIHelper
    {

        /// <summary>
        /// api地址
        /// </summary>
        private const string apiUri = "http://www.hellocdut.com/api/release/index.php";

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="dic">Post的内容组成的字典</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> SendHttpRequest(Dictionary<string, string> dic)
        {
            try
            {
                if (Functions.CheckNetWork())
                {
                    HttpClient client = new HttpClient();
                    HttpFormUrlEncodedContent content = new HttpFormUrlEncodedContent(dic);
                    HttpResponseMessage msg = await client.PostAsync(new Uri(apiUri), content);

                    return msg;
                }
                else
                {
                    Functions.ShowMessage("网络未连接");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("APIHelper.SendHttpRequest："+ex.Message);
                Functions.ShowMessage("网络出了点问题，请检查网络连接", "这真是令人尴尬");
                return null;
             
            }
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_password">用户密码</param>
        /// <param name="user_device_code">用户移动设备唯一识别码</param>
        public static async Task<HttpResponseMessage> RegisterUser(string user_name,string user_password,string user_device_code)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "registerUser");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_password", Functions.PublicEncrypt(user_password));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_password">用户密码</param>
        public static async Task<HttpResponseMessage> UserLogin(string user_name,string user_password)
        {
            Dictionary<string,string> dic = new Dictionary<string,string>();
            dic.Add("action","userLogin");
            dic.Add("user_name",Functions.PublicEncrypt(user_name));
            dic.Add("user_password",Functions.BCPublicEncrypt(user_password));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="key_words">查询用户名</param>
        /// <param name="query_type">查询类型</param>
        public static async void QueryUser(string user_name,string user_login_token,string key_words,string query_type)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "queryUser");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_password", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("key_words", key_words);
            dic.Add("query_type", query_type);
            HttpResponseMessage response = await SendHttpRequest(dic);
        }

        public enum ModifyCategory
        {
            user_nick_name,
            user_motto,
            user_love_status,
            user_sex_orientation
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="modify_category">修改类别</param>
        /// <param name="modify_value">修改后的值</param>
        /// <param name="modify_permission">修改后的权限</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> ModifyUserInfo(string user_name,string user_login_token,ModifyCategory modify_category,string modify_value,string modify_permission)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "modifyUserInfo");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("modify_category", modify_category.ToString());
            dic.Add("modify_value", modify_value);
            dic.Add("modify_permission", modify_permission);
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="user_new_password">新密码</param>
        public static async Task<HttpResponseMessage> ModifyUserPassword(string user_name,string user_login_token,string user_new_password)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "modifyUserPassword");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("user_new_password", Functions.PublicEncrypt(user_new_password));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 通过邮箱获取重置密码验证码
        /// </summary>
        /// <param name="user_email">用户邮箱账号 后台会判断是否绑定邮箱 提醒用户验证码30分钟内有效</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetResetUserPasswordTokenByEmail(string user_email)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "getResetUserPasswordTokenByEmail");
            dic.Add("user_email", Functions.PublicEncrypt(user_email));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 通过绑定邮箱找回密码
        /// </summary>
        /// <param name="user_email">用户邮箱账号</param>
        /// <param name="validate_code">邮箱重置密码验证码</param>
        /// <param name="new_password">新密码</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage>ResetUserPasswordByEmail(string user_email,
            string validate_code,string new_password)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "resetUserPasswordByEmail");
            dic.Add("user_email", Functions.PublicEncrypt(user_email));
            dic.Add("validate_code", Functions.PublicEncrypt(validate_code));
            dic.Add("new_password", Functions.PublicEncrypt(new_password));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 通过绑定的教务系统找回密码
        /// </summary>
        /// <param name="aao_account">教务处账号</param>
        /// <param name="aao_password">教务处密码</param>
        /// <param name="new_password">新密码</param>
        public static async Task<HttpResponseMessage> ResetUserPasswordByAAO(string aao_account,string aao_password,string new_password)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "resetUserPasswordByAAO");
            dic.Add("aoo_name", Functions.PublicEncrypt(aao_account));
            dic.Add("aoo_password", Functions.PublicEncrypt(aao_password));
            dic.Add("new_password", Functions.PublicEncrypt(new_password));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 创建自定义群
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="desc">群组描述 140字以内</param>
        /// <param name="isPublic">是否公开 true 或者 false</param>
        /// <param name="group_name">群组名称 48个UTF8字符以内</param>
        /// <param name="approval">加入群是否需要申请 不传则默认为false</param>
        /// <param name="secret_code">入群暗号</param>
        public static async void CreateGroups(string user_name, string user_login_token, string desc, string isPublic, string group_name, string secret_code, string approval = "true")
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "createGroups");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("desc", desc);
            dic.Add("public",isPublic);
            dic.Add("group_name", group_name);
            dic.Add("approval", approval);
            dic.Add("secret_code", Functions.PublicEncrypt(secret_code));
            HttpResponseMessage response = await SendHttpRequest(dic);
        }

        /// <summary>
        /// 用户添加群组
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">登录token</param>
        /// <param name="group_id">群组ID</param>
        /// <param name="secret_code">入群暗号 可选</param>
        public static async void UserAddGroups(string user_name,string user_login_token,
            string group_id,string secret_code="")
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "userAddGroups");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("group_id",group_id) ;
            dic.Add("secret_code", Functions.PublicEncrypt(secret_code));
            HttpResponseMessage response = await SendHttpRequest(dic);
        }

        /// <summary>
        /// 群组添加群成员
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="group_id">群ID</param>
        /// <param name="add_user_name">添加的用户名</param>
        public static async void GroupOwnerAddGroups(string user_name,string user_login_token,
            string group_id,string add_user_name)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "groupOwnerAddGroups");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("group_id", group_id);
            dic.Add("add_user_name", Functions.PublicEncrypt(add_user_name));
            HttpResponseMessage response = await SendHttpRequest(dic);
        }

        /// <summary>
        /// 删除群成员
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="group_id">群ID</param>
        public static async void DelGroupsUser(string user_name,string user_login_token,string group_id)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "delGroupsUser");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("group_id", group_id);
            HttpResponseMessage response = await SendHttpRequest(dic);
        }

        /// <summary>
        /// 获取群成员列表
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="group_id">群组ID</param>
        public static async void GetGroupUserList(string user_name,string user_login_token,string group_id)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "getGroupUserList");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("group_id", Functions.PublicEncrypt(group_id));
            HttpResponseMessage response = await SendHttpRequest(dic);
        }

        /// <summary>
        /// 用户反馈建议
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="advice">用户建议</param>
        /// <param name="version">客户端版本号</param>
        /// <param name="client">客户端(0->Android,1->iOS,2->WindowsPhone)</param>
        public static async Task<HttpResponseMessage> FeedbackAdvice(string user_name,string user_login_token,string advice,string version,string client)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "feedbackAdvice");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("advice", advice);
            dic.Add("version", version);
            dic.Add("client", client);
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 发送邮箱激活码邮件
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="user_email">用户邮箱</param>
        public static async Task<HttpResponseMessage> GetBindEmailToken(string user_name,string user_login_token,string user_email)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "getBindEmailToken");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("user_email", Functions.PublicEncrypt(user_email));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        /// <summary>
        /// 绑定教务系统
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="account">教务处账号</param>
        /// <param name="password">教务处密码</param>
        public static async Task<HttpResponseMessage> BindAAO(string user_name,string user_login_token,string account,string password)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "bindAAO");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("account", Functions.PublicEncrypt(account));
            dic.Add("password", Functions.PublicEncrypt(password));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 绑定一卡通
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="account">一卡通账号</param>
        /// <param name="password">一卡通密码</param>
        /// <param name="captcha">验证码</param>
        /// <param name="flag">标识位 第一次获取验证码不带上flag参数，获取验证码后带上flag=>true</param>
        public static async Task<HttpResponseMessage> BindCampus(string user_name,string user_login_token,string account,
            string password,string captcha,string flag)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "bindCampus");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("account", Functions.PublicEncrypt(account));
            dic.Add("password", Functions.PublicEncrypt(password));
            dic.Add("captcha", captcha);
            if (!string.IsNullOrEmpty(flag))
            {
                dic.Add("flag", flag);
            }
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
             
        /// <summary>
        /// 绑定邮箱
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">登录token</param>
        /// <param name="active_token">邮箱激活码</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage>BindEmail(string user_name,string user_login_token,string active_token)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "bindEmail");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("active_token", Functions.PublicEncrypt(active_token));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        /// <summary>
        /// 绑定图书馆
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="account">图书馆账号</param>
        /// <param name="password">图书馆密码</param>
        public static async Task<HttpResponseMessage> BindLib(string user_name,string user_login_token,string account,
            string password)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "bindLib");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("account", Functions.PublicEncrypt(account));
            dic.Add("password", Functions.PublicEncrypt(password));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 解除教务处绑定
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        public static async Task<HttpResponseMessage> UnbindAAO(string user_name,string user_login_token)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "unbindAAO");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 解除图书馆绑定
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        public static async Task<HttpResponseMessage> UnbindLib(string user_name,string user_login_token)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "unbindLib");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 解除邮箱绑定
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="user_email">用户email账号</param>
        public static async Task<HttpResponseMessage> UnbindEmail(string user_name,string user_login_token,string user_email)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "unbindEmail");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("user_email", Functions.PublicEncrypt(user_email));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 解除一卡通绑定
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        public static async Task<HttpResponseMessage> UnbindCampus(string user_name,string user_login_token)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "unbindCampus");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 查询图书馆信息
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="flag">查询标识  1获取读者个人信息,2获取当前借阅书籍,3获取借阅历史,4续借图书</param>
        /// <param name="jump_page">借阅历史页数 当flag设置为3的参数,第一次访问直接jump_page=>1</param>
        /// <param name="renew_href">续借超链接  当flag设为4时的参数</param>
        public static async Task<HttpResponseMessage> QueryLibInfo(string user_name,string user_login_token,
            string flag,string jump_page,string renew_href)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "queryLibInfo");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("flag", flag);
            if (!string.IsNullOrEmpty(jump_page))
            {
                dic.Add("jump_page", jump_page);
            }
            if (!string.IsNullOrEmpty(renew_href))
            {
                dic.Add("renew_href", renew_href);
            }
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 查询课表
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        public static async Task<HttpResponseMessage> QuerySchedule(string user_name,string user_login_token)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "querySchedule");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 查询成绩和绩点
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        public static async Task<HttpResponseMessage> QueryGrade(string user_name,string user_login_token)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "queryGrade");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        /// <summary>
        /// 查询教学计划
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">登录token</param>
        /// <param name="key_words">查询关键字</param>
        public static async Task<HttpResponseMessage> QueryTeachingPlan(string user_name,string user_login_token,string key_words)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "queryTeachingPlan");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("key_words",key_words);
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 查询空教室
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="building_num">查询的教学楼号 1,2,3,4,5,6a,6b,6c,7,8,9,art</param>
        /// <param name="query_data">查询时间 形如20xx-xx-xx</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> QueryEmptyRoom(string user_name,string user_login_token,
            string building_num,string query_data)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "queryEmptyRoom");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("building_num",building_num);
            dic.Add("query_date",query_data);
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        /// <summary>
        /// 查询馆藏图书
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="book_name">查询图书名</param>
        /// <param name="is_desc">是否按出版日期降序排列</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> SearchBook(string user_name,string user_login_token,
            string book_name,string is_desc)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "searchBook");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("book_name",book_name);
            dic.Add("is_desc",is_desc);
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        
        /// <summary>
        /// 查询馆藏图书翻页
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="jump_page">需要跳转的页数</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> BookJumpPage(string user_name,string user_login_token,
            string jump_page)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "bookJumpPage");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("jump_page",jump_page);
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        
        /// <summary>
        /// 查询馆藏图书详情
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="book_detail_index">需要查看详情的图书索引</param>
        /// <returns></returns>
         public static async Task<HttpResponseMessage> QueryBookDetail(string user_name,string user_login_token,
            string book_detail_index)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "queryBookDetail");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("book_detail_index",book_detail_index);
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        
        /// <summary>
        /// 获取一卡通登录验证码
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <returns></returns>
         public static async Task<HttpResponseMessage> GetAuthCode(string user_name,string user_login_token)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "getAuthCode");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        
        /// <summary>
        /// 获取一卡通用户信息
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token">用户登录token</param>
        /// <param name="captcha">一卡通登录验证码</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> CampusUserLogin(string user_name,string user_login_token,
            string captcha)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "campusUserLogin");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("captcha", captcha);
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        
        /// <summary>
        /// 查询一卡通存款信息
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <param name="user_login_token"></param>
        /// <param name="start_time">开始时间 如20XX-XX-XX,首次查询带上</param>
        /// <param name="end_time">结束时间 如20XX-XX-XX,首次查询带上</param>
        /// <param name="jump_page">需要跳转的页数 第一次不要带上，之后翻页带上</param>
        /// <returns></returns>
         public static async Task<HttpResponseMessage> QueryDepositInfo(string user_name,string user_login_token,
            string start_time,string end_time,string jump_page)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "queryDepositInfo");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("start_time", start_time);
            dic.Add("end_time",end_time);
            dic.Add("jump_page",jump_page);
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        
        /// <summary>
         /// 查询一卡通消费信息
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="user_login_token"></param>
         /// <param name="start_time">开始时间 如20XX-XX-XX,首次查询带上</param>
         /// <param name="end_time">结束时间 如20XX-XX-XX,首次查询带上</param>
         /// <param name="jump_page">需要跳转的页数 第一次不要带上，之后翻页带上</param>
         /// <returns></returns>
         public static async Task<HttpResponseMessage> QueryConsumeInfo(string user_name,string user_login_token,
            string start_time,string end_time,string jump_page)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "queryConsumeInfo");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("start_time", start_time);
            dic.Add("end_time",end_time);
            dic.Add("jump_page",jump_page);
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        
        /// <summary>
         /// 查询一卡通交易汇总信息
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="user_login_token"></param>
         /// <param name="start_time">开始时间 如20XX-XX-XX,首次查询带上</param>
         /// <param name="end_time">结束时间 如20XX-XX-XX,首次查询带上</param>
         /// <param name="jump_page"></param>
        /// <returns></returns>
         public static async Task<HttpResponseMessage> QueryCustStateInfo(string user_name,string user_login_token,
            string start_time,string end_time)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "queryCustStateInfo");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("start_time", start_time);
            dic.Add("end_time",end_time);
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        
        /// <summary>
         /// 查询更新
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="user_login_token"></param>
        /// <param name="client">客户端号 0是Android 1是IOS</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> QueryUpdate(string user_name,string user_login_token,
            string client)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "queryUpdate");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("client",client);
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
        
        /// <summary>
        /// 查询插件
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="user_login_token"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> QueryAddons(string user_name, string user_login_token,
            string client)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "queryAddons");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user_login_token"></param>
        /// <param name="user_name"></param>
        /// <param name="modify_category"></param>
        /// <param name="modify_value"></param>
        /// <param name="modify_permission"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> ModifyUserInfo(string user_login_token,string user_name,
            string modify_category,string modify_value,string modify_permission)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("action", "modifyUserInfo");
            dic.Add("user_name", Functions.PublicEncrypt(user_name));
            dic.Add("user_login_token", Functions.BCPublicEncrypt(Functions.BCPublicDecrypt(user_login_token)));
            dic.Add("modify_category", modify_category);
            HttpResponseMessage response = await SendHttpRequest(dic);
            return response;
        }
    }
}
