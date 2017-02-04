using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 你好理工.DataHelper.Model;

namespace 你好理工.ViewModel
{
    public class SettingsViewModel:ViewModelBase
    {
        private Settings settings;
        public SettingsViewModel()
        {
            settings = Settings.Instance;
        }
        public bool IsNightModel
        {
            get
            {
                if(settings.NightMode.Equals("True"))   //是夜间模式
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if(value) //夜间
                {
                    settings.NightMode = "True";
                }
                else
                {
                    settings.NightMode = "False";
                }
                OnPropertyChanged("IsNightModel");
            }
        }

    }
}
