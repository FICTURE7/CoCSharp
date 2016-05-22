<h1>
CoC# <a href="https://travis-ci.org/FICTURE7/CoCSharp"><img src="https://travis-ci.org/FICTURE7/CoCSharp.svg?branch=rewrite" alt="Build Status"></a>
</h1>
This is the `rewrite` branch where am doing what the branch's name is telling. I decided to rewrite
CoCSharp because some stuff could have been done better.

Clash of Clans library written in C# to handle networking, csv files and more to come.
It was written based off of [Clash of Clans Documentation Project](https://github.com/clanner/cocdp/)
and decompiled source code of [Ultrapowa Clash Server](https://github.com/Ultrapowa/UCS)
which is now open source however no decompiled code was used in this project.

### Notes
Due to the encryption update in 8.x.x, a patched version of `libg.so` with the standard public key is needed for
`CoCSharp.Proxy` and `CoCSharp.Server` to work properly. [Here](https://github.com/clugh/coc-proxy#installation) are the steps needed to patch `libg.so` or you can use [`coc-patcher`](https://github.com/clugh/coc-patcher) a small but great utility made in python by clugh to patch `libg.so` and sign the APK itself.

[`libsodium-net`](https://github.com/adamcaudill/libsodium-net) also requires the
[Visual C++ Redistributable for Visual Studio 2015](https://www.microsoft.com/en-us/download/details.aspx?id=48145), if
its not installed `Sodium.SodiumCore` will throw a `DllNotFoundException`.

## What can it do?
* CoCSharp.Data:
  * Includes classes that defines common Clash of Clans data.
* CoCSharp.Csv: Clash of Clans format CSV reader.
  * Includes classes for reading CSV files.
* CoCSharp.Logic: Village object structures and logic.
  * Includes classes for village objects: Village, Trap, Obstacle, Building etc...
* CoCSharp.Network: Clash of Clans network protocol implementation.
  * Includes classes for reading, writing messages & packet definitions.
* CoCSharp.Networking.Cryptography: Clash of Clans encryption implementation.
  * Include class for encryption version 7.x.x & 8.x.x version.

## Compiling
To be able to run the Post Build properly you must have `mono` in your `PATH` variable.

The simplest way to compile CoCSharp is to open the solution in Visual Studio and pressing `F6` to build the entire
solution, it will restore the nuget packages as well. Or you could use the latest version of mono and nuget to compile CoCSharp.

Run the following commands to build CoCSharp with mono.

Clone the project using git.
```
git clone --branch=rewrite git://github.com/FICTURE7/CoCSharp.git
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
Just create a fork and make changes to it, like adding features from Clash of Clans, fixing bugs and also reporting them
because we want a solid software right? Follow the code style else I won't merge your pull request,
its not that hard to follow ;]. Make your pull requests focused and readable. You can also contribute by
creating issues and reporting bugs, giving ideas for enhancement and by finding issues in the documentation.

You can also contribute by just giving a star or forking us, also by spreading the word about the project. ^^

Am not any merging pull requests like a clan search API which completely drifts away from the goal of this project.
If you want to make a clan search API, use CoCSharp as a library and go ahead. :]

Contact me via my e-mail, FICTURE7@gmail.com if you have any question.

### Thanks
Massive thank you to those [guys](https://github.com/FICTURE7/CoCSharp/blob/rewrite/CONTRIBUTORS) who helped with the project and
specially the MVPs. :]

<h2>Projects Using CoC#</h2>
* [CoCSharp.Server](https://github.com/FICTURE7/CoCSharp/tree/rewrite/CoCSharp.Server): *(Work in progress)*
  * Server that can do some stuff. It also supports the latest 8.x.x encryption. Yey!
* [CoCSharp.Client](https://github.com/FICTURE7/CoCSharp/tree/rewrite/CoCSharp.Client): *(7.x.x available on `master` branch)*
  * Simple client that can read chat message and send keep alive requests.
* [CoCSharp.Proxy](https://github.com/FICTURE7/CoCSharp/tree/rewrite/CoCSharp.Proxy): *(Work in progress)*
  * Proxy that logs raw decrypted packets and village JSON layout.

## Licensing
CoCSharp is licensed under the permissive [MIT License](http://mit-license.org/).

CoCSharp is not officially affiliated with Supercell.
