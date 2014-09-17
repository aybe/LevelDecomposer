LevelDecomposer
===============

*Converts a level bitmap to a tile sheet / level data and vice-versa.*

**Binaries**

Pre-built binaries are available [here](https://github.com/aybe/LevelDecomposer/releases).

**NuGet package**

Get it [here](https://www.nuget.org/packages/LevelDecomposer/).

**Example**

An input level bitmap :

![](https://raw.githubusercontent.com/aybe/LevelDecomposer/master/sample/example_recompose.png)

Gets decomposed to a tile sheet and JSON level data :

![](https://raw.githubusercontent.com/aybe/LevelDecomposer/master/sample/example_decompose.png)

```
{
  "SheetName": "decompose.png",
  "SheetWidth": 256,
  "SheetHeight": 112,
  "LevelWidth": 40,
  "LevelHeight": 24,
  "TileWidth": 16,
  "TileHeight": 16,
  "Tiles": [
    0,
    1,
    1,
    1,
    1,
    1,
    1,
    1,
    // etc ...
```

The other way around is also possible.

**Notes & limitations**

- the tile index uses a 32-bit integer, that should be enough for most purposes :D

- make sure that the tile sheet image is in the same folder than the JSON level data
