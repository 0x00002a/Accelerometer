# Accelerometer 

## Introduction 

This is a pretty simple mod which adds a readout of your current acceleration to your HUD. 
It is designed for use along another of my mods, [Deadly Acceleration](https://steamcommunity.com/workshop/filedetails/?id=2422178213), 
but can be used standalone. 

## Features

- Displays your angular + linear acceleration as text alongside the vanilla HUD
- Metric (m/s²) or GForce (g) unit display
- That's it

### Multiplayer

Works in hosted and dedicated multiplayer.

## Configuration

These options can (and really should) be edited through the TextHudAPI menu. They persist across all worlds and servers and are tied to 
each player.

- `Position`: Has `X` and `Y` values. These govern the position on the screen of the display. -0.64, -0.64 is the default
- `Unit`: Can be _GForce_ or _Metric_. Metric is default

## Reuse/Licensing

All of my code in this mod is licensed under the GNU GPLv3. Some parts of this code are not mine 
and I cannot and do not relicense them. Code that is not mine can be found in the following files:

- `Log.cs` (author Digi)
- Anything in `TextHudAPI` (TextHudAPI mod)

Please contact the respective authors for redistrubtion information about rights for those parts of the mod. In regards to my code:

This mod is GPLv3. That means you can reupload it or any mod that contains it _as long as_ you:

- Keep all existing license notices intact
- Credit me
- List your changes (easiest way is with git and github repo)
- Make _all_ the source code of the relevent mod avaliable freely and publically with no restrictions placed on its access
- Make your mod GPLv3 as well
- Give me your first born child

(ok that last one isn't actually legally binding)

If in doubt, ask me in comments or the Keen discord (\@Natomic). 
Full license is avaliable [here](https://github.com/0x00002a/Accelerometer/blob/74aad99d203f819d7097298f451a4efc131f2dea/LICENSE). I reserve the right to ask 
for your mod to be yeeted if you have reused my mod without obeying the license.

## Source

Source code for this mod can be found [here](https://github.com/0x00002a/Accelerometer)