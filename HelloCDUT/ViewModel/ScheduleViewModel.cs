using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.Web.Http;
using 你好理工.DataHelper.Helper;
using 你好理工.DataHelper.Model;

namespace 你好理工.ViewModel
{
    public class ScheduleViewModel:ViewModelBase
    {
        
        public ObservableCollection<Schedule> ScheduleCollection { get; set; }


        public ScheduleViewModel()
        {
            ScheduleCollection = new ObservableCollection<Schedule>();
            //LoadSchedule();
        }
        //从json取数据
        private async void LoadSchedule()
        {
            Uri dataUri = new Uri("ms-appx:///SampleData/Schedule.json");

            HttpResponseMessage response =await APIHelper.QuerySchedule((Application.Current as App).user_name, (Application.Current as App).user_login_token);

            if (response != null && response.Content != null)
            {
                string jsonText = response.Content.ToString();

                Schedule schedule = Functions.Deserlialize<Schedule>(jsonText);


                ScheduleCollection.Add(schedule);

                ObservableCollection<Schedule> pivotScheduleCollection = new ObservableCollection<Schedule>();
                for (byte weeknum = 1; weeknum <= 18; weeknum++)
                {
                    List<classItem> csList = ScheduleCollection.SelectMany(sd => sd.@class.Where(c => c.week_num.Contains(weeknum))).ToList();
                    pivotScheduleCollection.Add(new Schedule() { @class = new ObservableCollection<classItem>(csList) });

                }
                ScheduleCollection = pivotScheduleCollection;
            }
        }

        /// <summary>
        /// 按周重新排列ScheduleCollection
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private ObservableCollection<Schedule> PivotScheduleCollection(ObservableCollection<Schedule> collection)       
        {
            ObservableCollection<Schedule> pivotScheduleCollection = new ObservableCollection<Schedule>();
            for (byte weeknum = 1; weeknum <= 18; weeknum++)
            {
                List<classItem> csList = collection.SelectMany(sd => sd.@class.Where(c => c.week_num.Contains(weeknum))).ToList();
                pivotScheduleCollection.Add(new Schedule() { @class = new ObservableCollection<classItem>(csList) });
                //collection.Add(new ScheduleDataGroup() { @class = new ObservableCollection<classItem>(csList) });
            }
            collection = pivotScheduleCollection;   
            return collection;
        }
       
    }
}
