using System;
using System.Threading.Tasks;
using NSerial.Constants;
using NSerial.Core;
using NSerial.Model;

namespace NSerial.Control;

/// <summary>
/// Manages control pins of RS232 serial port.
/// </summary>
public class SerialControlPinManager : IControlPinManager, IPinSignalSender
{
    private readonly ISerialPortWrapper m_serialPortWrapper;
    private readonly IDelay m_delay;

    /// <summary>
    /// Creates a new instance of <see cref="SerialControlPinManager"/>.
    /// </summary>
    /// <param name="pin">Pin to be controlled.</param>
    /// <param name="serialPortWrapper">Serial port wrapper to which the control pin is attached.</param>
    /// <param name="delay">Delay object used to hold the pin for a certain duration.</param>
    public SerialControlPinManager(ControlPin pin, ISerialPortWrapper serialPortWrapper, IDelay delay)
    {
        Pin = pin;
        m_serialPortWrapper = serialPortWrapper;
        m_delay = delay;
    }

    #region Implementation of IControlPinManager

    /// <inheritdoc cref="IControlPinManager.EnablePin"/>
    public Task EnablePin()
    {
        switch (Pin)
        {
            case ControlPin.DTR:
                m_serialPortWrapper.DtrEnable = true;
                break;
            case ControlPin.RTS:
                m_serialPortWrapper.RtsEnable = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        State = PinState.Enabled;
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IControlPinManager.EnablePinFor" />
    public async Task EnablePinFor(TimeSpan duration)
    {
        switch (Pin)
        {
            case ControlPin.DTR:
                m_serialPortWrapper.DtrEnable = true;
                State = PinState.Enabled;
                await m_delay.Delay(ComputeHoldDuration(duration));
                m_serialPortWrapper.DtrEnable = false;
                State = PinState.Disabled;
                break;
            case ControlPin.RTS:
                m_serialPortWrapper.RtsEnable = true;
                State = PinState.Enabled;
                await m_delay.Delay(ComputeHoldDuration(duration));
                m_serialPortWrapper.RtsEnable = false;
                State = PinState.Disabled;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <inheritdoc cref="IControlPinManager.DisablePin" />
    public Task DisablePin()
    {
        switch (Pin)
        {
            case ControlPin.DTR:
                m_serialPortWrapper.DtrEnable = false;
                break;
            case ControlPin.RTS:
                m_serialPortWrapper.RtsEnable = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        State = PinState.Disabled;
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IControlPinManager.DisablePinFor" />
    public async Task DisablePinFor(TimeSpan duration)
    {
        switch (Pin)
        {
            case ControlPin.DTR:
                m_serialPortWrapper.DtrEnable = false;
                State = PinState.Disabled;
                await m_delay.Delay(ComputeHoldDuration(duration));
                m_serialPortWrapper.DtrEnable = true;
                State = PinState.Enabled;
                break;
            case ControlPin.RTS:
                m_serialPortWrapper.RtsEnable = false;
                State = PinState.Disabled;
                await m_delay.Delay(ComputeHoldDuration(duration));
                m_serialPortWrapper.RtsEnable = true;
                State = PinState.Enabled;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <inheritdoc cref="IControlPinManager.TogglePin" />
    public Task TogglePin()
    {
        switch (Pin)
        {
            case ControlPin.DTR:
                m_serialPortWrapper.DtrEnable = !m_serialPortWrapper.DtrEnable;
                break;
            case ControlPin.RTS:
                m_serialPortWrapper.RtsEnable = !m_serialPortWrapper.RtsEnable;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        State = State.Toggle();
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IControlPinManager.TogglePinFor" />
    public async Task TogglePinFor(TimeSpan duration)
    {
        switch (Pin)
        {
            case ControlPin.DTR:
                m_serialPortWrapper.DtrEnable = !m_serialPortWrapper.DtrEnable;
                State = State.Toggle();
                await m_delay.Delay(ComputeHoldDuration(duration));
                m_serialPortWrapper.DtrEnable = !m_serialPortWrapper.DtrEnable;
                State = State.Toggle();
                break;
            case ControlPin.RTS:
                m_serialPortWrapper.RtsEnable = !m_serialPortWrapper.RtsEnable;
                State = State.Toggle();
                await m_delay.Delay(ComputeHoldDuration(duration));
                m_serialPortWrapper.RtsEnable = !m_serialPortWrapper.RtsEnable;
                State = State.Toggle();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    #region Implementation of IPinSignalSender

    /// <inheritdoc cref="IPinSignalSender.SendSignal" />
    public Task SendSignal()
    {
        return SendSignalFor(TimeSpan.Zero);
    }

    /// <inheritdoc cref="IPinSignalSender.SendSignalFor" />
    public Task SendSignalFor(TimeSpan duration)
    {
        return TogglePinFor(duration);
    }

    #endregion

    private static TimeSpan ComputeHoldDuration(TimeSpan duration)
    {
        return duration < Timings.MinimumSignalSwitchTime
            ? Timings.MinimumSignalSwitchTime
            : duration;
    }

    /// <inheritdoc cref="IControlPinManager.Pin" />
    public ControlPin Pin { get; }

    /// <inheritdoc cref="IControlPinManager.State" />
    public PinState State { get; private set; }
}