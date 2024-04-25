using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAndroidSerialPortNative
{
    public enum Parity
    {
        //public const uint PARENB = 0x0100; // 启用奇偶校验
        //public const uint PARODD = 0x0200; // 如果清除则使用偶校验，如果设置则使用奇校验
            //
            // 摘要:
            //     No parity check occurs.
            None,
            //
            // 摘要:
            //     Sets the parity bit so that the count of bits set is an odd number.
            Odd,
            //
            // 摘要:
            //     Sets the parity bit so that the count of bits set is an even number.
            Even
    }
}
