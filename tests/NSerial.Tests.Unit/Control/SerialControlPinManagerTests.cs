using FluentAssertions;
using NSerial.Constants;
using NSerial.Control;
using NSerial.Core;
using NSerial.Model;
using NSubstitute;

namespace NSerial.Tests.Unit.Control;

public class SerialControlPinManagerTests
{
    public static IEnumerable<TimeSpan> DurationTestData = new[]
    {
        TimeSpan.Zero,
        Timings.MinimumSignalSwitchTime - TimeSpan.FromMilliseconds(50),
        Timings.MinimumSignalSwitchTime,
        Timings.MinimumSignalSwitchTime + TimeSpan.FromMilliseconds(200)
    };

    [Theory]
    [CombinatorialData]
    public async Task PinCanBeEnabled(ControlPin pin)
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var delayMock = Substitute.For<IDelay>();
        var controlPinManager = new SerialControlPinManager(pin, serialPortWrapperMock, delayMock);

        // Act
        await controlPinManager.EnablePin();

        // Assert
        controlPinManager.State.Should().Be(PinState.Enabled);

        switch (pin)
        {
            case ControlPin.DTR:
                serialPortWrapperMock.Received().DtrEnable = true;
                break;
            case ControlPin.RTS:
                serialPortWrapperMock.Received().RtsEnable = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pin), pin, null);
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task PinCanBeEnabledForDuration(ControlPin pin,
        [CombinatorialMemberData(nameof(DurationTestData))]
        TimeSpan enableDuration)
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);
        var controlPinManager = new SerialControlPinManager(pin, serialPortWrapperMock, delayMock);

        // Act
        await controlPinManager.EnablePinFor(enableDuration);

        // Assert
        controlPinManager.State.Should().Be(PinState.Disabled);

        switch (pin)
        {
            case ControlPin.DTR:
                serialPortWrapperMock.Received().DtrEnable = true;
                serialPortWrapperMock.Received().DtrEnable = false;
                break;
            case ControlPin.RTS:
                serialPortWrapperMock.Received().RtsEnable = true;
                serialPortWrapperMock.Received().RtsEnable = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pin), pin, null);
        }

        await delayMock.Received().Delay(Arg.Is<TimeSpan>(duration => duration >= Timings.MinimumSignalSwitchTime));
    }

    [Theory]
    [CombinatorialData]
    public async Task PinCanBeDisabled(ControlPin pin)
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var delayMock = Substitute.For<IDelay>();
        var controlPinManager = new SerialControlPinManager(pin, serialPortWrapperMock, delayMock);

        // Act
        await controlPinManager.DisablePin();

        // Assert
        controlPinManager.State.Should().Be(PinState.Disabled);

        switch (pin)
        {
            case ControlPin.DTR:
                serialPortWrapperMock.Received().DtrEnable = false;
                break;
            case ControlPin.RTS:
                serialPortWrapperMock.Received().RtsEnable = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pin), pin, null);
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task PinCanBeDisabledForDuration(ControlPin pin,
        [CombinatorialMemberData(nameof(DurationTestData))]
        TimeSpan disableDuration)
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);
        var controlPinManager = new SerialControlPinManager(pin, serialPortWrapperMock, delayMock);

        // Act
        await controlPinManager.DisablePinFor(disableDuration);

        // Assert
        controlPinManager.State.Should().Be(PinState.Enabled);

        switch (pin)
        {
            case ControlPin.DTR:
                serialPortWrapperMock.Received().DtrEnable = false;
                serialPortWrapperMock.Received().DtrEnable = true;
                break;
            case ControlPin.RTS:
                serialPortWrapperMock.Received().RtsEnable = false;
                serialPortWrapperMock.Received().RtsEnable = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pin), pin, null);
        }

        await delayMock.Received().Delay(Arg.Is<TimeSpan>(duration => duration >= Timings.MinimumSignalSwitchTime));
    }

    [Theory]
    [CombinatorialData]
    public async Task PinCanBeToggled(ControlPin pin, PinState initialState)
    {
        // Arrange
        var expectedFinalState = initialState switch
        {
            PinState.Disabled => PinState.Enabled,
            PinState.Enabled => PinState.Disabled,
            _ => throw new ArgumentOutOfRangeException(nameof(initialState), initialState, null)
        };

        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);
        var controlPinManager = new SerialControlPinManager(pin, serialPortWrapperMock, delayMock);

        if (initialState == PinState.Enabled)
        {
            await controlPinManager.EnablePin();
        }
        else
        {
            await controlPinManager.DisablePin();
        }

        // Act
        await controlPinManager.TogglePin();

        // Assert
        controlPinManager.State.Should().Be(expectedFinalState);

        switch (pin)
        {
            case ControlPin.DTR:
                serialPortWrapperMock.Received().DtrEnable = true;
                serialPortWrapperMock.Received().DtrEnable = false;
                break;
            case ControlPin.RTS:
                serialPortWrapperMock.Received().RtsEnable = true;
                serialPortWrapperMock.Received().RtsEnable = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pin), pin, null);
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task PinCanBeToggledForDuration(ControlPin pin,
        PinState initialState,
        [CombinatorialMemberData(nameof(DurationTestData))]
        TimeSpan toggleDuration)
    {
        // Arrange
        var expectedFinalState = initialState switch
        {
            PinState.Disabled => PinState.Disabled,
            PinState.Enabled => PinState.Enabled,
            _ => throw new ArgumentOutOfRangeException(nameof(initialState), initialState, null)
        };

        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);
        var controlPinManager = new SerialControlPinManager(pin, serialPortWrapperMock, delayMock);

        if (initialState == PinState.Enabled)
        {
            await controlPinManager.EnablePin();
        }
        else
        {
            await controlPinManager.DisablePin();
        }

        // Act
        await controlPinManager.TogglePinFor(toggleDuration);

        // Assert
        controlPinManager.State.Should().Be(expectedFinalState);

        switch (pin)
        {
            case ControlPin.DTR:
                serialPortWrapperMock.Received().DtrEnable = true;
                serialPortWrapperMock.Received().DtrEnable = false;
                break;
            case ControlPin.RTS:
                serialPortWrapperMock.Received().RtsEnable = true;
                serialPortWrapperMock.Received().RtsEnable = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pin), pin, null);
        }

        await delayMock.Received().Delay(Arg.Is<TimeSpan>(duration => duration >= Timings.MinimumSignalSwitchTime));
    }

    [Theory]
    [CombinatorialData]
    public async Task PinCanBeSignaled(ControlPin pin, PinState initialPinState)
    {
        // Arrange
        var expectedFinalState = initialPinState switch
        {
            PinState.Disabled => PinState.Disabled,
            PinState.Enabled => PinState.Enabled,
            _ => throw new ArgumentOutOfRangeException(nameof(initialPinState), initialPinState, null)
        };

        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);
        var controlPinManager = new SerialControlPinManager(pin, serialPortWrapperMock, delayMock);

        if (initialPinState == PinState.Enabled)
        {
            await controlPinManager.EnablePin();
        }
        else
        {
            await controlPinManager.DisablePin();
        }

        // Act
        await controlPinManager.SendSignal();

        // Assert
        controlPinManager.State.Should().Be(expectedFinalState);

        switch (pin)
        {
            case ControlPin.DTR:
                serialPortWrapperMock.Received().DtrEnable = true;
                serialPortWrapperMock.Received().DtrEnable = false;
                break;
            case ControlPin.RTS:
                serialPortWrapperMock.Received().RtsEnable = true;
                serialPortWrapperMock.Received().RtsEnable = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pin), pin, null);
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task PinCanBeSignaledForDuration(ControlPin pin,
        PinState initialPinState,
        [CombinatorialMemberData(nameof(DurationTestData))]
        TimeSpan signalDuration)
    {
        // Arrange
        var expectedFinalState = initialPinState switch
        {
            PinState.Disabled => PinState.Disabled,
            PinState.Enabled => PinState.Enabled,
            _ => throw new ArgumentOutOfRangeException(nameof(initialPinState), initialPinState, null)
        };

        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);
        var controlPinManager = new SerialControlPinManager(pin, serialPortWrapperMock, delayMock);

        if (initialPinState == PinState.Enabled)
        {
            await controlPinManager.EnablePin();
        }
        else
        {
            await controlPinManager.DisablePin();
        }

        // Act
        await controlPinManager.SendSignalFor(signalDuration);

        // Assert
        controlPinManager.State.Should().Be(expectedFinalState);

        switch (pin)
        {
            case ControlPin.DTR:
                serialPortWrapperMock.Received().DtrEnable = true;
                serialPortWrapperMock.Received().DtrEnable = false;
                break;
            case ControlPin.RTS:
                serialPortWrapperMock.Received().RtsEnable = true;
                serialPortWrapperMock.Received().RtsEnable = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pin), pin, null);
        }

        await delayMock.Received().Delay(Arg.Is<TimeSpan>(duration => duration >= Timings.MinimumSignalSwitchTime));
    }
}