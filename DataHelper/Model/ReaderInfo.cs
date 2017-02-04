using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHelper.Model
{
    public class ReaderInfo:ResultBase
    {
        public reader reader { get; set; }   
    }

    public struct reader
    {
        public string readerNum { get; set; }
        public string readerName { get; set; }
        public string readerUnit { get; set; }
        public string readerDegree { get; set; }
        public double readerFine { get; set; }
        public double readerReparation { get; set; }
        public double readerBookCount { get; set; }
    }
}
