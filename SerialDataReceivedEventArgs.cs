using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAndroidSerialPortNative
{
    public class SerialDataReceivedEventArgs : EventArgs
    {
        public int ReadBufferSize { get; set; }
        // 可以添加其他属性，例如读取到的数据长度等
    }

    public delegate void SerialDataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e);
}
