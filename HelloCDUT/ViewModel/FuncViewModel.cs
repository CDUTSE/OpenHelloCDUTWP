using DataHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.Web.Http;
using 你好理工.Common;
using 你好理工.DataHelper.Helper;

namespace 你好理工.ViewModel
{
    public class FuncViewModel
    {
         public FuncViewModel()
        {
            _RenewCommand = new RelayCommand(new Action<object>(Method));
        }

        private RelayCommand _RenewCommand;
        public RelayCommand RenewCommand
        {
            get
            {
                return _RenewCommand;
            }
            set
            {
                _RenewCommand = value;
            }
        }

        public async void Method(object obj)
        {
            BorrowedBook bBook =  obj as BorrowedBook;
            if(bBook==null)
            {
                return;
            }
            HttpResponseMessage response = await APIHelper.QueryLibInfo((App.Current as App).user_name, (App.Current as App).user_login_token,
                "4", "", bBook.bookRenewHref);
            if(response==null)
            {
                return;
            }
            RenewResult result = Functions.Deserlialize<RenewResult>(response.Content.ToString());
            if(result==null)
            {
                return;
            }
            Functions.ShowMessage("续借成功,归还时间为："+ result.back_time);
        }

        public async void NavigateToEmptyPointer()
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.emptypointer.com/about/"));
        }

        public async void ReviewInStore()
        {
            //"ad629406-8e84-4c86-bdf7-b68226a52bd7"
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + "ad629406-8e84-4c86-bdf7-b68226a52bd7"));
        }

        public async void CheckUpdate()
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:navigate?appid=" + "ad629406-8e84-4c86-bdf7-b68226a52bd7"));
        }
    }
}
