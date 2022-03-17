using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartRider.Record
{
    public class RecordStamp
    {
        public double Time { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float angle_W { get; set; }
        public float angle_X { get; set; }
        public float angle_Y { get; set; }
        public float angle_Z { get; set; }
        public ushort Status { get; set; }
        public string[] GetCarStatus()
        {
            string[] gasStatus = { "", "噴紅氣", "噴藍氣", "短噴", "開前噴", "gas(101)", "gas(110)", "開噴" };
            string[] characterStatus = { "", "左擺頭", "右擺頭", "閃到頭", "倒退頭", "倒左頭", "倒右頭", "撞到頭" };
            string[] effectStatus = { "", "加速特效", "甩尾特效", "甩+加速" };
            List<string> output = new List<string>();
            if (gasStatus[Status & 7] != "")
                output.Add(gasStatus[Status & 7]);
            if (characterStatus[(Status >> 3) & 7] != "")
                output.Add(characterStatus[(Status >> 3 & 7)]);
            if (effectStatus[(Status >> 6) & 3] != "")
                output.Add(effectStatus[(Status >> 6 & 3)]);
            return output.ToArray();

        }
    }
}
