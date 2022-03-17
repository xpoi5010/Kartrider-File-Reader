using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KartRider.Encrypt
{
    public class Rho5DecryptStream : Stream
    {
        public Stream BaseStream { get; set; }
        private Rho5KeyProvider KeyProvider { get; }

        public override bool CanRead => BaseStream.CanRead;

        public override bool CanSeek => BaseStream.CanSeek;

        public override bool CanWrite => false;

        public override long Length => BaseStream.Length;

        public override long Position { get => bufStartPos + bufPos; set { BaseStream.Position = value; bufPos = bufStartPos = 64;  } }

        private byte[] Buffer = new byte[64];

        private int bufStartPos = 0;

        private int bufferCount = 64;

        private int bufPos = 64;

        private bool Inited = false;

        public Rho5DecryptStream(Stream BaseStream, byte[] Key)
        {
            this.BaseStream = BaseStream;
            KeyProvider = new Rho5KeyProvider();
            KeyProvider.InitFromKey(Key);
            Inited = true;
            bufPos = bufStartPos = 64;
        }

        public Rho5DecryptStream(Stream BaseStream, string fileName, string anotherData)
        {
            this.BaseStream = BaseStream;
            KeyProvider = new Rho5KeyProvider();
            KeyProvider.InitHeaderKey(fileName,anotherData);
            Inited = true;
            bufPos = bufStartPos = 64;
        }

        public Rho5DecryptStream(Stream BaseStream)
        {
            this.BaseStream = BaseStream;
            KeyProvider = new Rho5KeyProvider();
            Inited = false;
        }

        public override void Flush()
        {
            BaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            for(int i = 0; i < count; i++)
            {
                if (bufPos >= bufferCount)
                {
                    bool result = refreshBuffer();
                    if (!result)
                        return i;
                }
                buffer[i] = this.Buffer[bufPos];
                bufPos++;
            }
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            BaseStream.Seek(offset, origin);
            long newOffset = BaseStream.Position;
            bufferCount = 64;
            bufPos = 64;
            return newOffset;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        private unsafe bool refreshBuffer()
        {
            bufStartPos = (int)BaseStream.Position;
            int readLen = BaseStream.Read(Buffer, 0, 64);
            if(readLen <=0)
                return false;
            int count = (readLen + 3) >> 2;
            fixed(byte* p = Buffer)
            {
                uint* ptr = (uint*)p;
                for (int i = 0; i < count; i++)
                {
                    ptr[i] -= KeyProvider.GetNextSubNum();
                }
            }
            bufPos = 0;
            bufferCount = readLen;
            return true;
        }

        public void SetToHeaderKey(string fileName,string anotherData)
        {
            KeyProvider.InitHeaderKey(fileName, anotherData);
        }

        public void SetToFileInfoKey(string fileName, string anotherData)
        {
            KeyProvider.InitFileInfoKey(fileName, anotherData);
        }
    }
}
