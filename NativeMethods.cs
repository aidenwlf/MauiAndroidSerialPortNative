using System.Runtime.InteropServices;

namespace MauiAndroidSerialPortNative
{
    using ssize_t = System.IntPtr;
    using size_t = System.IntPtr;
    internal static class NativeMethods
    {
        [DllImport("libc.so", SetLastError = true)]
        public static extern int open(string pathname, int flags);

        [DllImport("libc.so", SetLastError = true)]
        public static extern int close(int fd);

        [DllImport("libc.so", SetLastError = true)]
        public static extern ssize_t read(int fd, byte[] buf, size_t count);

        [DllImport("libc.so", SetLastError = true)]
        public static extern ssize_t write(int fd, byte[] buf, size_t count);

        [DllImport("libc.so", SetLastError = true)]
        public static extern int tcgetattr(int fd, out Termios termios);

        [DllImport("libc.so", SetLastError = true)]
        public static extern int tcsetattr(int fd, int optional_actions, ref Termios termios);

        [DllImport("libc.so", SetLastError = true)]
        public static extern int cfsetispeed(ref Termios termios, uint speed);

        [DllImport("libc.so", SetLastError = true)]
        public static extern int cfsetospeed(ref Termios termios, uint speed);

        [DllImport("libc.so", SetLastError = true)]
        public static extern int tcflush(int fd, int queue_selector);

        private const int FIONREAD = 0x541B;

        [DllImport("libc.so", SetLastError = true)]
        public static extern int ioctl(int fd, int request, ref int data);

        [DllImport("libc.so", SetLastError = true)]
        public static extern int select(int nfds, ref Fd_set readfds, IntPtr writefds, IntPtr exceptfds, ref Timeval timeout);

        public const int FD_SETSIZE = 64;

        public static void FD_SET(int fd, ref Fd_set set)
        {
            set.fds_bits0 |= ((ulong)1 << (fd % FD_SETSIZE));
        }

        public static void FD_ZERO(ref Fd_set set)
        {
            set.fds_bits0 = 0;
        }

        public static bool FD_ISSET(int fd, ref Fd_set set)
        {
            return (set.fds_bits0 & ((ulong)1 << (fd % FD_SETSIZE))) != 0;
        }

        public static int GetPendingDataLength(int serial_fd)
        {
            int pendingBytes = 0;
            if (ioctl(serial_fd, FIONREAD, ref pendingBytes) < 0)
            {
                throw new InvalidOperationException("Failed to get pending data length.");
            }
            return pendingBytes;
        }
    }




    [StructLayout(LayoutKind.Sequential)]
    public struct Timeval
    {
        public long tv_sec;
        public long tv_usec;
    }
    [StructLayout(LayoutKind.Explicit, Size = 128)]
    public struct Fd_set
    {
        [FieldOffset(0)] public ulong fds_bits0;
        // 可以根据需要扩展此结构
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Termios
    {
        public uint c_iflag;
        public uint c_oflag;
        public uint c_cflag;
        public uint c_lflag;
        public byte c_line;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] c_cc;
        //public uint c_ispeed;
        //public uint c_ospeed;
    }
}
