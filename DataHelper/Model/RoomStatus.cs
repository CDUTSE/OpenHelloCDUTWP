using DataHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 你好理工.DataHelper.Model
{
    /// <summary>
    /// 教室状态
    /// </summary>
    public class RoomStatus:ResultBase
    {
        public List<roomsItem> rooms { get; set; }

    }
    public struct roomsItem
    {
        public string roomName { get; set; }
        public string seatNum { get; set; }
        /// <summary>
        /// 教室状态 0空闲 1借用 2教学
        /// </summary>
        public int[] status { get; set; }        
    }
}
