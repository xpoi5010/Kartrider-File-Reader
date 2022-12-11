using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KartRider.Encrypt;
using Ionic.Zlib;

namespace KartRider.File
{
    public class RhoBlockInfo: IComparable<RhoBlockInfo>
    {
        public uint Index { get; set; }
        public long Offset { get; set; }
        public int BlockSize { get; set; }
        public int OriginalSize { get; set; }
        public RhoBlockProperty BlockProperty { get; set; }
        public uint Hash { get; set; }

        public int CompareTo(RhoBlockInfo? other)
        {
            return this.Index.CompareTo(other?.Index);
        }
    }
    //Extension
    public static class RhoBlockReader
    {
        public static RhoBlockInfo ReadBlockInfo(this BinaryReader reader,uint Key)
        {
            RhoBlockInfo output = new RhoBlockInfo();
            byte[] blockInfoData = reader.ReadBytes(0x20);
            blockInfoData = RhoEncrypt.DecryptHeaderInfo(blockInfoData, Key);
            using(MemoryStream ms = new MemoryStream(blockInfoData))
            {
                BinaryReader msReader = new BinaryReader(ms);
                output.Index = msReader.ReadUInt32();
                output.Offset = msReader.ReadUInt32() << 8;
                output.BlockSize = msReader.ReadInt32();
                output.OriginalSize = msReader.ReadInt32();
                output.BlockProperty = (RhoBlockProperty)msReader.ReadInt32();
                output.Hash = msReader.ReadUInt32();
            }
            return output;
        }

        // For Rho layer 1.0
        public static RhoBlockInfo ReadBlockInfoOld(this BinaryReader reader, byte[] Key)
        {
            RhoBlockInfo output = new RhoBlockInfo();
            byte[] blockInfoData = reader.ReadBytes(0x20);
            blockInfoData = RhoEncrypt.DecryptBlockInfoOld(blockInfoData, Key);
            using (MemoryStream ms = new MemoryStream(blockInfoData))
            {
                BinaryReader msReader = new BinaryReader(ms);
                output.Index = msReader.ReadUInt32();
                output.Offset = msReader.ReadUInt32() << 8;
                output.BlockSize = msReader.ReadInt32();
                output.OriginalSize = msReader.ReadInt32();
                output.BlockProperty = (RhoBlockProperty)msReader.ReadInt32();
                output.Hash = msReader.ReadUInt32();
            }
            return output;
        }

        public static byte[] ReadBlock(this BinaryReader reader, Rho RhoFile, uint BlockIndex,uint Key)
        {
            RhoBlockInfo BlockInfo = RhoFile.GetBlockInfo(BlockIndex);
            if (BlockInfo is null)
                return null;
            reader.BaseStream.Seek(BlockInfo.Offset, SeekOrigin.Begin);
            byte[] BlockData = reader.ReadBytes(BlockInfo.BlockSize);
            if ((BlockInfo.BlockProperty & RhoBlockProperty.Compressed) == RhoBlockProperty.Compressed)
            {
                using (MemoryStream ms = new MemoryStream(BlockData))
                {
                    BlockData = new byte[BlockInfo.OriginalSize];
                    ZlibStream ds = new ZlibStream(ms, CompressionMode.Decompress);
                    ds.Read(BlockData, 0, BlockData.Length);
                }
            }
            if ((BlockInfo.BlockProperty & RhoBlockProperty.PartialEncrypted) == RhoBlockProperty.PartialEncrypted) // Encrypted or PartialEncrypted
            {
                RhoEncrypt.DecryptData(Key, BlockData,0,BlockData.Length);
            }
            if(BlockInfo.BlockProperty == RhoBlockProperty.PartialEncrypted) // PartialEncrypted
            {
                RhoBlockInfo secPartInfo = RhoFile.GetBlockInfo(BlockIndex+1);
                if (secPartInfo is null)
                    return BlockData;
                Array.Resize(ref BlockData,BlockInfo.BlockSize + secPartInfo.BlockSize);
                reader.BaseStream.Read(BlockData, BlockInfo.BlockSize, secPartInfo.BlockSize);
            }
            return BlockData;
        }
    }

    public enum RhoBlockProperty
    {
        None,
        Compressed = 2,
        PartialEncrypted = 4,
        FullEncrypted = 5,
        CompressedEncrypted = FullEncrypted | Compressed
    }
}
