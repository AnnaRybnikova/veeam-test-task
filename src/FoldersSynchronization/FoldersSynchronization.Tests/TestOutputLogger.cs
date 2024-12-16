// <copyright file="TestOutputLogger.cs" person="Anna Rybnikova">
// Copyright (c) Rybnikova. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace FoldersSynchronization.Tests;

public class TestOutputLogger<T>(ITestOutputHelper output) : ILogger<T>
{
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        output.WriteLine(formatter(state, exception));
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }
}