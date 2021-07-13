# Crowbar X

This is a modified version of crowbar which was converted from VB(Visual Basic) to C#. (only Decompilation Module)

For those who doesnt know what Crowbar is, read here > https://steamcommunity.com/groups/CrowbarTool


### ChangeLog

**0.2**
- Removed Some unused files
- Refactored `FileManager.cs` due to Path and SMD files problems
- Added "-v" for verbose/debugging printing
- Changed some `if (true)` to `check for verbose command` (Generate the Log Files)

### Contents

* CLI application
* Cross-Platform


### Why this?

I did this, because there's a blender plugin called io_vmf which needs crowbar to decompile MDL files.
And it doesnt work in linux because there's no Crowbar to linux (altought you can use with wine) but it gets stuck when importing map in blender.

### How to Use?

commands between `[]` are optional arguments 

* Linux
./crowbar "input/path.mdl" "output/path/" [-v]

* Windows
crowbar.exe "input/path.mdl" "output/path/" [-v]

simples as that :D (and yeah the release works for all OSes)

### Requirements

.Net Core 3.1


### References

Zeq's Crowbar - https://github.com/ZeqMacaw/Crowbar

Lasa's VMF import for blender - https://github.com/lasa01/io_import_vmf

VB2CS - https://github.com/icsharpcode/CodeConverter/

