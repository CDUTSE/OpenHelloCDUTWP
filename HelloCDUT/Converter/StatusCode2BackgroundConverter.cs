using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace 你好理工.Converter
{
    public class StatusCode2BackgroundConverter:IValueConverter
    {
        //教室状态 0空闲 1借用 2教学
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int status = (int)value;
            switch(status)
            {
                case 0:
                    return Application.Current.Resources["EmptyColorBrush"] as SolidColorBrush;
                    
                case 1:
                    return Application.Current.Resources["BorrowedColorBrush"] as SolidColorBrush;
                    
                case 2:
                    return Application.Current.Resources["ClassColorBrush"] as SolidColorBrush;
                   
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
