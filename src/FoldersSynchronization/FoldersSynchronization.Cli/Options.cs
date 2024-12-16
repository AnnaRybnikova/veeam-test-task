// <copyright file="Options.cs" person="Anna Rybnikova">
// Copyright (c) Rybnikova. All rights reserved.
// </copyright>

using CommandLine;

namespace FoldersSynchronization.Cli;

public class Options
{
    [Option('f', "from", Required = true, HelpText = "The folder containing the source files.")]
    public string From { get; set; }
    
    [Option('t', "to", Required = true, HelpText = "The folder containing the destination files.")]
    public string To { get; set; }
    
    [Option('i', "interval", Required = false, HelpText = "The interval to run the job periodically.")]
    public TimeSpan? Interval { get; set; }
}