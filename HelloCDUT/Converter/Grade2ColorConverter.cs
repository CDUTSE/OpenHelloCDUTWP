using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace 你好理工.Converter
{
    /// <summary>
    /// 成绩到颜色的转换器
    /// </summary>
    public class Grade2ColorConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string strGrade = System.Convert.ToString(value);
            if (strGrade != null)
            {
                switch (strGrade)
                {
                    case "优":
                        return Application.Current.Resources["A"] as SolidColorBrush;
                    
                    case "良":
                        return Application.Current.Resources["B"] as SolidColorBrush;
                        
                    case "中":
                        return Application.Current.Resources["C"] as SolidColorBrush;
                        
                    case "差":
                        return Application.Current.Resources["D"] as SolidColorBrush;
                         
                }
                int intGrade = int.Parse(strGrade);
                if (intGrade >= 90)
                {
                    return Application.Current.Resources["A"] as SolidColorBrush;
                }
                else if (intGrade >= 80)
                {
                    return Application.Current.Resources["B"] as SolidColorBrush;
                }
                else if (intGrade >= 70)
                {
                    return Application.Current.Resources["C"] as SolidColorBrush;
                }
                else if (intGrade >= 60)
                {
                    return Application.Current.Resources["D"] as SolidColorBrush;
                }
                else
                {
                    return Application.Current.Resources["E"] as SolidColorBrush;
                }
            }
            //int intGrade = -1;
            //bool isInt = int.TryParse((string)value,out intGrade);
            //if (value )
            //{
            
            //}
            //else
            //{
            //    //判断优良中差
            //    string strGrade = System.Convert.ToString(value);
                
            //}
            return Windows.UI.Colors.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
