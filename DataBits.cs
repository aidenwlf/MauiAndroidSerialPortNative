using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAndroidSerialPortNative
{
    public enum DataBits
    {
        CS5 = 0x0000,  // 5数据位
        CS6 = 0x0010,  // 6数据位
        CS7 = 0x0020,  // 7数据位
        CS8 = 0x0030,  // 8数据位
    }
}
