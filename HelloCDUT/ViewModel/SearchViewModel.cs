using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using 你好理工.Common;
using 你好理工.DataHelper.Helper;

namespace 你好理工.ViewModel
{
    public class SearchViewModel
    {
        public SearchViewModel()
        {
            _NavigateCommand = new RelayCommand(new Action<object>(method));
        }
     
       
      

        private RelayCommand _NavigateCommand;
        public RelayCommand NavigateCommand
        {
            get
            {
                return _NavigateCommand;
            }
            set
            {
                _NavigateCommand = value;
            }
        }

        private void method(object obj)
        {
            ListViewItem btn = obj as ListViewItem;
            if (btn != null)
            {
                Functions.ShowMessage(btn.Content.ToString());
            }
        }

        public void GradeNavigate()
        {
            if((App.Current as App).user_aao_status.Equals("1"))
            {
                
            }
        }
    }
}
