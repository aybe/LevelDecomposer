LevelDecomposer
===============

*Converts a level bitmap to a tile sheet / level data and vice-versa.*

**Binaries**

Pre-built binaries are available [here](https://github.com/aybe/LevelDecomposer/releases).


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

**Notes**

- debug builds have command line arguments for testing against 'test\level.png' file

- whenever 'LevelDecomposer' is built the 'test' folder gets cleaned

- both command-line utilities are self-contained thanks to Fody, i.e. you only need the EXE

- the tile index uses a 32-bit integer, that should be enough for most purposes :D

**Limitations**

- make sure that the tile sheet image is in the same folder than the JSON level data

**TODO**

- allow the user to specify tile sheet width
