using System;
using System.Threading.Tasks;

namespace NSerial.Core;

/// <inheritdoc />
public class TaskDelay : IDelay
{
    /// <inheritdoc />
    public Task Delay(TimeSpan duration)
    {
        return Task.Delay(duration);
    }
}