<h1> 
CoC# <a href="https://travis-ci.org/FICTURE7/CoCSharp"><img src="https://travis-ci.org/FICTURE7/CoCSharp.svg?branch=rewrite" alt="Build Status"></a>
</h1>
This is the `rewrite` branch where am doing what the branch's name is telling. I decided to rewrite
CoCSharp because some stuff could have been done better.

Clash of Clans library written in C# to handle networking, csv files and more to come. 
It was written based off of [Clash of Clans Documentation Project](https://github.com/clanner/cocdp/)
and decompiled source code of [Ultrapowa Clash Server](https://github.com/Ultrapowa/UCS) 
which is now open source however no decompiled code was used in this project.

## What can it do?
* CoCSharp.Csv: Clash of Clans format CSV reader.
  * Includes classes for reading CSV files.
* CoCSharp.Logging: *(Needs to implemented)*
  * Includes classes to log and dump packet.
* CoCSharp.Logic: Village object strutures and logic. *(Needs to be improved)*
  * Includes classes for village objects: Village, Trap, Obstacle, Building etc...
* CoCSharp.Networking: Clash of Clans network protocol implementaion.
  * Includes classes for reading and writing messages.
  * Includes some classes containing packet definition.
* CoCSharp.Networking.Cryptography: Clash of Clans encryption implementation.
  * Include class for encryption version 7.x.x and 8.x.x version.

## Compiling
The simplest way to compile CoCSharp is to open the solution in Visual Studio and pressing `F6` to build the entire
solution or you could use the latest version of mono to compile CoCSharp.

Run the following commands to build CoCSharp with mono.

Clone the project using git.
```
git clone --branch=rewrite git://github.com/FICTURE7/CoCSharp.git
```
Then you will have to restore the nuget packages, to get the dependencies.
```
nuget restore
```
In the root directory run or where `CoCSharp.sln` is located however it wont compile with the current `CoCSharp.sln`.
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
Follow the code style else I won't merge your pull request, its not that hard to follow ;]. Make your pull 
requests focused and readable. You can also contribute by creating issues and reporting bugs, giving ideas 
for enhancement and by finding issues in the documentation.

You can also contribute by just giving a star or forking us, also by spreading the word about the project. ^^

Am not any merging pull requests like a clan search API which completely drifts away from the goal of this project. 
If you want to make a clan search API, use CoCSharp as a library and go ahead. :]

### Thanks
Massive thank you to those [guys](https://github.com/FICTURE7/CoCSharp/blob/rewrite/CONTRIBUTORS) who helped with the project! :]

<h2>Projects Using CoC#</h2>
* [CoCSharp.Server](https://github.com/FICTURE7/CoCSharp/tree/rewrite/CoCSharp.Server): *(Work in progress)*
  * Server that can save villages and buy actions. It also supports the latest 8.x.x encryption. Yey!
* [CoCSharp.Client](https://github.com/FICTURE7/CoCSharp/tree/rewrite/CoCSharp.Client): *(Not available)*
  * Simple client that can read chat message and send keep alive requests.
* [CoCSharp.Proxy](https://github.com/FICTURE7/CoCSharp/tree/rewrite/CoCSharp.Proxy): *(Work in progress)*
  * Proxy that could log parsed and raw decrypted/encrypted packets.

## Licensing
CoCSharp is licensed under the permissive [MIT License](http://mit-license.org/).

CoCSharp is not officially affiliated with Supercell.
