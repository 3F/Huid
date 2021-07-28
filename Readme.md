# [Huid](https://github.com/3F/Huid)

High-speed a [FNV-1a-**128**](https://github.com/3F/Fnv1a128) (\<-[LX4Cnh](https://github.com/3F/LX4Cnh)) hash-based **UUID** implementations.

```r
Copyright (c) 2021  Denis Kuzmin <x-3F@outlook.com> github/3F
```

[ [ <sub>@</sub> ☕ ] ](https://3F.github.io/Donation/) &nbsp;&nbsp; [![License](https://img.shields.io/badge/License-MIT-74A5C2.svg)](https://github.com/3F/Huid/blob/master/License.txt)


## .NET implementation

[![Build status](https://ci.appveyor.com/api/projects/status/l11wcuplkqvtwu40/branch/master?svg=true)](https://ci.appveyor.com/project/3Fs/huid/branch/master)
[![NuGet package](https://img.shields.io/nuget/v/Huid.svg)](https://www.nuget.org/packages/Huid/) 
[![Tests](https://img.shields.io/appveyor/tests/3Fs/huid/master.svg)](https://ci.appveyor.com/project/3Fs/huid/build/tests)

[![Build history](https://buildstats.info/appveyor/chart/3Fs/huid?buildCount=15&includeBuildsFromPullRequest=true&showStats=true)](https://ci.appveyor.com/project/3Fs/huid/history)

```csharp
Huid.NewGuid
(
    "LodgeX4CorrNoHigh (LX4Cnh) algorithm of the high-speed multiplications of 128-bit numbers"
)
```

[![](/img/benchmark.png)](https://twitter.com/github3F/status/1419045735807467520)

[![](/img/benchmark.inf.png)](https://twitter.com/github3F/status/1419045735807467520)

*(1 ns = 0.000000001 sec)*

**\+** ✔ Compatible with .NET [System.Guid](https://docs.microsoft.com/en-us/dotnet/api/system.guid).

**\+** ✔ Free and Open. MIT License. *Fork! Star! Contribute! Share! Enjoy!*
