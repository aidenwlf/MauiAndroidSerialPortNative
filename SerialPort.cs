using AndroidX.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAndroidSerialPortNative
{
    using ssize_t = System.IntPtr;
    using size_t = System.IntPtr;
    public class SerialPort : IDisposable
    {
        private const int O_RDWR = 2;       // 打开文件为读写模式
        private const int O_NOCTTY = 0x100;   // 不使文件成为进程的控制终端

        private int fd;

        public int WriteTimeout { get; set; }

        public int ReadTimeout { get; set; } = 5;
        

        public static int InfiniteTimeout { get; set; } = 9999;

        public SerialPort(string portName, BaudRate baudRate = BaudRate.B9600, Parity parity = Parity.None, DataBits dataBits = DataBits.CS8, StopBits stopBits = StopBits.One)
        {
            fd = Open(portName);
            if (NativeMethods.tcgetattr(fd, out Termios currentTermios) != 0)
            {
                // 错误处理
            }

            if (NativeMethods.cfsetispeed(ref currentTermios, (uint)baudRate) != 0 ||
                NativeMethods.cfsetospeed(ref currentTermios, (uint)baudRate) != 0)
            {
                // 错误处理
            }

            SetParity(ref currentTermios, parity);
            SetCSTOP(ref currentTermios, stopBits);
            SetDateBits(ref currentTermios, (uint)dataBits);
            //Task.Run(Update);
        }

        private void Update(){

            while (true)
            {
                Fd_set readfds = new Fd_set();
                NativeMethods.FD_ZERO(ref readfds);
                NativeMethods.FD_SET(fd, ref readfds);
                Timeval timeout;
                timeout.tv_sec = ReadTimeout;
                timeout.tv_usec = 0;
                int retval = NativeMethods.select(fd + 1, ref readfds, IntPtr.Zero, IntPtr.Zero, ref timeout);
                if (retval > 0 && NativeMethods.FD_ISSET(fd, ref readfds))
                {
                    OnDataReceived(new SerialDataReceivedEventArgs());
                }
            } 
        }

        public int Read(byte[] data, int offset, int length)
        {
            ssize_t bytesRead = 0;
            Fd_set readfds = new Fd_set();
            NativeMethods.FD_ZERO(ref readfds);
            NativeMethods.FD_SET(fd, ref readfds);
            Timeval timeout;
            timeout.tv_sec = ReadTimeout;
            timeout.tv_usec = 0;
            int retval = NativeMethods.select(fd + 1, ref readfds, IntPtr.Zero, IntPtr.Zero, ref timeout);
            if (retval == -1)
            {
                //Console.WriteLine("Error in select.");
            }
            else if (retval > 0 && NativeMethods.FD_ISSET(fd, ref readfds))
            {
                if (offset + length > data.Length)
                {
                    throw new ArgumentException("Offset and length exceed data array size.");
                }

                byte[] buffer = new byte[length];

                bytesRead = NativeMethods.read(fd, buffer, length);

                if (bytesRead > 0)
                {
                    Array.Copy(buffer, 0, data, offset, bytesRead);
                }

                //Console.WriteLine("Data available to read.");
            }
            else
            {
                // 超时
                //Console.WriteLine("No data within five seconds.");
            }
            return (int)bytesRead;
        }

        public void Write(byte[] data, int offset, int length)
        {

            if (offset + length > data.Length)
            {
                throw new ArgumentException("Offset and length exceed data array size.");
            }

            byte[] buffer = new byte[length];
            Array.Copy(data, offset, buffer, 0, length);
            ssize_t bytesWritten = NativeMethods.write(fd, buffer, length);
            if (bytesWritten < 0)
            {
                // 错误处理
            }
        }
        //private const int TCIFLUSH = 0;
        //private const int TCOFLUSH = 1;
        private const int TCIOFLUSH = 2;
        public void DiscardInBuffer()
        {
            if (NativeMethods.tcflush(fd, TCIOFLUSH) != 0)
            {
                // 错误处理
            }
        }


        public event SerialDataReceivedEventHandler DataReceived;

        protected virtual void OnDataReceived(SerialDataReceivedEventArgs e)
        {
            var t =NativeMethods.GetPendingDataLength(fd);
            e.ReadBufferSize = t;
            DataReceived?.Invoke(this, e);
        }

        private int Open(string portName)
        {

            int fd = NativeMethods.open(portName, O_RDWR | O_NOCTTY);
            if (fd < 0)
            {
                // 错误处理
            }
            return fd;
        }

        private const uint PARENB = 0x0100; // 启用奇偶校验
        private const uint PARODD = 0x0200;

        private void SetParity(ref Termios currentTermios, Parity parity)
        {
            if (parity== Parity.None)
            {
                currentTermios.c_cflag &= ~PARENB;
            }
            else if(parity == Parity.Odd)
            {
                currentTermios.c_cflag |= PARENB | PARODD;
            }
            else if (parity == Parity.Even)
            {
                currentTermios.c_cflag |= PARENB;  // 启用奇偶校验
                currentTermios.c_cflag &= ~PARODD; // 使用偶校验
            }
        }

        private const uint CSTOPB = 0x0400;

        private void SetCSTOP(ref Termios currentTermios, StopBits stopBits)
        {
            if (stopBits == StopBits.One)
            {
                currentTermios.c_cflag &= ~CSTOPB;
            }
            else if(stopBits == StopBits.Two)
            {
                currentTermios.c_cflag |= CSTOPB;
            }
        }
        private const uint Qcsjw = 0x0030;
        private void SetDateBits(ref Termios currentTermios,uint dataBits)
        {
            currentTermios.c_cflag &= ~Qcsjw;  // 清除数据位设置
            currentTermios.c_cflag |= dataBits;      // 设置为7位数据
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //告诉GC不需要再次调用
        }
        private bool _isDisposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    //释放托管资源
                }
                NativeMethods.close(fd);
                //serialPort = IntPtr.Zero;
                // 释放非托管资源
                // 释放大对象

                this._isDisposed = true;
            }

        }
    }
}
