using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KartLibrary.File;
using osu.Framework.IO.Stores;

namespace KartCityStudio.Game.IO.Stores
{
    public class KartStorageResourceStore : IResourceStore<byte[]>
    {
        private KartStorageSystem storageSystem;

        public KartStorageResourceStore(KartStorageSystem storageSystem)
        {
            this.storageSystem = storageSystem;
        }

        public void Dispose()
        {  
            
        }

        public byte[] Get(string name)
        {
            KartStorageFile kartFile = storageSystem.GetFile(name);
            return kartFile?.GetBytes();
        }

        public async Task<byte[]> GetAsync(string name, CancellationToken cancellationToken = default)
        {
            KartStorageFile kartFile = storageSystem.GetFile(name);
            return await kartFile?.GetBytesAsync(cancellationToken);
        }

        public IEnumerable<string> GetAvailableResources()
        {
            List<string> resourceNames = new List<string>();
            Queue<KartStorageFolder> folderQueue = new Queue<KartStorageFolder>();
            folderQueue.Enqueue(storageSystem.RootFolder);
            while(folderQueue.Count > 0)
            {
                KartStorageFolder folder = folderQueue.Dequeue();
                foreach(KartStorageFolder subFolder in folder.Folders)
                    folderQueue.Enqueue(subFolder);
                foreach(KartStorageFile file in folder.Files)
                    resourceNames.Add(file.FullName);
            }
            return resourceNames;
        }

        public Stream GetStream(string name)
        {
            KartStorageFile kartFile = storageSystem.GetFile(name);
            return kartFile?.CreateStream();
        }
    }
}
