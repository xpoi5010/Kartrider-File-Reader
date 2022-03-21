using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KartRider.Encrypt
{
    public static class RhoEncrypt
    {

        /// <summary>
        /// Used to decrypt rho file data, or DataProcessed data.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static byte[] DecryptData(uint Key,byte[] Data)
        {
            byte[] extendedKey = RhoKey.ExtendKey(Key);
            byte[] output = new byte[Data.Length];
            for (int i = 0; i < Data.Length; i++)
            {
                output[i] = (byte)(Data[i] ^ extendedKey[i & 63]);
            }
            return output;
        }

        public static async void DecryptData(uint Key,byte[] Data,int Offset,int Length)
        {
            if ((Offset + Length) > Data.Length)
                throw new Exception("Over range.");
            byte[] extendedKey = RhoKey.ExtendKey(Key);
            
            for (int i = 0; i < Length; i++)
            {
                int index = i + Offset;
                Data[index] = (byte)(Data[index] ^ extendedKey[index & 63]);
            }
            /*
            List<Task> tasks = new List<Task>();
            int ThreadCount = 3;
            int lenPerThread = Length/ ThreadCount;
            int allocLen = Length;
            for(int i = 0; i < ThreadCount; i++)
            {
                tasks.Add(DecryptData(extendedKey,Data,i* lenPerThread, (allocLen >=lenPerThread ? lenPerThread : allocLen)));
                allocLen -= lenPerThread;
            }
            await Task.WhenAll(tasks);
            */
        }

        private static async Task DecryptData(byte[] extendedKey, byte[] Data, int Offset, int Length)
        {
            for (int i = 0; i < Length; i++)
            {
                int index = i + Offset;
                Data[index] = (byte)(Data[index] ^ extendedKey[index & 63]);
            }
        }

        /// <summary>
        /// Used to decrypt rho header, and block info.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static unsafe byte[] DecryptHeaderInfo(byte[] Data,uint Key)
        {
            uint currentKey = Key;
            uint a = 0;
            byte[] output = new byte[Data.Length];
            fixed(byte *wPtr = output, rPtr = Data)
            {
                uint* writePtr = (uint*)wPtr;
                uint* readPtr = (uint*)rPtr;
                for(int i =0;i< Data.Length >> 2; i++)
                {
                    uint vector = RhoKey.GetVector(currentKey);
                    uint curData = readPtr[i];
                    curData ^= vector;
                    curData ^= a;
                    writePtr[i] = curData;
                    a += curData;
                    currentKey++;
                }
            }
            return output;
        }

        public static byte[] DecryptBlockInfoOld(byte[] Data,byte[] key)
        {
            if (Data.Length != 0x20)
                throw new NotSupportedException("Exception: the length of Data is not 32 bytes.");
            byte[] output = new byte[32];
            for (int i = 0; i < 32; i++)
                output[i] =(byte)( key[i] ^ Data[i]);
            return output;
        }

        /// <summary>
        /// Used to encrypt rho file data, or DataProcessed data.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static byte[] EncryptData(uint Key, byte[] Data)
        {
            byte[] extendedKey = RhoKey.ExtendKey(Key);
            byte[] output = new byte[Data.Length];
            for (int i = 0; i < Data.Length; i++)
            {
                output[i] = (byte)(Data[i] ^ extendedKey[i & 63]);
            }
            return output;
        }

        public static void EncryptData(uint Key, byte[] Data, int Offset, int Length)
        {
            if ((Offset + Length) >= Data.Length)
                throw new Exception("Over range.");
            byte[] extendedKey = RhoKey.ExtendKey(Key);
            for (int i = 0; i < Length; i++)
            {
                int index = i + Offset;
                Data[index] = (byte)(Data[index] ^ extendedKey[index & 63]);
            }
        }

        /// <summary>
        /// Used to encrypt rho header, and block info.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static byte[] EncryptHeaderInfo(byte[] Data, uint Key)
        {
            uint curKey = Key;
            uint a = 0;
            byte[] output = new byte[Data.Length];
            using (MemoryStream rs = new MemoryStream(Data), ws = new MemoryStream())
            {
                BinaryReader br = new BinaryReader(rs);
                BinaryWriter bw = new BinaryWriter(ws);
                for (int i = 0; i < Data.Length; i += 4)
                {
                    uint vector = RhoKey.GetVector(curKey);
                    uint curData = br.ReadUInt32();
                    uint cryData = curData ^ vector;
                    cryData ^= a;
                    a += curData;
                    bw.Write(curData);
                    curKey++;
                }
                return ws.ToArray();
            }
        }
    }
}
