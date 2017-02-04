using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHelper.Model
{
    public class BorrowHistory:ResultBase
    {
        public history history { get; set; }
    }
    public struct history
    {
        public int current_page { get; set; }
        public int total_page { get; set; }
        public book[] borrow_history { get; set; }
    }
    public struct book
    {
        public string title{get;set;}
        public string login_number{get;set;}
        public string bar_code{get;set;}
        public DateTime handle_time{get;set;}
        public string handle_type{get;set;}
    }
}
