using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.File
{
    public class FileDataSource: IDataSource
    {
        private string _fileName;
        private FileStream _stream;
        private bool _locked;
        private int _size;

        private bool _disposed;

        public bool Locked => _locked;

        public int Size => _size;

        
        public FileDataSource(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
                throw new FileNotFoundException("file not found", fileName);
            _fileName = fileName;
            _stream = new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            _size = (int)_stream.Length;
            _locked = false;
            _disposed = false;
        }

        
        public Stream CreateStream()
        {
            return new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public void WriteTo(Stream stream)
        {
            _stream.CopyTo(stream);
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            await _stream.CopyToAsync(stream, cancellationToken);
        }

        public void WriteTo(byte[] buffer, int offset, int count)
        {
            _stream.Read(buffer, offset, count);
        }

        public async Task WriteToAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            await _stream.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public byte[] GetBytes()
        {
            byte[] output = new byte[_size];
            _stream.Read(output);
            return output;
        }

        public async Task<byte[]> GetBytesAsync(CancellationToken cancellationToken = default)
        {
            byte[] output = new byte[_size];
            await WriteToAsync(output, 0, output.Length);
            return output;
        }

        public void Dispose()
        {
            _stream.Dispose();
            _disposed = true;
        }
    }
}
