# CoCSharp
---
Clash of Clans library written in C# to handle networking and csv files(more to come?). It was written based off of the [Clash of Clans Documentation Project](https://github.com/clanner/cocdp/).

## What it can do
---
* CoCSharp.Networking: Networking and protocol support.
  * Defines some packets of the CoC networking protocol.
  * Includes classes to read and write CoC packets.
  * Includes encryption support for 7.x versions.
* CoCSharp.Databases: CSV files support. *(Needs to be refactored)*
  * Includes class for reading CSV files.
  * Includes class for managing CSV files and downloading thm
* CoCSharp.Logic: Village object strutures. *(Needs to be refactored)*
  * Includes class for village objects: Village, Trap, Obstacle, Building etc...
  
## Projects using CoCSharp
---
* CoCSharp.Server: *(Planned)*
* CoCSharp.Proxy:
  * Proxy that logs parsed and raw decrypted packets
