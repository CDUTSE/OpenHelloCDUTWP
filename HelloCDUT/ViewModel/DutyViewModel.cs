using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using 你好理工.DataHelper.Model;
using 你好理工.School;
using 你好理工.School.Librarys;
using 你好理工.View.Scholl;


namespace 你好理工.ViewModel
{
    class DutyViewModel:ViewModelBase
    {
        public ObservableCollection<DutyModel> dutyCollection { get; set; }

        

        public DutyViewModel()
        {
            dutyCollection = new ObservableCollection<DutyModel>();
            LoadDuty();
        }
        
        private void LoadDuty()
        {
            dutyCollection.Add(new DutyModel { DutyName = "日程", IconPath = "ms-appx:///Assets/class.png",BackgroundColor = Color.FromArgb(255,255,106,106),SourcePageType=typeof(Course) });
            dutyCollection.Add(new DutyModel { DutyName = "查询", IconPath = "ms-appx:///Assets/search.png",BackgroundColor = Color.FromArgb(255,219,84,241),SourcePageType=typeof(Search) });
            //dutyCollection.Add(new DutyModel { DutyName = "选课", IconPath = "ms-appx:///Assets/search.png", BackgroundColor = Color.FromArgb(255,68,157,255),SourcePageType=typeof(SelectCourse) });
            //dutyCollection.Add(new DutyModel { DutyName = "跳蚤市场", IconPath = "ms-appx:///Assets/Windows.png",BackgroundColor = Color.FromArgb(255,255,151,0),SourcePageType=typeof(Market) });
            //dutyCollection.Add(new DutyModel { DutyName = "社区", IconPath = "ms-appx:///Assets/Windows.png",BackgroundColor = Color.FromArgb(255,80,213,118),SourcePageType=typeof(Community) });
            dutyCollection.Add(new DutyModel { DutyName = "图书馆", IconPath = "ms-appx:///Assets/library.png",BackgroundColor = Color.FromArgb(255, 0,207,197),SourcePageType=typeof(Library) });
            dutyCollection.Add(new DutyModel { DutyName = "一卡通", IconPath = "ms-appx:///Assets/card.png", BackgroundColor = Color.FromArgb(255, 255, 205, 0),SourcePageType=typeof(CampusCard) });
            //dutyCollection.Add(new DutyModel { DutyName = "更多", IconPath = "ms-appx:///Assets/more.png",BackgroundColor = Color.FromArgb(250,249,151,237),SourcePageType=typeof(More) });
           
        }

    
    }
}
