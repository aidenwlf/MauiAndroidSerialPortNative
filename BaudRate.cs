using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAndroidSerialPortNative
{
    public enum BaudRate
    {
        B0 = 0,   // hang up
        B50 = 1,
        B75 = 2,
        B110 = 3,
        B134 = 4,
        B150 = 5,
        B200 = 6,
        B300 = 7,
        B600 = 8,
        B1200 = 9,
        B1800 = 10,
        B2400 = 11,
        B4800 = 12,
        B9600 = 13,
        B19200 = 14,     // Alias: B38400 in some systems
        B38400 = 15,
        B57600 = 4097,
        B115200 = 4098,
        B230400 = 4099,
        B460800 = 4100,
        B500000 = 4101,
        B576000 = 4102,
        B921600 = 4103,
        B1000000 = 4104,
        B1152000 = 4105,
        B1500000 = 4106,
        B2000000 = 4107,
        B2500000 = 4108,
        B3000000 = 4109,
        B3500000 = 4110,
        B4000000 = 4111,
    }
}
