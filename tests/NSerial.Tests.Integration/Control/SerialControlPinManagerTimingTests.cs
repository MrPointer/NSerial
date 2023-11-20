using System.Diagnostics;
using FluentAssertions;
using NSerial.Constants;
using NSerial.Control;
using NSerial.Core;
using NSerial.Model;
using NSubstitute;

namespace NSerial.Tests.Integration.Control;

public class SerialControlPinManagerTimingTests
{
    private static readonly TimeSpan srm_LowerGracePeriod = TimeSpan.FromMilliseconds(10);
    private static readonly TimeSpan srm_UpperGracePeriod = TimeSpan.FromMilliseconds(100);

    public static TheoryData<TimeSpan> DurationData = new()
    {
        TimeSpan.Zero,
        TimeSpan.FromMilliseconds(100),
        Timings.MinimumSignalSwitchTime,
        Timings.MinimumSignalSwitchTime + TimeSpan.FromMilliseconds(200)
    };


    [Theory]
    [MemberData(nameof(DurationData))]
    public async Task DtrPinCanBeEnabledForDuration(TimeSpan enableDuration)
    {
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var controlPinManager = new SerialControlPinManager(ControlPin.DTR, serialPortWrapperMock, new TaskDelay());

        var stopwatch = Stopwatch.StartNew();
        await controlPinManager.EnablePinFor(enableDuration);
        stopwatch.Stop();

        stopwatch.Elapsed.Should().BeGreaterOrEqualTo(Timings.MinimumSignalSwitchTime - srm_LowerGracePeriod);

        if (enableDuration > Timings.MinimumSignalSwitchTime)
        {
            stopwatch.Elapsed.Should().BeLessThan(enableDuration + srm_UpperGracePeriod);
        }
    }

    [Theory]
    [MemberData(nameof(DurationData))]
    public async Task RtsPinCanBeEnabledForDuration(TimeSpan enableDuration)
    {
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var controlPinManager = new SerialControlPinManager(ControlPin.RTS, serialPortWrapperMock, new TaskDelay());

        var stopwatch = Stopwatch.StartNew();
        await controlPinManager.EnablePinFor(enableDuration);
        stopwatch.Stop();

        stopwatch.Elapsed.Should().BeGreaterOrEqualTo(Timings.MinimumSignalSwitchTime - srm_LowerGracePeriod);

        if (enableDuration > Timings.MinimumSignalSwitchTime)
        {
            stopwatch.Elapsed.Should().BeLessThan(enableDuration + srm_UpperGracePeriod);
        }
    }

    [Theory]
    [MemberData(nameof(DurationData))]
    public async Task DtrPinCanBeDisabledForDuration(TimeSpan disableDuration)
    {
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var controlPinManager = new SerialControlPinManager(ControlPin.DTR, serialPortWrapperMock, new TaskDelay());

        var stopwatch = Stopwatch.StartNew();
        await controlPinManager.DisablePinFor(disableDuration);
        stopwatch.Stop();

        stopwatch.Elapsed.Should().BeGreaterOrEqualTo(Timings.MinimumSignalSwitchTime - srm_LowerGracePeriod);

        if (disableDuration > Timings.MinimumSignalSwitchTime)
        {
            stopwatch.Elapsed.Should().BeLessThan(disableDuration + srm_UpperGracePeriod);
        }
    }

    [Theory]
    [MemberData(nameof(DurationData))]
    public async Task RtsPinCanBeDisabledForDuration(TimeSpan disableDuration)
    {
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var controlPinManager = new SerialControlPinManager(ControlPin.RTS, serialPortWrapperMock, new TaskDelay());

        var stopwatch = Stopwatch.StartNew();
        await controlPinManager.DisablePinFor(disableDuration);
        stopwatch.Stop();

        stopwatch.Elapsed.Should().BeGreaterOrEqualTo(Timings.MinimumSignalSwitchTime - srm_LowerGracePeriod);

        if (disableDuration > Timings.MinimumSignalSwitchTime)
        {
            stopwatch.Elapsed.Should().BeLessThan(disableDuration + srm_UpperGracePeriod);
        }
    }

    [Theory]
    [MemberData(nameof(DurationData))]
    public async Task DtrPinCanBeToggledForDuration(TimeSpan toggleDuration)
    {
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var controlPinManager = new SerialControlPinManager(ControlPin.DTR, serialPortWrapperMock, new TaskDelay());

        var stopwatch = Stopwatch.StartNew();
        await controlPinManager.TogglePinFor(toggleDuration);
        stopwatch.Stop();

        stopwatch.Elapsed.Should().BeGreaterOrEqualTo(Timings.MinimumSignalSwitchTime - srm_LowerGracePeriod);

        if (toggleDuration > Timings.MinimumSignalSwitchTime)
        {
            stopwatch.Elapsed.Should().BeLessThan(toggleDuration + srm_UpperGracePeriod);
        }
    }

    [Theory]
    [MemberData(nameof(DurationData))]
    public async Task RtsPinCanBeToggledForDuration(TimeSpan toggleDuration)
    {
        var serialPortWrapperMock = Substitute.For<ISerialPortWrapper>();
        var controlPinManager = new SerialControlPinManager(ControlPin.RTS, serialPortWrapperMock, new TaskDelay());

        var stopwatch = Stopwatch.StartNew();
        await controlPinManager.TogglePinFor(toggleDuration);
        stopwatch.Stop();

        stopwatch.Elapsed.Should().BeGreaterOrEqualTo(Timings.MinimumSignalSwitchTime - srm_LowerGracePeriod);

        if (toggleDuration > Timings.MinimumSignalSwitchTime)
        {
            stopwatch.Elapsed.Should().BeLessThan(toggleDuration + srm_UpperGracePeriod);
        }
    }
}