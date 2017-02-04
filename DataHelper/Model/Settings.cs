using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace 你好理工.DataHelper.Model
{
    /// <summary>
    /// 设置
    /// </summary>
    public class Settings:ModelBase
    {
        /// <summary>
        /// 本地设置
        /// </summary>
        private ApplicationDataContainer settings = Windows.Storage.ApplicationData.Current.LocalSettings;
        /// <summary>
        /// 漫游设置
        /// </summary>
        private ApplicationDataContainer roaming = Windows.Storage.ApplicationData.Current.RoamingSettings;

        //夜间模式
        private const string SettingKey_NightModeTheme = "IsNightModel";
        //语言
        private const string SettingKey_UILanguage = "UILanguage";
        //主题色
        private const string SettingKey_ThemeColor = "ThemeColor";
        //用户账号
        private const string SettingKey_UserAccount = "UserAccount";
        //用户密码
        private const string SettingKey_UserPassword = "UserPassword";
        //应用是否第一次启动
        private const string SetingKey_IsFirstLaunch = "IsFirstLaunch";

        //volatile 不允许线程本地缓存 保持变量始终一致
        private static volatile Settings _Instance;
        private static object _locker = new object();

        private Settings() { }

        public static Settings Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_locker)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new Settings();
                        }
                    }
                }
                return _Instance;
            }
        }
        /// <summary>
        /// 夜间模式 True为夜间模式
        /// </summary>
        public string NightMode
        {
            get
            {
                var obj = this.settings.Values[SettingKey_NightModeTheme];
                return obj == null ? "False" : (string)obj;
            }
            set
            {
                this.settings.Values[SettingKey_NightModeTheme] = value;
                base.OnPropertyChanged("NightMode");
            }
        }

        /// <summary>
        /// 语言
        /// </summary>
        public string UILanguage
        {
            get
            {
                var value = settings.Values[SettingKey_UILanguage];
                return value == null ? "zh-CN" : value.ToString();
            }
            set
            {
                settings.Values[SettingKey_UILanguage] = value.ToString();
            }
        }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount
        {
            get
            {
                var value = settings.Values[SettingKey_UserAccount];
                return value==null?string.Empty:value.ToString();
            }
            set
            {
                settings.Values[SettingKey_UserAccount] = value.ToString();
            }
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string UserPassword
        {
            get
            {
                var value = settings.Values[SettingKey_UserPassword];
                return value == null ? string.Empty : value.ToString();
            }
            set
            {
                settings.Values[SettingKey_UserPassword] = value.ToString();
            }
        }

        /// <summary>
        /// 应用是否第一次启动 默认为是
        /// </summary>
        public bool IsFirstLaunch
        {
            get
            {
                var value = settings.Values[SetingKey_IsFirstLaunch];
                return value == null ? true : (bool)value;
            }
            set
            {
                settings.Values[SetingKey_IsFirstLaunch] = value;
            }
        }
    }
}
