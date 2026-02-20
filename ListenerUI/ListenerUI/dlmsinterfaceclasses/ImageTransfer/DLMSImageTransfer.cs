using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReader.DLMSInterfaceClasses.ImageTransfer
{
    [Serializable]
    public class DLMSImageTransfer
    {
        public string logical_name { get; set; }//1
        public string image_block_size { get; set; }//2
        public string image_transferred_blocks_status { get; set; }//3
        public string image_first_not_transferred_block_number { get; set; }//4
        public string image_transfer_enabled { get; set; }//5
        public string image_transfer_status { get; set; }//6
        public string image_to_activate_info { get; set; }//7
    }
}
