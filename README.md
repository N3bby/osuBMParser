# osuBMParser

This project is something I made as a preparation for making an osu! clone (educational purposes).

The osuBMParser project contains all that is needed for beatmap parsing.

osuBMParserTest is a project I created to test out my code. It shows all the properties of a Beatmap object in a Windows Forms Property Grid

## How to use?

Using this class library is pretty easy.
Build the assembly from the osuBMParser project or download it [here](https://github.com/Razacx/osuBMParser/raw/master/osuBMParser/bin/Debug/osuBMParser.dll).

To parse a beatmap (.osu) file, use the following code:

```Beatmap beatmap = new Beatmap(string path);```

The Beatmap object will contain all properties found on this webpage (including lists with HitObjects, TimingPoints, etc.):
https://osu.ppy.sh/wiki/Osu_(file_format)

## To-do List
* Calculate time offset for HitSliderSegments
* Add classes for Brezier, Catmull and Linear curves for calculating positions that are in between HitSliderSegments.
* Figure out how the hitSounds work
* Figure out how additions and edgeAdditions work
* Add code for writing Beatmap object back to a file
* Parse the "Event" section of beatmap files.
* Figure out default values for some propery settings (for example AR)