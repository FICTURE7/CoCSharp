# CoCSharp
Clash of Clans proxy(turning into a library) written in C# to log packets, log raw packets and monitor traffic between clients and servers. It was written based off of the [Clash of Clans Documentation Project](https://github.com/clanner/cocdp/).

### Implemented Packets
All packets that CoCSharp currently can read and parse, if a packet is not supported it will be returned as an UnknownPacket which contains the encrypted and decrypted payload of the packet.

#### Serverbound - [C > S]

LoginRequest - 0x2775 (10101)

KeepAliveRequest - 0x277C (10108)

ChatMessageClient - 0x397B (14715)

#### Clientbound - [C < S]

UpdateKey - 0x4E20 (20000)

LoginSuccess - 0x4E88 (20104)

KeepAliveResponse - 0x4E8C (20108)

ChatMessageServer - 0x608B (24715)

### Databases/CSV Files
[Download](https://www.dropbox.com/s/ygrrow188az1vg3/database.rar?dl=0) and extract the files and place them in "\database".
