# CoCSharp
Clash of Clans proxy written in C# to log packets and monitor traffic between clients and servers. It was written based off of the [Clash of Clans Documentation Project](https://github.com/clanner/cocdp/).

### Implemented Packets
All packets that CoCSharp currently can read and parse, if a packet is not supported it will be returned as an UnknownPacket which contains the encrypted and decrypted payload of the packet.

#### Serverbound - [C > S]

LoginRequest - 0x2775 (10101)

KeepAliveRequest - 0x277C (10108)

ChatMessageClient - 0x397B (14715)

#### Clientbound - [C < S]

UpdateKey - 0x4E20 (20000)

LoginSuccess - 0x4E88 (20104)

KeepAliveRequest - 0x4E8C (20108)

ChatMessageServer - 0x608B (24715)
