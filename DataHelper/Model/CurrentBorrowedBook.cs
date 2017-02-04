using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHelper.Model
{
    public class CurrentBorrowedBook:ResultBase
    {
        public BorrowedBook[] borrowed_book { get; set; }
    }
    public class BorrowedBook
    {
        public string barCode{get;set;}
        public string title{get;set;}
        public string borrowTime{get;set;}
        public string returnTime{get;set;}
        public string renewTime{get;set;}
        public string bookRenewHref{get;set;}
        public string bookIndexCode{get;set;}
        public string bookLocation{get;set;}
    }
}
