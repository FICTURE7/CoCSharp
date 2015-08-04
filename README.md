<h1> CoC# </h1>
Clash of Clans library written in C# to handle networking, csv files and more to come. It was written based off of the [Clash of Clans Documentation Project](https://github.com/clanner/cocdp/).

## What can it do?
* CoCSharp.Data:
  * Includes classes for reading CSV files.
  * Includes classes for managing CSV files and downloading them.
  * Includes classes for all CSV file data. Such as: BuildingData, TrapData, ObstacleData etc...
* CoCSharp.Logging:
  * Includes classes to log and dump packet.
* CoCSharp.Logic: Village object strutures. *(Needs to be refactored)*
  * Includes class for village objects: Village, Trap, Obstacle, Building etc...
* CoCSharp.Networking: Networking and protocol support. *(Needs to be more flexible)*
  * Defines some packets of the CoC networking protocol.
  * Includes classes to read and write CoC packets.
  * Includes encryption support for 7.x versions.
  
## Usage
CoCSharp is trying to implement most the Clash of Clans features and also trying be easy as possible to use.

### Networking
CoCSharp current networking system was designed mainly for a proxy and is not very flexible at the moment.

### CSV Tables
Example to read a compressed "buildings.csv" file.
```c#
// loads the .csv file
var table = new CsvTable("buildings.csv", true);
// deserializes the CsvTable into an object array of the specified type.
var buildingsData = CsvSerializer.Deserialize(table, typeof(BuildingData));
```

## Projects using CoCSharp
* [CoCSharp.Server](https://github.com/FICTURE7/CoCSharp/tree/messy-stuff/CoCSharp.Server): *(Work in progress)*
* [CoCSharp.Proxy](https://github.com/FICTURE7/CoCSharp/tree/messy-stuff/CoCSharp.Proxy):
  * Proxy that logs parsed and raw decrypted/encrypted packets.

## Licensing
CoCSharp is licensed under the [MIT License](http://mit-license.org/).
