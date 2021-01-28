using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KartRider.File
{
    public static class RhoPackedFilesInfoDecoder
    {
        //CurrentPath : /
        public static IPackedObject[] GetRhoPackedFileInfos(byte[] CryptedData,uint HeaderKey,uint CurrentPathindex)
        {
            byte[] DecryptedData = Crypt.RhoCrypt.Decrypt(CryptedData, HeaderKey + 0x2593A9F1);
            List<IPackedObject> files = new List<IPackedObject>();
            using (MemoryStream ms = new MemoryStream(DecryptedData))
            {
                BinaryReader br = new BinaryReader(ms);
                int type1Count = br.ReadInt32();
                for (int i = 1; i <= type1Count; i++)
                {
                    List<char> t_FileName = new List<char>();
                    short a = br.ReadInt16();
                    while (a != 0x00)
                    {
                        t_FileName.Add((char)a);
                        a = br.ReadInt16();
                    }
                    uint index = br.ReadUInt32();
                    string filename = new string(t_FileName.ToArray());
                    files.Add(new RhoPackedFolderInfo()
                    {
                        Index = index,
                        FolderName = filename,
                        ParentIndex = CurrentPathindex
                    }) ;
                }
                int type2Count = br.ReadInt32();
                for (int i = 1; i <= type2Count; i++)
                {
                    List<char> t_FileName = new List<char>();
                    short a = br.ReadInt16();
                    while (a != 0x00)
                    {
                        t_FileName.Add((char)a);
                        a = br.ReadInt16();
                    }
                    uint ext = br.ReadUInt32();
                    int cm = br.ReadInt32();
                    uint index = br.ReadUInt32();
                    int fileSize = br.ReadInt32();//FileSize
                    string filename = new string(t_FileName.ToArray());
                    files.Add(new RhoPackedFileInfo()
                    {
                        CryptMode = (CryptMode)cm,
                        Ext = ext,
                        FileName = filename,
                        Index = index,
                        Path = CurrentPathindex,
                        FileSize = fileSize
                    });
                }
            }
            return files.ToArray();
        }
    }
}
