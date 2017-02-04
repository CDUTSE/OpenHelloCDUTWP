using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace 你好理工.DataHelper.Model
{

    /// <summary>
    /// 功能类
    /// </summary>
    public class DutyModel:ModelBase
    {
        
        public DutyModel()
        {
            
        }
       
        //功能名
        private string _DutyName;
        public string DutyName
        {
            get
            {
                return _DutyName;
            }
            set
            {
                _DutyName = value;
                OnPropertyChanged("DutyName");
            }
        }
        //图标的路径
        private string _IconPath;
        public string IconPath
        {
            get
            { return _IconPath; }
            set
            {
                _IconPath = value;
                OnPropertyChanged("IconPath");
            }
        }
        //背景色
        private Color _BackgroundColor;
        public Color BackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                _BackgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }
        //页面导航
        private Type _SourcePageType;
        public Type SourcePageType
        {
            get
            {
                return _SourcePageType;
            }
            set
            {
                _SourcePageType = value;
                OnPropertyChanged("SourcePageType");
            }
        }
        
    }
}
