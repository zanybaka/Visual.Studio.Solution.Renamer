Visual Studio Solution Renamer
===

Allows to rename a solution, its projects and namespaces.

Get started
---

```shell
dotnet tool install -g vsrenamer_gui
vsrenamer_gui.exe
```

Under the hood
---

Here is the list of actions to be done during the process of renaming
- Rename .sln file
- Rename .csproj files
- Rename parent folder of the projects
- Update their relative paths in the .sln file
- Set proper AssemblyName and RootNamespace in the .csproj files
- Replace text in files. By default, *.cs *.xaml *.xml *.json *.asax *.cshtml *.config *.js

Limitations
---

- A single solution file only (.sln)
- A single project (.csproj) per folder
- C# projects only
- No source version control history support

Be aware
---

- No backup feature
- No roll-back feature
- No proper symlinks support
- Tested with
    - Microsoft Visual Studio Solution File, Format Version 12.00
    - Visual Studio Version 16 (2019)
- It's strongly recommended to perform renaming with the following two steps:
    1. Replace file content, review the changes and commit (svn, git, etc.)
    2. Rename files and folders

References
---

* [Serilog](https://serilog.net/) library
    * [Apache 2.0 License](https://www.apache.org/licenses/LICENSE-2.0)
* [Command Line Parser Library for CLR and NetStandard](https://github.com/commandlineparser/commandline)
    * [MIT License](https://github.com/zzzprojects/html-agility-pack/blob/master/LICENSE)
* [Microsoft.Build](https://github.com/dotnet/msbuild) library
    * [MIT License](https://github.com/zzzprojects/html-agility-pack/blob/master/LICENSE)
