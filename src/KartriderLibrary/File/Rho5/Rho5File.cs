using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KartLibrary.File
{
    public class Rho5File : IRhoFile, IModifiableRhoFile
    {
        #region Members
        internal Rho5Folder? _parentFolder;
        internal int _dataPackID;

        private string _name;
        private string _nameWithoutExt;
        private string _fullname;
        private IDataSource? _dataSource;

        private string _originalName;
        private IDataSource? _originalSource;

        private bool _disposed;
        #endregion

        #region Properties
        public Rho5Folder? Parent => _parentFolder;

        IRhoFolder? IRhoFile.Parent => Parent;

        IModifiableRhoFolder? IModifiableRhoFile.Parent => Parent;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                Regex fileNamePattern = new Regex(@"^(.*)\..*");
                Match match = fileNamePattern.Match(_name);
                if (match.Success)
                {
                    _nameWithoutExt = match.Groups[1].Value;
                }
                else
                {
                    _nameWithoutExt = _name;
                }
            }
        }

        public string FullName
        {
            get => Parent is not null ? $"{Parent.FullName}/{_name}" : _name;
        }

        public string NameWithoutExt => _nameWithoutExt;

        public int Size => _dataSource?.Size ?? 0;

        public IDataSource? DataSource
        {
            get => _dataSource;
            set => _dataSource = value;
        }

        public bool IsModified => _originalName != _name || _originalSource != _dataSource;
        #endregion

        #region Constructors
        public Rho5File()
        {
            _parentFolder = null;
            _name = "";
            _nameWithoutExt = "";
            _fullname = "";
            _dataSource = null;
            _dataPackID = -1;
            _originalSource = null;
            _originalName = "";
        }
        #endregion

        #region Methods
        public Stream CreateStream()
        {
            if (_dataSource is null)
                throw new InvalidOperationException("DataSource is null.");
            return _dataSource.CreateStream();
        }

        public void Dispose()
        {

        }

        public override string ToString()
        {
            return $"Rho5File:{FullName}";
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

                }
            }
        }

        internal void appliedChanges()
        {
            _originalName = _name;
            _originalSource = _dataSource;
        }
        #endregion
    }
}
