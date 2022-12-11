using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using KartRider.Xml;
using KartRider.Record;
using Ionic.Zlib;

namespace KartRider.IO
{
    public static class BinaryReaderExt
    {
        public static string ReadText(this BinaryReader br,Encoding encoding,int Count)
        {
            byte[] data = br.ReadBytes(Count);
            return encoding.GetString(data);
        }

        public static string ReadText(this BinaryReader br, Encoding encoding)
        {
            int count = br.ReadInt32() << 1;
            byte[] data = br.ReadBytes(count);
            return encoding.GetString(data);
        }

        public static BinaryXmlTag ReadBinaryXmlTag(this BinaryReader br, Encoding encoding)
        {
            BinaryXmlTag tag = new BinaryXmlTag();
            tag.Name = br.ReadText(encoding);
            //Text
            tag.Text = br.ReadText(encoding);
            //Attributes
            int attCount = br.ReadInt32();
            for (int i = 0; i < attCount; i++)
                tag.Attributes.Add(br.ReadText(encoding), br.ReadText(encoding));
            //SubTags
            int SubCount = br.ReadInt32();
            for (int i = 0; i < SubCount; i++)
                tag.SubTags.Add(br.ReadBinaryXmlTag(encoding));
            return tag;
        }
    }

    public static class KSVBinaryRExt
    {
        public static DateTime ReadKRDateTime(this BinaryReader br)
        {
            DateTime dt = new DateTime(1900, 1, 1);
            uint date = (uint)br.ReadUInt16();
            uint time = (uint)br.ReadUInt16() * 4;
            dt = dt.AddDays(date);
            dt = dt.AddSeconds(time);
            return dt;
        }
        public static string ReadKRString(this BinaryReader br)
        {
            int len = br.ReadInt32();
            byte[] strData = br.ReadBytes(len << 1);
            return Encoding.GetEncoding("UTF-16").GetString(strData);
        }

        public static KSVInfo ReadKSVInfo(this BinaryReader br)
        {
            KSVInfo ki = new KSVInfo();
            uint headerClassIdentifier = br.ReadUInt32();
            ki.RecordHeaderVersion = KSVStructVersion.GetHeaderVersion(headerClassIdentifier);
            ki.RecordTitle = br.ReadKRString();
            ki.RegionCode = (RegionCode)br.ReadInt16();
            ki.Unknown1_1 = br.ReadByte();
            ki.ContestType = (ContestType)br.ReadByte();
            ki.PlayerNameHash = br.ReadUInt32();
            ki.Unknown1_2 = br.ReadUInt32();
            ki.RecorderAccount = br.ReadKRString();
            ki.RecorderName = br.ReadKRString();
            ki.RecordingDate = br.ReadKRDateTime();
            ki.RecordChecksum = br.ReadUInt32();
            ki.IsOffical = br.ReadByte() == 1;
            ki.Description = br.ReadKRString();
            ki.TrackName = br.ReadKRString();
            ki.Unknown3 = br.ReadInt32();
            ki.BestTime = new TimeSpan(0, 0, 0, 0, br.ReadInt32());
            ki.ContestImg = br.ReadKRString();
            ki.Unknown4 = br.ReadInt32();
            ki.Unknown5 = br.ReadInt32();
            ki.Unknown6 = br.ReadByte();
            if (ki.RecordHeaderVersion >= 9)
                ki.Speed = (SpeedType)(br.ReadByte());
            int playerCount = br.ReadInt32();
            PlayerInfo[] players = new PlayerInfo[playerCount];
            for (int i = 0; i < playerCount; i++)
                players[i] = br.ReadPlayerInfo(ki.RecordHeaderVersion);
            ki.Players = players;
            uint recordClassIdentifier = br.ReadUInt32();
            ki.RecordVersion = KSVStructVersion.GetVersion(recordClassIdentifier);
            int recordCount = br.ReadInt32();
            RecordData[] records = new RecordData[recordCount];
            for (int i = 0; i < recordCount; i++)
                records[i] = br.ReadRecordData(ki.RecordHeaderVersion);
            ki.Records = records;
            return ki;
        }

        public static PlayerInfo ReadPlayerInfo(this BinaryReader br, int KSVHeaderVersion)
        {
            PlayerInfo pi = new PlayerInfo();
            pi.PlayerName = br.ReadKRString();
            pi.ClubName = br.ReadKRString();
            pi.Equipment = br.ReadPlayerEquipment(KSVHeaderVersion);
            return pi;
        }

        public static PlayerEquipment ReadPlayerEquipment(this BinaryReader br, int KSVHeaderVersion)
        {
            PlayerEquipment pe = new PlayerEquipment();
            pe.Character = br.ReadInt16();
            if (KSVHeaderVersion >= 10)
                pe.KartPaint = br.ReadInt16();
            pe.CharacterColor = br.ReadInt16();
            pe.Kart = br.ReadInt16();
            pe.Plate = br.ReadInt16();
            pe.Goggle = br.ReadInt16();
            pe.Balloon = br.ReadInt16();
            pe.Equ2 = br.ReadInt16();
            pe.Headband = br.ReadInt16();
            pe.Replay = br.ReadInt16();
            pe.Cane = br.ReadInt16();
            pe.Equ3 = br.ReadInt16();
            pe.Apparel = br.ReadInt16();
            pe.Equ4 = br.ReadInt16();
            pe.PlateText = br.ReadKRString();
            pe.Equ5 = br.ReadInt16();
            pe.Equ6 = br.ReadInt16();
            pe.Equ7 = br.ReadInt16();
            pe.Equ8 = br.ReadInt16();
            pe.Equ9 = br.ReadInt16();
            pe.Equ10 = br.ReadInt16();
            pe.Equ11 = br.ReadInt16();
            if (KSVHeaderVersion >= 11)
            {
                pe.Equ12 = br.ReadInt16();
                pe.Equ13 = br.ReadInt16();
            }
            return pe;
        }

        public static RecordData ReadRecordData(this BinaryReader br, int KSVHeaderVersion)
        {
            RecordData rd = new RecordData();
            int totalCount = br.ReadInt32();
            RecordStamp[] rss = new RecordStamp[totalCount];
            for (int i = 0; i < totalCount; i++)
            {
                rss[i] = br.ReadRecordStramp(KSVHeaderVersion);
            }
            rd.Stamps = rss;
            return rd;
        }

        public static RecordStamp ReadRecordStramp(this BinaryReader br, int KSVHeaderVersion)
        {
            RecordStamp rs = new RecordStamp();
            rs.Time = br.ReadInt16() * 100;
            rs.X = br.ReadInt16() * 0.1f;
            rs.Y = br.ReadInt16() * 0.1f;
            rs.Z = br.ReadInt16() * 0.1f;
            float angle_W = br.ReadInt16() * 0.01f;
            float angle_X = br.ReadInt16() * 0.01f;
            float angle_Y = br.ReadInt16() * 0.01f;
            float angle_Z = br.ReadInt16() * 0.01f;
            rs.Angle = new System.Numerics.Quaternion(angle_X, angle_Y, angle_Z, angle_W);
            rs.Status = br.ReadUInt16();
            return rs;
        }
    }
}
