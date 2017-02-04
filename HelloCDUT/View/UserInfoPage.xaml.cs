using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using 你好理工.DataHelper.Helper;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserInfoPage : Page
    {
        public UserInfoPage()
        {
            this.InitializeComponent();
            
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Functions.ApplyDayModel(this);

            var app = App.Current as App;
            if (app == null) return;
            if(app.user_avatar_url!=null)
            {
                avatarImgBrush.ImageSource = new BitmapImage(new Uri(app.user_avatar_url));
                //avatarBitmap.UriSource = new System.Uri((App.Current as App).user_avatar_url);                
            }
          
            accountTextBlock.Text = NullOrString(app.user_name);
            nickNameTextBlock.Text = NullOrString(app.user_nick_name);
            mottoTextBlock.Text = NullOrString(app.user_motto);
            
            switch(app.user_love_status)
            {
                case "0":
                    loveStatusTextBlock.Text = "保密";
                    break;
                case "1":
                    loveStatusTextBlock.Text = "求交往";
                    break;
                case "2":
                    loveStatusTextBlock.Text = "热恋中";
                    break;
            }
           

            switch(app.user_sex_orientation)
            {
                case "0":
                    sexOrientationTextBlock.Text = "异性";
                    break;
                case "1":
                    sexOrientationTextBlock.Text = "同性";
                    break;
                case "2":
                    sexOrientationTextBlock.Text = "孤独终生";
                    break;
                case "3":
                    sexOrientationTextBlock.Text = "双性";
                    break;
            }


            realNameTextBlock.Text = NullOrString(app.user_real_name);
            if(app.user_gender.Equals("0"))
            {
                genderTextBlock.Text = "保密";
            }else if(app.user_gender.Equals("1"))
            {
                genderTextBlock.Text = "男";
            }
            else
            {
                genderTextBlock.Text = "女";
            }
            birthTextBlock.Text = NullOrString(app.user_birthdate);

            stuIdTextBlock.Text = NullOrString(app.user_stu_id);
            instituteTextBlock.Text = NullOrString(app.user_institute);
            majorTextBlock.Text = NullOrString(app.user_major);
            classIdTextBlock.Text = NullOrString(app.user_class_id);
            entranceYearTextBlock.Text = NullOrString(app.user_entrance_year);

            //items.Add(new Item() { Title = "姓名", Desc = (App.Current as App).user_avatar_url });
            //items.Add(new Item() { Title = "账号", Desc = (App.Current as App).user_name });
            //items.Add(new Item() { Title = "昵称", Desc = (App.Current as App).user_nick_name });
            //items.Add(new Item() { Title = "座右铭", Desc = (App.Current as App).user_motto });
            //items.Add(new Item() { Title = "恋爱状态", Desc = (App.Current as App).user_love_status });
            //items.Add(new Item() { Title = "性取向", Desc = (App.Current as App).user_sex_orientation });
            //items.Add(new Item() { Title = "认证实名", Desc = (App.Current as App).user_real_name });
            //items.Add(new Item() { Title = "性别", Desc = (App.Current as App).user_gender });
            //items.Add(new Item() { Title = "生日", Desc = (App.Current as App).user_birthdate});
            //items.Add(new Item() { Title = "学院", Desc = (App.Current as App).user_institute });
            //items.Add(new Item() { Title = "专业", Desc = (App.Current as App).user_major });

            //userInfoListBox.ItemsSource = items;
        }

        private string NullOrString(string str)
        {
            return str == null ? string.Empty : str;
        }
        private ObservableCollection<Item> items = new ObservableCollection<Item>();
        /// <summary>
        /// 列表Model
        /// </summary>
        public class Item
        {
            public string Title { get; set; }
            public string Desc { get; set; }
        }
    }
}
