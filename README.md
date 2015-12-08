<h1> 
CoC# <a href="https://travis-ci.org/FICTURE7/CoCSharp"><img src="https://travis-ci.org/FICTURE7/CoCSharp.svg?branch=rewrite" alt="Build Status"></a>
</h1>
This is the `rewrite` branch where am doing what the branch's name is telling. I decided to rewrite
CoCSharp because some stuff could have been done better.

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

You can also contribute by just giving a star or forking us, also by spreading the word about this project. ^^

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
