<h1> 
CoC# <a href="https://travis-ci.org/FICTURE7/CoCSharp"><img src="https://travis-ci.org/FICTURE7/CoCSharp.svg?branch=master" alt="Build Status"></a>
</h1>

Clash of Clans library written in C# to handle networking, csv files and more to come. 
It was written based off of [Clash of Clans Documentation Project](https://github.com/clanner/cocdp/)
and decompiled source code of [Ultrapowa Clash Server](https://github.com/Ultrapowa/ucs) 
which is now open source.

## What can it do?
* CoCSharp.Data: CSV file structures and other data structures.
  * Includes classes for reading CSV files.
  * Includes classes for all CSV file data. Such as: BuildingData, TrapData, ObstacleData etc...
* CoCSharp.Logging: *(Needs to be improved)*
  * Includes classes to log and dump packet.
* CoCSharp.Logic: Village object strutures. *(Needs to be improved)*
  * Includes classes for village objects: Village, Trap, Obstacle, Building etc...
* CoCSharp.Networking: Clash of Clans network protocol implementaion.
  * Includes classes for reading and writing packets.
  * Includes some classes containing packet definition. Such as: AllianceWarLogPacket, OwnHomeDataPacket etc...
  
## Usage
CoCSharp is trying to implement most the Clash of Clans features and also trying be easy as possible to use.
Here are some examples. 

### Networking
CoCSharp.Networking implements the `SocketAsyncEventArgs` async model.

Example to write and read packets async.
```c#
// creates a new Socket
var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

// connects to the official clash of clans server
socket.Connect("gamea.clashofclans.com", 9339);

// creates a new NetworkManagerAsync
// the PacketReceived event is raised when a packet is recieved
// here we are using it to write the packet name and packet id to the console
var networkManagerAsync = new NetworkManagerAsync(socket);
networkManagerAsync.PacketReceived += (object sender, PacketReceivedEventArgs e) =>
{
    if (e.Exception != null)
    {
        // e.Exception != null if there was no exception while 
        // receiving the packet
        Console.WriteLine("Recieved {0}:{1}", e.Packet.GetType().Name, e.Packet.ID);
    }
    else
    {
        // else there was an exception while
        // receiving the packet
        Console.WriteLine("Failed to receive packet: {0}", e.Exception);
    }
};


// sends a LoginRequestPacket to the server
// if the server accepts the login request, it will
// send an UpdateKeyPacket which contains the server random
// which will then be scrambled and the RC4 streams updated with.
// the UpdateKeyPacket is handled automatically by the NetworkManagerAsync.
// there is no need to update the keys.
networkManagerAsync.SendPacket(new LoginRequestPacket()
{
    UserID = 0,
    UserToken = null,
    ClientMajorVersion = 7,
    ClientContentVersion = 12,
    ClientMinorVersion = 200,
    FingerprintHash = "7af2ba412c1716cffe3949f1dcffcea6822560f2",
    OpenUDID = "563a6f060d8624db",
    MacAddress = null,
    DeviceModel = "GT-I9300",
    LocaleKey = 2000000,
    Language = "en",
    AdvertisingGUID = "",
    OSVersion = "4.0.4",
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
// creates a new instance of the PacketLogger that will log
// packets to the console and to the file "packets.log"
var packetLogger = new PacketLogger("packets.log")
{
    LogConsole = true
    // set to false if you dont want logs in the console
    // it is true by default, you could do that
    // var packetLogger = PacketLogger("log_filename.log")
    // simply
};

// creates a new instance of the ExceptionLogger that will log
// exceptions to the console and to the file "exceptions.log"
var exceptionLogger = new ExceptionLogger("exceptions.log");

var networkManagerAsync = new NetworkManagerAsync(socket);
networkManagerAsync.PacketReceived += (object sender, PacketReceivedEventArgs e) =>
{
    if (e.Exception != null)
    {
        packetLogger.LogPacket(e.Packet, PacketDirection.Client);
    }
    else
    {
        exceptionLogger.LogException(e.Exception);
    }
};
```
and you should see logs in your console.


### CSV Files
Example to read a compressed `buildings.csv` file.
```c#
// loads the .csv file
var table = new CsvTable("buildings.csv", true);

// deserializes the CsvTable into an object array of the specified type.
var buildingsData = CsvSerializer.Deserialize(table, typeof(BuildingData));
```

## Compiling
The simplest way to compile CoCSharp is to use Visual Studio and pressing `F6` to build the solution or you could 
use the latest version of mono to compile CoCSharp.

Run the following commands to build CoCSharp with mono.

Clone the project using git.
```
git clone --branch=master git://github.com/FICTURE7/CoCSharp.git
```
Then you will have to restore the nuget packages, to get the dependencies.
```
nuget restore
```
In the root directory run or where `CoCSharp.sln` is located.
```
xbuild
```
Then you should have the compiled binaries in `/bin`, and you should be able to run the binaries using the latest 
version of mono.

```
mono CoCSharp.Proxy/bin/Debug/CoCSharp.Proxy.exe
```

If you would like to check the version of mono you're using
```
mono --version
```

## Contributing
Just create a fork and make changes to it, like adding features from Clash of Clans and fixing bugs. 
Follow the code style else I won't merge your pull request. Make your pull requests focused and readable. 
You can also contribute by creating issues and reporting bugs, giving ideas for enhancement.

Am not any merging pull requests like a clan search API which completely drifts away from the goal of this project. 
If you want to make a clan search API, use CoCSharp as a library and go ahead. :]

### Thanks
Massive thank you to those [guys](https://github.com/FICTURE7/CoCSharp/blob/master/CONTRIBUTORS) who helped with the project! :]

<h2>Projects Using CoC#</h2>
* [CoCSharp.Server](https://github.com/FICTURE7/CoCSharp/tree/master/CoCSharp.Server): *(Work in progress)*
  * Server that can't do much at the moment.
* [CoCSharp.Client](https://github.com/FICTURE7/CoCSharp/tree/master/CoCSharp.Client):
  * Simple client that can read chat message and send keep alive requests.
* [CoCSharp.Proxy](https://github.com/FICTURE7/CoCSharp/tree/master/CoCSharp.Proxy):
  * Proxy that logs parsed and raw decrypted/encrypted packets.

## Licensing
CoCSharp is licensed under the permissive [MIT License](http://mit-license.org/).

CoCSharp is not officially affiliated with Supercell.