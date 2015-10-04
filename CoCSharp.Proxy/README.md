<h1>CoC# Proxy</h1>
The CoC# proxy, a Clash of Clans proxy thats logs parsed and decrypted packets. CoCSharp.Proxy 
uses CoCSharp as its core library where most of the networking functionalities are implemented.

## What can it do?
It can decrypt encrypted Clash of Clans packets/messages and trys to read them.

If the packet/message has not been implemented, an UnknownPacket will be returned and this UnknownPacket contains:
* Packet Length
* Packet ID
* Unknown (Lets call it Packet Version)
* Encrypted bytes
* Decrypted bytes

CoC#.Proxy also stores user information such as: user token, user ID, fingerprint hash and village JSON 
inside of memory. Just for debugging purposes.

## How to use it?
To use the proxy just edit the host *gamea.clashofclans.com* to the machine running the proxy Ex: *192.168.1.1*
using [Host Editor](https://play.google.com/store/apps/details?id=com.nilhcem.hostseditor&hl=en) and you 
should see some fancy logs appearing in your stdout. =]
