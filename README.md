# CoCSharp
Clash of Clans library written in C# to handle networking, csv files and more to come. It was written based off of the [Clash of Clans Documentation Project](https://github.com/clanner/cocdp/).

## What can it do?
* CoCSharp.Common: Common data between client and server.
  * Includes enum for resource type and building type.
* CoCSharp.Data:
  * Includes classes for reading CSV files.
  * Includes classes for managing CSV files and downloading them.
  * Includes classes for BuildingData.
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
Example to read a compressed .csv table and print all its fields. 
```c#
// loads the .csv file
var table = new CsvTable("buildings.csv", true);

// prints all column name
for (int i = 0; i < table.Columns.Count; i++)
{
    Console.Write(table.Columns[i].ColumnName.PadRight(30));
}
Console.WriteLine();

// print all row items
for (int i = 0; i < table.Rows.Count; i++)
{
  for (int k = 0; k < table.Rows[i].ItemArray.Length; k++)
    Console.Write(table.Rows[i].ItemArray[k].ToString().PadRight(50));
  Console.WriteLine();
}
```

## Projects using CoCSharp
* CoCSharp.Server: *(Planned)*
* CoCSharp.Proxy:
  * Proxy that logs parsed and raw decrypted packets

## Licensing
CoCSharp is licensed under the [MIT License](http://mit-license.org/).
