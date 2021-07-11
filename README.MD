# Crowbar X

This is a modified version of crowbar which was converted from VB(Visual Basic) to C#.

### Contents

* CLI application
* X-Platform


### Why this?

I did this, because there's a blender plugin called io_vmf which needs crowbar to decompile MDL files.
And it doesnt work in linux because there's no Crowbar to linux (altought you can use with wine) but it gets stuck when importing map in blender.

### How to Use?

* Linux
./crowbar "input/path.mdl" "output/path/"

* Windows
crowbar.exe "input/path.mdl" "output/path/"

simples as that :D (and yeah the release works for all OSes)

### Requirements

.Net Core 3.1