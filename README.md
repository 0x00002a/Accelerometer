# Accelerometer 

## Introduction 

This is a pretty simple mod which adds a readout of your current acceleration to your HUD. 
It is designed for use along another of my mods, [Deadly Acceleration](https://steamcommunity.com/workshop/filedetails/?id=2422178213), 
but can be used standalone. 

## Features

- Displays your angular + linear acceleration as text alongside the vanilla HUD
- That's it

### Multiplayer

Untested. May or may not work dependendent on whether `MyAPIGateway.Session.Player` is `null` clientside. 
Worst case, it simply will not run. I will be testing this in the future.

### Planned

Stuff I would like to add to this mod if I have the time/can work out how:

- A nice graphic for acceleration would be nice, rather than just a number. Similar to the vanilla velocity display

## Configuration

None for now

## Dependencies

- TextHudAPI

## Reuse/Licensing

All of my code in this mod is licensed under the GNU GPLv3. Some parts of this code are not mine 
and I cannot and do not relicense them. Code that is not mine can be found in the following files:

- `Log.cs` (author Digi)
- Anything in `TextHudAPI` (TextHudAPI mod)

Please contact the respective authors for redistrubtion information about rights for those parts of the mod. In regards to my code, 
read the license:

```
    Accelerometer Space Engineers mod
    Copyright (C) 2021 Natasha England-Elbro

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
```

