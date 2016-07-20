using System;

namespace CoCSharp.Logic
{
    // Experimenting with stuff here.

    [Flags]
    enum LogicOperations
    {
        None = 0,

        Buy = 2,

        Upgrade = 4,

        Sell = 8,

        SpeedUp = 16
    }
}
