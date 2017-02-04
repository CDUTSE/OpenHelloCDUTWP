using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 你好理工.DataHelper.Model;

namespace 你好理工.DesignTime
{
    public class RoomStatusViewModel
    {
        public List<roomsItem> RoomList { get; set; }

        public RoomStatusViewModel()
        {
            RoomList = new List<roomsItem>();

            LoadData();
        }

        private void LoadData()
        {
            string jsonText = "{\"result\":true,\"rooms\":[{\"roomName\":\"1101\",\"seatNum\":\"192\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1103\",\"seatNum\":\"45\",\"status\":[0,0,2,0,0]},{\"roomName\":\"1104\",\"seatNum\":\"45\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1105\",\"seatNum\":\"45\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1106\",\"seatNum\":\"45\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1107\",\"seatNum\":\"239\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1109\",\"seatNum\":\"48\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1201\",\"seatNum\":\"160\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1202\",\"seatNum\":\"69\",\"status\":[1,0,0,0,0]},{\"roomName\":\"1203\",\"seatNum\":\"45\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1204\",\"seatNum\":\"45\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1205\",\"seatNum\":\"45\",\"status\":[1,1,0,0,0]},{\"roomName\":\"1206\",\"seatNum\":\"45\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1207\",\"seatNum\":\"160\",\"status\":[1,0,0,0,0]},{\"roomName\":\"1208\",\"seatNum\":\"50\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1209\",\"seatNum\":\"48\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1301\",\"seatNum\":\"160\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1302\",\"seatNum\":\"70\",\"status\":[1,1,0,0,0]},{\"roomName\":\"1303\",\"seatNum\":\"104\",\"status\":[1,0,0,0,0]},{\"roomName\":\"1305\",\"seatNum\":\"104\",\"status\":[0,0,0,0,0]},{\"roomName\":\"1307\",\"seatNum\":\"160\",\"status\":[1,0,0,0,0]},{\"roomName\":\"1308\",\"seatNum\":\"70\",\"status\":[1,1,0,0,0]},{\"roomName\":\"1309\",\"seatNum\":\"48\",\"status\":[0,0,0,0,0]}]}";
            RoomStatus roomStatus =  你好理工.DataHelper.Helper.Functions.Deserlialize<RoomStatus>(jsonText);
            RoomList.AddRange(roomStatus.rooms);
        }
    }
}
