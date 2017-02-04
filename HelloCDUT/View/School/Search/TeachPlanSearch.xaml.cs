using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using 你好理工.DataHelper.Helper;
using 你好理工.DataHelper.Model;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace 你好理工.View.Scholl.Search
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TeachPlanSearch : Page
    {
        public TeachPlanSearch()
        {
            this.InitializeComponent();
           
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Functions.ApplyDayModel(this);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            progress0.IsActive = true;
            string user_name = (Application.Current as App).user_name;
            string user_login_token = (Application.Current as App).user_login_token;
            string key_word = keyWordTextBox.Text.Trim();
            HttpResponseMessage response = await APIHelper.QueryTeachingPlan(user_name, user_login_token, key_word);
            if (response != null)
            {
                Pdf result = Functions.Deserlialize<Pdf>(response.Content.ToString());
                if (result != null)
                {
                    if(result.result.Equals("false"))
                    {
                        Functions.ShowMessage(result.message);
                        progress0.IsActive = false;
                        return;
                    }
                    flipView.ItemsSource = result.pdf_list;
                    pdfListView.ItemsSource = result.pdf_list;
                }
            }
            progress0.IsActive = false;
        }

        private void Pdf2Image(string pdfUrl)
        {
        }

        private async void pdfListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            pdfItem pdf = e.ClickedItem as pdfItem;
            if(pdf!=null)
            {
                await Launcher.LaunchUriAsync(new Uri(pdf.url));
            }
        }

       
    }
}
