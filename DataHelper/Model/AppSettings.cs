using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using 你好理工.DataHelper.Model;

namespace DataHelper.Model
{
    public class AppSettings:ModelBase
    {
         //volatile 不允许线程本地缓存 保证变量始终一致
        private static volatile AppSettings _Instance;
        private static object _locker = new object();

        private AppSettings() { }

        public static AppSettings Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_locker)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new AppSettings();
                        }
                    }
                }
                return _Instance;
            }
        }

        private ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;

        const string SettingKey_NightMode = "night_mode";
        /// <summary>
        /// 夜间模式 true为夜间 false为日间
        /// </summary>
        public bool NightMode
        {
            get
            {
                var obj = this.settings.Values[SettingKey_NightMode];
                return obj == null ? false : (bool)obj;
            }
            set
            {
                this.settings.Values[SettingKey_NightMode] = value;
                OnPropertyChanged("NightMode");
            }
        }
    }
}
