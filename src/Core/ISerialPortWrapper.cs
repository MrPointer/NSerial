using System;
using System.IO;
using System.IO.Ports;
using System.Text;

namespace NSerial.Core;

/// <summary>
/// Wrapper for <see cref="SerialPort"/> to allow testability.
/// </summary>
public interface ISerialPortWrapper : IDisposable
{
    /// <summary>
    /// Closes the port connection, sets the IsOpen property to false, and disposes of the internal Stream object.
    /// </summary>
    void Close();

    /// <summary>
    /// Discards data from the serial driver's receive buffer.
    /// </summary>
    void DiscardInBuffer();

    /// <summary>
    /// Discards data from the serial driver's transmit buffer.
    /// </summary>
    void DiscardOutBuffer();

    /// <summary>
    /// Opens a new serial port connection.
    /// </summary>
    /// <exception cref="UnauthorizedAccessException">
    /// Access is denied to the port.
    /// -or-
    /// The current process, or another process on the system, already has the specified COM port open either by a
    /// SerialPort instance or in unmanaged code.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// One or more of the properties for this instance are invalid.For example, the Parity, DataBits, or
    /// Handshake properties are not valid values;
    /// the BaudRate is less than or equal to zero;
    /// the ReadTimeout or WriteTimeout property is less than zero and is not InfiniteTimeout.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The port name does not begin with "COM", or the file type of the port is not supported.
    /// -or-
    /// The file type of the port is not supported.
    /// </exception>
    /// <exception cref="IOException">
    /// The port is in an invalid state.
    /// -or-
    /// An attempt to set the state of the underlying port failed. For example, the parameters passed from this
    /// <see cref="ISerialPortWrapper"/> object were invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The specified port on the current instance of the <see cref="ISerialPortWrapper"/> is already open.
    /// </exception>
    void Open();

    /// <summary>
    /// Reads a number of bytes from the <see cref="ISerialPortWrapper"/> input buffer and writes
    /// them into an array of bytes at a given offset.
    /// </summary>
    /// <param name="buffer">The byte array to write the input to.</param>
    /// <param name="offset">The offset in the buffer array to begin writing.</param>
    /// <param name="count">The maximum number of bytes to read.
    /// Fewer bytes are read if count is greater than the number of bytes in the input buffer.</param>
    /// <returns>The number of bytes read.</returns>
    int Read(byte[] buffer, int offset, int count);

    /// <summary>
    /// Reads a number of characters from the <see cref="ISerialPortWrapper"/> input buffer and writes
    /// them into an array of characters at a given offset.
    /// </summary>
    /// <param name="buffer">The character array to write the input to.</param>
    /// <param name="offset">The offset in the buffer at which to write the characters.</param>
    /// <param name="count">The maximum number of characters to read.
    /// Fewer characters are read if count is greater than the number of characters in the input buffer.</param>
    /// <returns>The number of characters read.</returns>
    int Read(char[] buffer, int offset, int count);

    /// <summary>
    /// Reads a byte from the input buffer.
    /// </summary>
    /// <returns>The byte, cast to an Int32, or -1 if the end of the stream has been read.</returns>
    /// <exception cref="InvalidOperationException">The specified port is not open.</exception>
    /// <exception cref="TimeoutException">
    /// The operation did not complete before the time-out period ended.
    /// -or-
    /// No byte was read.
    /// </exception>
    /// <remarks>
    /// This method reads one byte.
    /// Use caution when using ReadByte and ReadChar together.
    /// Switching between reading bytes and reading characters can cause
    /// extra data to be read and/or other unintended behavior.
    /// If it is necessary to switch between reading text and reading binary data from the stream,
    /// select a protocol that carefully defines the boundary between text and binary data,
    /// such as manually reading bytes and decoding the data.
    /// </remarks>
    int ReadByte();

    /// <summary>
    /// Synchronously reads one character from the SerialPort input buffer.
    /// </summary>
    /// <returns>The character that was read.</returns>
    /// <exception cref="InvalidOperationException">The specified port is not open.</exception>
    /// <exception cref="TimeoutException">
    /// The operation did not complete before the time-out period ended.
    /// -or-
    /// No character was available in the allotted time-out period.
    /// </exception>
    /// <remarks>
    /// This method reads one complete character based on the encoding.
    /// Use caution when using ReadByte and ReadChar together.
    /// Switching between reading bytes and reading characters can cause
    /// extra data to be read and/or other unintended behavior.
    /// If it is necessary to switch between reading text and reading binary data from the stream,
    /// select a protocol that carefully defines the boundary between text and binary data,
    /// such as manually reading bytes and decoding the data.
    /// </remarks>
    int ReadChar();

    string ReadExisting();

    string ReadLine();

    string ReadTo(string value);

    void Write(string text);

    void Write(byte[] buffer, int offset, int count);

    void Write(char[] buffer, int offset, int count);

    void WriteLine(string text);

    public event SerialDataReceivedEventHandler DataReceived;

    public event SerialErrorReceivedEventHandler ErrorReceived;

    public event SerialPinChangedEventHandler PinChanged;

    public int BytesToRead { get; }

    public int ReadBufferSize { get; set; }

    public int ReadTimeout { get; set; }

    public int ReceivedBytesThreshold { get; set; }

    public int BytesToWrite { get; }

    public int WriteBufferSize { get; set; }

    public int WriteTimeout { get; set; }

    public Stream BaseStream { get; }

    public string PortName { get; set; }

    public int BaudRate { get; set; }

    public int DataBits { get; set; }

    public StopBits StopBits { get; set; }

    public Parity Parity { get; set; }

    public byte ParityReplace { get; set; }

    public Handshake Handshake { get; set; }

    public bool BreakState { get; set; }

    public bool CDHolding { get; }

    public bool CtsHolding { get; }

    public bool DsrHolding { get; }

    public bool DtrEnable { get; set; }

    public bool RtsEnable { get; set; }

    public Encoding Encoding { get; set; }

    public string NewLine { get; set; }

    public bool DiscardNull { get; set; }

    public bool IsOpen { get; }

    public SerialPort WrappedSerialPort { get; }
}