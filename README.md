# MauiAndroidSerialPortNative
由于微软官方的串口不支持安卓的操作系统，我自己要在maui平台创建使用串口，所以创建的这个库，使用方面和微软官方的差不多

初始化
var serialPort = new SerialPort("/dev/ttyS7", BaudRate.B38400, Parity.Even, DataBits.CS8, StopBits.One);
