<h1> 
CoC# <a href="https://travis-ci.org/FICTURE7/CoCSharp"><img src="https://travis-ci.org/FICTURE7/CoCSharp.svg?branch=master" alt="Build Status"></a>
</h1>

Clash of Clans library written in C# to handle networking, csv files and more to come. It was written based off of the [Clash of Clans Documentation Project](https://github.com/clanner/cocdp/).

## What can it do?
* CoCSharp.Data: CSV file structures and other data structures.
  * Includes classes for reading CSV files.
  * Includes classes for managing CSV files and downloading them.
  * Includes classes for all CSV file data. Such as: BuildingData, TrapData, ObstacleData etc...
* CoCSharp.Logging: *(Needs to be improved)*
  * Includes classes to log and dump packet.
* CoCSharp.Logic: Village object strutures. *(Needs to be improved)*
  * Includes classes for village objects: Village, Trap, Obstacle, Building etc...
  
## Usage
CoCSharp is trying to implement most the Clash of Clans features and also trying be easy as possible to use.<br>
This info is currently out of date, will update soon. :]

### Networking
CoCSharp.Networking implements the `SocketAsyncEventArgs` async model.

Example to write and read packets async.
```c#
// creates a new Socket
var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
// connects to the official clash of clans server
socket.Connect("gamea.clashofclans.com", 9339);
// creates a new NetworkManagerAsync
// the PacketReceivedHandler delegate is called when a packet is recieved
// here we are using it to write the packet type and packet id to the console
var networkManagerAsync = new NetworkManagerAsync(socket, 
(SocketAsyncEventArgs args, IPacket packet) =>
{
    Console.WriteLine("Recieved {0}:{1}", packet.GetType().Name, packet.ID);
}, null /*use this delegate to handle errors*/);
// sends a LoginRequestPacket to the server
networkManagerAsync.WritePacket(new LoginRequestPacket()
{
    UserID = 0, 
    UserToken = null,
    ClientMajorVersion = 7,
    ClientContentVersion = 0,
    ClientMinorVersion = 156,
    FingerprintHash = "ae9b056807ac8bfa58a3e879b1f1601ff17d1df5",
    OpenUDID = "563a6f060d8624db",
    MacAddress = null,
    DeviceModel = "GT-I9300",
    LocaleKey = 2000000,
    Language = "en",
    AdvertisingGUID = "",
    OsVersion = "4.0.4",
    IsAdvertisingTrackingEnabled = false,
    AndroidDeviceID = "563a6f060d8624db",
    FacebookDistributionID = "",
    VendorGUID = "",
    Seed = 1329685020 // should be random
});
```

### Logging
If you would like to log packets in a more fancy way use this instead
```c#
// creates a new PacketLogger that will log
// to the console and to the file "log_filename.log"
var packetLogger = new PacketLogger("log_filename.log")
{
    LogConsole = true
    // set to false if you dont want logs in the console
    // it is true by default, you could do that
    // var packetLogger = PacketLogger("log_filename.log")
    // simply
};
var networkManager = new NetworkManagerAsync(socket, 
(SocketAsyncEventArgs args, IPacket packet) =>
{
    packetLogger.LogPacket(packet, PacketDirection.Client);
}, null /*use this delegate to handle errors*/);
```
and you should see logs in your console.


### CSV Tables
Example to read a compressed `buildings.csv` file.
```c#
// loads the .csv file
var table = new CsvTable("buildings.csv", true);
// deserializes the CsvTable into an object array of the specified type.
var buildingsData = CsvSerializer.Deserialize(table, typeof(BuildingData));
```

## Compiling
The simplest way to compile CoCSharp is to use Visual Studio and pressing `F6` to build the solution or you could use the latest version of mono to compile CoCSharp.

Run the following commands to build CoCSharp with mono.

Use a git clone
```
git clone --branch=master git://github.com/FICTURE7/CoCSharp.git
```
Then you will have to restore the nuget packages.
```
nuget restore
```
In the root directory run or where `CoCSharp.sln` is located.
```
xbuild
```
Then you should have the compiled binaries in `/bin`, and you should be able to run the binaries using the latest version of mono.

```
mono CoCSharp.Proxy/bin/Debug/CoCSharp.Proxy.exe
```

## Contributing
Just create a fork and make changes to it, like adding features from Clash of Clans, refactoring and fixing bugs. Try to follow the code style. Make your pull requests focused and readable. You can also contribute by creating issues and reporting bugs, giving ideas for enhancement.

<h2>Projects Using CoC#</h2>
* [CoCSharp.Server](https://github.com/FICTURE7/CoCSharp/tree/master/CoCSharp.Server): *(Work in progress)*
  * Server that can't do much at the moment.
* [CoCSharp.Client](https://github.com/FICTURE7/CoCSharp/tree/master/CoCSharp.Client):
  * Simple client that can read chat message and send keep alive requests.
* [CoCSharp.Proxy](https://github.com/FICTURE7/CoCSharp/tree/master/CoCSharp.Proxy):
  * Proxy that logs parsed and raw decrypted/encrypted packets.

## Licensing
CoCSharp is licensed under the permissive [MIT License](http://mit-license.org/).
