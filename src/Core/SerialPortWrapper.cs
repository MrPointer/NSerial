using System.IO;
using System.IO.Ports;
using System.Text;

namespace NSerial.Core;

public class SerialPortWrapper : ISerialPortWrapper
{
    /// <summary>
    /// The wrapped <see cref="SerialPort"/> instance.
    /// </summary>
    /// <param name="wrappedSerialPort"></param>
    public SerialPortWrapper(SerialPort wrappedSerialPort)
    {
        WrappedSerialPort = wrappedSerialPort;
        WrappedSerialPort.DataReceived += (sender, args) =>
        {
            BytesToRead = WrappedSerialPort.BytesToRead;
            DataReceived?.Invoke(sender, args);
        };
        WrappedSerialPort.ErrorReceived += (sender, args) => ErrorReceived?.Invoke(sender, args);
        WrappedSerialPort.PinChanged += (sender, args) => PinChanged?.Invoke(sender, args);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        BaseStream.Dispose();
        WrappedSerialPort.Dispose();
    }

    /// <inheritdoc />
    public void Close()
    {
        WrappedSerialPort.Close();
    }

    /// <inheritdoc />
    public void DiscardInBuffer()
    {
        WrappedSerialPort.DiscardInBuffer();
        BytesToRead = WrappedSerialPort.BytesToRead;
    }

    /// <inheritdoc />
    public void DiscardOutBuffer()
    {
        WrappedSerialPort.DiscardOutBuffer();
        BytesToWrite = WrappedSerialPort.BytesToWrite;
    }

    /// <inheritdoc />
    public void Open()
    {
        WrappedSerialPort.Open();
    }

    /// <inheritdoc />
    public int Read(byte[] buffer, int offset, int count)
    {
        int readBytes = WrappedSerialPort.Read(buffer, offset, count);
        BytesToRead = WrappedSerialPort.BytesToRead;
        return readBytes;
    }

    /// <inheritdoc />
    public int Read(char[] buffer, int offset, int count)
    {
        int readBytes = WrappedSerialPort.Read(buffer, offset, count);
        BytesToRead = WrappedSerialPort.BytesToRead;
        return readBytes;
    }

    /// <inheritdoc />
    public int ReadByte()
    {
        int readByte = WrappedSerialPort.ReadByte();
        BytesToRead = WrappedSerialPort.BytesToRead;
        return readByte;
    }

    /// <inheritdoc />
    public int ReadChar()
    {
        int readChar = WrappedSerialPort.ReadChar();
        BytesToRead = WrappedSerialPort.BytesToRead;
        return readChar;
    }

    /// <inheritdoc />
    public string ReadExisting()
    {
        string readString = WrappedSerialPort.ReadExisting();
        BytesToRead = WrappedSerialPort.BytesToRead;
        return readString;
    }

    /// <inheritdoc />
    public string ReadLine()
    {
        string readLine = WrappedSerialPort.ReadLine();
        BytesToRead = WrappedSerialPort.BytesToRead;
        return readLine;
    }

    /// <inheritdoc />
    public string ReadTo(string value)
    {
        string readString = WrappedSerialPort.ReadTo(value);
        BytesToRead = WrappedSerialPort.BytesToRead;
        return readString;
    }

    /// <inheritdoc />
    public void Write(string text)
    {
        WrappedSerialPort.Write(text);
        BytesToWrite = WrappedSerialPort.BytesToWrite;
    }

    /// <inheritdoc />
    public void Write(byte[] buffer, int offset, int count)
    {
        WrappedSerialPort.Write(buffer, offset, count);
        BytesToWrite = WrappedSerialPort.BytesToWrite;
    }

    /// <inheritdoc />
    public void Write(char[] buffer, int offset, int count)
    {
        WrappedSerialPort.Write(buffer, offset, count);
        BytesToWrite = WrappedSerialPort.BytesToWrite;
    }

    /// <inheritdoc />
    public void WriteLine(string text)
    {
        WrappedSerialPort.WriteLine(text);
        BytesToWrite = WrappedSerialPort.BytesToWrite;
    }

    /// <inheritdoc />
    public event SerialDataReceivedEventHandler? DataReceived;

    /// <inheritdoc />
    public event SerialErrorReceivedEventHandler? ErrorReceived;

    /// <inheritdoc />
    public event SerialPinChangedEventHandler? PinChanged;

    /// <inheritdoc />
    public int BytesToRead { get; private set; }

    /// <inheritdoc />
    public int ReadBufferSize
    {
        get => WrappedSerialPort.ReadBufferSize;
        set => WrappedSerialPort.ReadBufferSize = value;
    }

    /// <inheritdoc />
    public int ReadTimeout
    {
        get => WrappedSerialPort.ReadTimeout;
        set => WrappedSerialPort.ReadTimeout = value;
    }

    /// <inheritdoc />
    public int ReceivedBytesThreshold
    {
        get => WrappedSerialPort.ReceivedBytesThreshold;
        set => WrappedSerialPort.ReceivedBytesThreshold = value;
    }

    /// <inheritdoc />
    public int BytesToWrite { get; private set; }

    /// <inheritdoc />
    public int WriteBufferSize
    {
        get => WrappedSerialPort.WriteBufferSize;
        set => WrappedSerialPort.WriteBufferSize = value;
    }

    /// <inheritdoc />
    public int WriteTimeout
    {
        get => WrappedSerialPort.WriteTimeout;
        set => WrappedSerialPort.WriteTimeout = value;
    }

    /// <inheritdoc />
    public Stream BaseStream => WrappedSerialPort.BaseStream;

    /// <inheritdoc />
    public string PortName
    {
        get => WrappedSerialPort.PortName;
        set => WrappedSerialPort.PortName = value;
    }

    /// <inheritdoc />
    public int BaudRate
    {
        get => WrappedSerialPort.BaudRate;
        set => WrappedSerialPort.BaudRate = value;
    }

    /// <inheritdoc />
    public int DataBits
    {
        get => WrappedSerialPort.DataBits;
        set => WrappedSerialPort.DataBits = value;
    }

    /// <inheritdoc />
    public StopBits StopBits
    {
        get => WrappedSerialPort.StopBits;
        set => WrappedSerialPort.StopBits = value;
    }

    /// <inheritdoc />
    public Parity Parity
    {
        get => WrappedSerialPort.Parity;
        set => WrappedSerialPort.Parity = value;
    }

    /// <inheritdoc />
    public byte ParityReplace
    {
        get => WrappedSerialPort.ParityReplace;
        set => WrappedSerialPort.ParityReplace = value;
    }

    /// <inheritdoc />
    public Handshake Handshake
    {
        get => WrappedSerialPort.Handshake;
        set => WrappedSerialPort.Handshake = value;
    }

    /// <inheritdoc />
    public bool BreakState
    {
        get => WrappedSerialPort.BreakState;
        set => WrappedSerialPort.BreakState = value;
    }

    /// <inheritdoc />
    public bool CDHolding => WrappedSerialPort.CDHolding;

    /// <inheritdoc />
    public bool CtsHolding => WrappedSerialPort.CtsHolding;

    /// <inheritdoc />
    public bool DsrHolding => WrappedSerialPort.DsrHolding;

    /// <inheritdoc />
    public bool DtrEnable
    {
        get => WrappedSerialPort.DtrEnable;
        set => WrappedSerialPort.DtrEnable = value;
    }

    /// <inheritdoc />
    public bool RtsEnable
    {
        get => WrappedSerialPort.RtsEnable;
        set => WrappedSerialPort.RtsEnable = value;
    }

    /// <inheritdoc />
    public Encoding Encoding
    {
        get => WrappedSerialPort.Encoding;
        set => WrappedSerialPort.Encoding = value;
    }

    /// <inheritdoc />
    public string NewLine
    {
        get => WrappedSerialPort.NewLine;
        set => WrappedSerialPort.NewLine = value;
    }

    /// <inheritdoc />
    public bool DiscardNull
    {
        get => WrappedSerialPort.DiscardNull;
        set => WrappedSerialPort.DiscardNull = value;
    }

    /// <inheritdoc />
    public bool IsOpen => WrappedSerialPort.IsOpen;

    /// <inheritdoc />
    public SerialPort WrappedSerialPort { get; }
}