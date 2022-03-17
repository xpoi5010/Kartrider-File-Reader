using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KartRider;
using KartRider.Xml;
using System.IO;

namespace KartRider.File
{
    public class PackFolderManager
    {
        public bool Initizated { get; private set; } = false;
        private struct ProcessObj
        {
            public string Path;

            public BinaryXmlTag Obj;

            public PackFolderInfo Parent;
        }

        public PackFolderManager()
        {

        }

        private List<PackFolderInfo> RootFolder { get; init; } = new List<PackFolderInfo>();

        private List<Rho> RhoPool { get; init; } = new List<Rho>(2000);

        public void Initization(string aaaPkFilePath)
        {
            FileInfo fileInfo = new FileInfo(aaaPkFilePath);
            if (!fileInfo.Exists)
                throw new FileNotFoundException(aaaPkFilePath);
            while (RhoPool.Count > 0)
            {
                RhoPool[0].Dispose();
                RhoPool.RemoveAt(0);
            }
            RootFolder.Clear();
            FileStream fileStream = new FileStream(aaaPkFilePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fileStream);
            int dataLen = br.ReadInt32();
            byte[] aaapkData = br.ReadKRData(dataLen);
            fileStream.Close();
            BinaryXmlDocument bxmlDoc = new BinaryXmlDocument();
            bxmlDoc.Read(Encoding.GetEncoding("UTF-16"), aaapkData);
            BinaryXmlTag rootTag = bxmlDoc.RootTag;
            Queue<ProcessObj> ProcessQue = new Queue<ProcessObj>();
            if (rootTag.Name != "PackFolder" || rootTag.GetAttribute("name") != "KartRider")
            {
                throw new NotSupportedException($"aaa.pk file not support.");
            }
            foreach (BinaryXmlTag subtag in rootTag.SubTags)
            {
                ProcessQue.Enqueue(new ProcessObj
                {
                    Obj = subtag,
                    Parent = null,
                    Path = ""
                });
            }
            while (ProcessQue.Count > 0)
            {
                ProcessObj currectProcObj = ProcessQue.Dequeue();
                BinaryXmlTag currentTag = currectProcObj.Obj;
                switch (currentTag.Name)
                {
                    case "PackFolder":
                        string subname = currentTag.GetAttribute("name");
                        PackFolderInfo newSubFolder = new PackFolderInfo()
                        {
                            FolderName = currectProcObj.Parent is null ? $"{subname}_" : subname,
                            FullName = currectProcObj.Parent is null ? $"{subname}_" : $"{currectProcObj.Path}/{subname}",
                            ParentFolder = currectProcObj.Parent
                        };
                        foreach (BinaryXmlTag subTag in currentTag.SubTags)
                        {
                            ProcessObj newProcObj = new ProcessObj()
                            {
                                Path = newSubFolder.FullName,
                                Parent = newSubFolder,
                                Obj = subTag
                            };
                            ProcessQue.Enqueue(newProcObj);
                        }
                        if (currectProcObj.Parent is not null)
                            currectProcObj.Parent.Folders.Add(newSubFolder);
                        else
                            RootFolder.Add(newSubFolder);
                        break;
                    case "RhoFolder":
                        string name = currentTag.GetAttribute("name");
                        string fileName = currentTag.GetAttribute("fileName");
                        PackFolderInfo NewFolder = new PackFolderInfo()
                        {
                            FolderName = currectProcObj.Parent is null ? $"{name}_" : name,
                            FullName = currectProcObj.Parent is null ? $"{name}_" : $"{currectProcObj.Path}/{name}",
                            ParentFolder = currectProcObj.Parent
                        };
                        if (name == "")
                            NewFolder = currectProcObj.Parent;
                        else {
                            if (currectProcObj.Parent is null)
                                RootFolder.Add(NewFolder);
                            else
                                currectProcObj.Parent.Folders.Add(NewFolder);
                        }
                        Rho rhoFile = new Rho($"{fileInfo.DirectoryName}\\{fileName}");
                        Queue<(PackFolderInfo, RhoDirectory)> dirQue = new Queue<(PackFolderInfo, RhoDirectory)>();
                        RhoDirectory rootDir = rhoFile.RootDirectory;
                        dirQue.Enqueue((NewFolder, rootDir));
                        while (dirQue.Count > 0)
                        {
                            (PackFolderInfo, RhoDirectory) curObj = dirQue.Dequeue();
                            PackFolderInfo currectPackFolder = curObj.Item1;
                            foreach (RhoFileInfo file in curObj.Item2.Files)
                            {
                                PackFileInfo newFileInfo = new PackFileInfo()
                                {
                                    FileName = file.FullFileName,
                                    FileSize = file.FileSize,
                                    FullName = $"{currectPackFolder.FullName}/{file.FullFileName}",
                                    OriginalFile = file,
                                    PackFileType = PackFileType.RhoFile
                                };
                                currectPackFolder.Files.Add(newFileInfo);
                            }
                            foreach (RhoDirectory dir in curObj.Item2.Directories)
                            {
                                PackFolderInfo subDirInfo = new PackFolderInfo()
                                {
                                    FolderName = dir.DirectoryName,
                                    FullName = $"{currectPackFolder.FullName}/{dir.DirectoryName}",
                                    ParentFolder = currectPackFolder
                                };
                                currectPackFolder.Folders.Add(subDirInfo);
                                dirQue.Enqueue((subDirInfo, dir));
                            }
                        }
                        RhoPool.Add(rhoFile);
                        break;
                }
            }
            RegionCode regionCode = RegionCode.None;
            PackFolderInfo[] ZETA_Folders = GetDirectories("zeta_");
            if (Array.Exists(ZETA_Folders, x => x.FolderName == "kr"))
                regionCode = RegionCode.Korea;
            else if (Array.Exists(ZETA_Folders, x => x.FolderName == "cn"))
                regionCode = RegionCode.China;
            else if (Array.Exists(ZETA_Folders, x => x.FolderName == "tw"))
                regionCode = RegionCode.Taiwan;
            Initizated = true;
        }

        public PackFolderInfo[] GetDirectories(string Path)
        {
            if (Path == "")
                return RootFolder.ToArray();
            string[] path_sp = Path.Split('/');
            List<PackFolderInfo> currentFindFolders = RootFolder;
            foreach (string sp in path_sp)
            {
                PackFolderInfo findFolder = currentFindFolders.Find(x => x.FolderName == sp);
                if (findFolder is null)
                    return null;
                currentFindFolders = findFolder.Folders;
            }
            return currentFindFolders.ToArray();
        }
    }
    public class PackFolderInfo
    {
        public string FolderName { get; set; }
        public string FullName { get; set; }
        public PackFolderInfo ParentFolder { get; set; }
        public List<PackFolderInfo> Folders { get; init; } = new List<PackFolderInfo>();
        public List<PackFileInfo> Files { get; init; } = new List<PackFileInfo>();

    }
    public class PackFileInfo
    {
        public string FileName { get; set; }
        public string FullName { get; set; }
        public int FileSize { get; set; }
        public PackFileType PackFileType { get; set; }
        public object OriginalFile { get; set; }
    }

    public enum PackFileType
    {
        RhoFile,
        Rho5File
    }
}
