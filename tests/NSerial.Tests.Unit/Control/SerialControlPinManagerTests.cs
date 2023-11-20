using FluentAssertions;
using NSerial.Constants;
using NSerial.Control;
using NSerial.Core;
using NSerial.Model;
using NSubstitute;

namespace NSerial.Tests.Unit.Control;

public class SerialControlPinManagerTests
{
    public static TheoryData<TimeSpan> DurationData = new()
    {
        TimeSpan.Zero,
        TimeSpan.FromMilliseconds(100),
        Timings.MinimumSignalSwitchTime,
        Timings.MinimumSignalSwitchTime + TimeSpan.FromMilliseconds(200)
    };

    [Fact]
    public void DtrPinCanBeEnabled()
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);
        var controlPinManager = new SerialControlPinManager(ControlPin.DTR, serialPortWrapperMock, delayMock);

        // Act
        controlPinManager.EnablePin();

        // Assert
        controlPinManager.State.Should().Be(PinState.Enabled);
        
        serialPortWrapperMock.Received().DtrEnable = true;
    }

    [Theory]
    [MemberData(nameof(DurationData))]
    public async Task DtrPinCanBeEnabledForDuration(TimeSpan enableDuration)
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();

        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);

        var controlPinManager = new SerialControlPinManager(ControlPin.DTR, serialPortWrapperMock, delayMock);

        // Act
        await controlPinManager.EnablePinFor(enableDuration);

        // Assert
        controlPinManager.State.Should().Be(PinState.Disabled);
        
        serialPortWrapperMock.Received().DtrEnable = true;
        serialPortWrapperMock.Received().DtrEnable = false;
        await delayMock.Received().Delay(Arg.Is<TimeSpan>(duration => duration >= Timings.MinimumSignalSwitchTime));
    }

    [Fact]
    public void RtsPinCanBeEnabled()
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);
        var controlPinManager = new SerialControlPinManager(ControlPin.RTS, serialPortWrapperMock, delayMock);

        // Act
        controlPinManager.EnablePin();

        // Assert
        controlPinManager.State.Should().Be(PinState.Enabled);
        
        serialPortWrapperMock.Received().RtsEnable = true;
    }

    [Theory]
    [MemberData(nameof(DurationData))]
    public async Task RtsPinCanBeEnabledForDuration(TimeSpan enableDuration)
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();

        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);

        var controlPinManager = new SerialControlPinManager(ControlPin.RTS, serialPortWrapperMock, delayMock);

        // Act
        await controlPinManager.EnablePinFor(enableDuration);

        // Assert
        controlPinManager.State.Should().Be(PinState.Disabled);
        
        serialPortWrapperMock.Received().RtsEnable = true;
        serialPortWrapperMock.Received().RtsEnable = false;
        await delayMock.Received().Delay(Arg.Is<TimeSpan>(duration => duration >= Timings.MinimumSignalSwitchTime));
    }

    [Fact]
    public void DtrPinCanBeDisabled()
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);
        var controlPinManager = new SerialControlPinManager(ControlPin.DTR, serialPortWrapperMock, delayMock);

        // Act
        controlPinManager.DisablePin();

        // Assert
        controlPinManager.State.Should().Be(PinState.Disabled);
        
        serialPortWrapperMock.Received().DtrEnable = false;
    }

    [Theory]
    [MemberData(nameof(DurationData))]
    public async Task DtrPinCanBeDisabledForDuration(TimeSpan disableDuration)
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();

        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);

        var controlPinManager = new SerialControlPinManager(ControlPin.DTR, serialPortWrapperMock, delayMock);

        // Act
        await controlPinManager.DisablePinFor(disableDuration);

        // Assert
        controlPinManager.State.Should().Be(PinState.Enabled);
        
        serialPortWrapperMock.Received().DtrEnable = false;
        serialPortWrapperMock.Received().DtrEnable = true;
        await delayMock.Received().Delay(Arg.Is<TimeSpan>(duration => duration >= Timings.MinimumSignalSwitchTime));
    }

    [Fact]
    public void RtsPinCanBeDisabled()
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);
        var controlPinManager = new SerialControlPinManager(ControlPin.RTS, serialPortWrapperMock, delayMock);

        // Act
        controlPinManager.DisablePin();

        // Assert
        controlPinManager.State.Should().Be(PinState.Disabled);
        serialPortWrapperMock.Received().RtsEnable = false;
    }

    [Theory]
    [MemberData(nameof(DurationData))]
    public async Task RtsPinCanBeDisabledForDuration(TimeSpan disableDuration)
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();

        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);

        var controlPinManager = new SerialControlPinManager(ControlPin.RTS, serialPortWrapperMock, delayMock);

        // Act
        await controlPinManager.DisablePinFor(disableDuration);

        // Assert
        controlPinManager.State.Should().Be(PinState.Enabled);
        
        serialPortWrapperMock.Received().RtsEnable = false;
        serialPortWrapperMock.Received().RtsEnable = true;
        await delayMock.Received().Delay(Arg.Is<TimeSpan>(duration => duration >= Timings.MinimumSignalSwitchTime));
    }

    [Theory]
    [InlineData(PinState.Disabled)]
    [InlineData(PinState.Enabled)]
    public void DtrPinCanBeToggled(PinState initialState)
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
        var controlPinManager = new SerialControlPinManager(ControlPin.DTR, serialPortWrapperMock, delayMock);

        if (initialState == PinState.Enabled)
        {
            controlPinManager.EnablePin();
        }
        else
        {
            controlPinManager.DisablePin();
        }

        // Act
        controlPinManager.TogglePin();

        // Assert
        controlPinManager.State.Should().Be(expectedFinalState);
        
        serialPortWrapperMock.Received().DtrEnable = true;
        serialPortWrapperMock.Received().DtrEnable = false;
    }

    [Theory]
    [MemberData(nameof(DurationData))]
    public async Task DtrPinCanBeToggledForDuration(TimeSpan toogleDuration)
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();

        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);

        var controlPinManager = new SerialControlPinManager(ControlPin.DTR, serialPortWrapperMock, delayMock);

        // Act
        await controlPinManager.TogglePinFor(toogleDuration);

        // Assert
        controlPinManager.State.Should().Be(PinState.Disabled);
        
        serialPortWrapperMock.Received().DtrEnable = true;
        serialPortWrapperMock.Received().DtrEnable = false;
        await delayMock.Received().Delay(Arg.Is<TimeSpan>(duration => duration >= Timings.MinimumSignalSwitchTime));
    }

    [Theory]
    [InlineData(PinState.Disabled)]
    [InlineData(PinState.Enabled)]
    public void RtsPinCanBeToggled(PinState initialState)
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
        var controlPinManager = new SerialControlPinManager(ControlPin.RTS, serialPortWrapperMock, delayMock);

        if (initialState == PinState.Enabled)
        {
            controlPinManager.EnablePin();
        }
        else
        {
            controlPinManager.DisablePin();
        }

        // Act
        controlPinManager.TogglePin();

        // Assert
        controlPinManager.State.Should().Be(expectedFinalState);
        serialPortWrapperMock.Received().RtsEnable = true;
        serialPortWrapperMock.Received().RtsEnable = false;
    }

    [Theory]
    [MemberData(nameof(DurationData))]
    public async Task RtsPinCanBeToggledForDuration(TimeSpan toggleDuration)
    {
        // Arrange
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();

        var delayMock = Substitute.For<IDelay>();
        delayMock.Delay(Arg.Any<TimeSpan>()).ReturnsForAnyArgs(Task.CompletedTask);

        var controlPinManager = new SerialControlPinManager(ControlPin.RTS, serialPortWrapperMock, delayMock);

        // Act
        await controlPinManager.TogglePinFor(toggleDuration);

        // Assert
        controlPinManager.State.Should().Be(PinState.Disabled);
        
        serialPortWrapperMock.Received().RtsEnable = true;
        serialPortWrapperMock.Received().RtsEnable = false;
        await delayMock.Received().Delay(Arg.Is<TimeSpan>(duration => duration >= Timings.MinimumSignalSwitchTime));
    }
}