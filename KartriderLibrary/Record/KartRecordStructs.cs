using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zlib;

namespace KartRider.Record
{
    public enum SpeedType
    {
        Normal,
        HighSpeed,
        SuperSpeed,
        Slow,
        Nolimit
    }

    public enum ContestType
    {
        Invaild,
        PersonalRace,
        PersonalItem,
        TeamRace,
        TeamItem,
        SchoolSpeed,
        SchoolItem,
        TeamFlag,
        PersonalFlag,
        PersonalTimeRace
    }

}
