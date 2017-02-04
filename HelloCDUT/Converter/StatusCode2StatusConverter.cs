using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace 你好理工.Converter
{
    public class StatusCode2StatusConverter:IValueConverter
    {
        //教室状态 0空闲 1借用 2教学
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int status = (int)value;
            switch(status)
            {
                case 0:
                    return "空闲";
                  
                case 1:
                    return "教学";
                  
                case 2:
                    return "借用";
                  
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
