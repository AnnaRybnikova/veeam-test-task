# veeam-test-task

### Task:
Please implement a program that synchronizes two folders: source and
replica. The program should maintain a full, identical copy of source
folder at replica folder. Solve the test task by writing a program in C#.

- Synchronization must be one-way: after the synchronization content of the
replica folder should be modified to exactly match content of the source
folder;
- Synchronization should be performed periodically;
- File creation/copying/removal operations should be logged to a file and to the
console output;
- Folder paths, synchronization interval and log file path should be provided
using the command line arguments;
- It is undesirable to use third-party libraries that implement folder
synchronization;
- It is allowed (and recommended) to use external libraries implementing other
well-known algorithms. For example, there is no point in implementing yet
another function that calculates MD5 if you need it for the task

### Implementation

Console utility was created using C# to synchronize 2 folders. The algorithm is pretty straightforward. As a first step destination folder is cleaned up, and than, all the files and filders copied recursively. Also files are copied in parallel to speed up programm execution.
Libraries used:

- `CommandLineParser` - to work with CLI arguments
- `Serilog` - to work with logs
- `XUnit` - to test the application

### Usage example:

In order to test the synchronization next command could be executed:

```
> dotnet run -- -f <from-folder> -t <to-folder> -i <interval>
```

arguments:

- From folder - `f` or `from`. Could be relative or absolute path.
- To folder - `t`, `to`. Could be relative or absolute path.
- Interval - `i`, `interval`. Optional argument in timespan format (Ex: `00:00:30` - 30 second, `00:20:00` - 20 minutes)
