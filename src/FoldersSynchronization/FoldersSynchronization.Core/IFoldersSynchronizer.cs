// <copyright file="IFoldersSynchronizer.cs" person="Anna Rybnikova">
// Copyright (c) Rybnikova. All rights reserved.
// </copyright>

namespace FoldersSynchronization.Core;

public interface IFoldersSynchronizer
{
    Task SynchronizeAsync(string source, string destination);
}