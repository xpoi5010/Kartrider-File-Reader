using KartLibrary.Consts;
using KartLibrary.Game.Engine.Relements;
using KartLibrary.Game.Engine.Track;
using KartLibrary.File;
using KartLibrary.IO;
using KartLibrary.Tests.Command;
using KartLibrary.Tests.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Formats.Tar;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Testing
{
    public class TestKartStorageSystem: TestStage
    {
        private KartStorageSystem _storageSystem;

        private KartStorageFolder _currentFolder;

        private string _dataFolderPath = @"H:\game\KartRider\Data";

        private CountryCode _region = CountryCode.KR;

        private Dictionary<string, Process> _openedTmpFiles = new Dictionary<string, Process>();

        public TestKartStorageSystem()
        {
            addStep("Construct KartStorageSystem", () =>
            {
                KartStorageSystemBuilder kartStorageSystemBuilder = new KartStorageSystemBuilder();
                _storageSystem =
                    kartStorageSystemBuilder
                        .UseRho()
                        .UseRho5()
                        .SetDataPath(_dataFolderPath)
                        .SetClientRegion(_region)
                        .Build();
            });
            addStep("Close Opened KartStorageSystem", () =>
            {
                if(_storageSystem.IsInitialized)
                    _storageSystem.Close();
            });
            addStep("Initialize KartStorageSystem", () =>
            {
                _storageSystem.Initialize();
                _currentFolder = _storageSystem.RootFolder;
            });
            addStep("Close KartStorageSystem", () =>
            {
                _storageSystem.Close();
            });
            addStep("Initialize KartStorageSystem and set current folder", () =>
            {
                _storageSystem.Initialize();
                _currentFolder = _storageSystem.RootFolder;
            });
        }

        protected override void OnExit()
        {
            if (_storageSystem is not null && _storageSystem.IsInitialized)
                _storageSystem.Close();
            if (_openedTmpFiles.Count > 0)
            {
                foreach(KeyValuePair<string, Process> tmpFile in _openedTmpFiles)
                {
                    if (!tmpFile.Value.HasExited)
                    {
                        tmpFile.Value.Kill();
                        tmpFile.Value.WaitForExitAsync().Wait(300);
                    }
                    if (tmpFile.Value.HasExited)
                    {
                        System.IO.File.Delete(tmpFile.Key);
                    }
                }
            }
            _currentFolder = null;
            base.OnExit();
        }

        [Command("init", "Initialize KartStorageSystem")]
        private CommandExecuteResult commandInitialize(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (_storageSystem is not null)
                if (_storageSystem.IsInitialized)
                    return new CommandExecuteResult(ResultType.Failure, "KartStorageSystem has been initialized.");
                else
                {
                    _storageSystem.Dispose();
                    _storageSystem = null;
                }
            KartStorageSystemBuilder kartStorageSystemBuilder = new KartStorageSystemBuilder();
            if (argumentQueue.Count > 0)
            {
                while (argumentQueue.Count > 0)
                {
                    string option = argumentQueue.PopOption();
                    switch (option)
                    {
                        case "use-file-list":
                            kartStorageSystemBuilder.UsePackFolderListFile();
                            break;
                    }
                }
            }
            _storageSystem =
                kartStorageSystemBuilder
                    .UseRho()
                    .UseRho5()
                    .SetDataPath(_dataFolderPath)
                    .SetClientRegion(_region)
                    .Build();
            _storageSystem.Initialize();
            _currentFolder = _storageSystem.RootFolder;
            commandConsole.WriteLine("Initialization complete.");
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [CommandAutoComplete("init")]
        private string[] commandAutoComplInitialize(CommandArgumentQueue argumentQueue)
        {
            if(argumentQueue.Count > 0)
            {
                while (argumentQueue.Count > 1)
                    argumentQueue.Pop();
                if(argumentQueue.CommandArgumentType == CommandArgumentType.Option)
                {
                    string option = argumentQueue.PopOption();
                    if ("use-file-list".StartsWith(option))
                        return new string[] { "use-file-list" };
                }
            }
            return Array.Empty<string>();
        }

        [Command("set", "Set environment variable of this stage.")]
        private CommandExecuteResult commandSet(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if(argumentQueue.Count == 0)
            {
                commandConsole.WriteLine("Usage: set <variable_name> <variable_value>");
                commandConsole.WriteLine("\tavailable variables:");
                commandConsole.WriteLine("\t\tregion: korea, china, taiwan");
                commandConsole.WriteLine("\t\tdatapath");
                return new CommandExecuteResult(ResultType.Success, "");
            }
            else
            {
                string variableName = argumentQueue.PopArgumentString();
                string variableValue = argumentQueue.PopArgumentString();
                switch (variableName)
                {
                    case "region":
                        switch(variableValue.ToLower())
                        {
                            case "korea": _region = CountryCode.KR; break;
                            case "china": _region = CountryCode.CN; break;
                            case "taiwan": _region = CountryCode.TW; break;
                        }
                        break;
                    case "datapath":
                        if (!Directory.Exists(variableValue))
                            return new CommandExecuteResult(ResultType.Failure, $"Cannot found folder: {variableValue}.");
                        _dataFolderPath = variableValue;
                        break;
                    default:
                        return new CommandExecuteResult(ResultType.Failure, $"There are no variable called \"{variableValue}\"");
                }
            }
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [Command("ls", "List all contents of current folder or specific folder.")]
        private CommandExecuteResult commandListFolder(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (_storageSystem is null || !_storageSystem.IsInitialized)
                return new CommandExecuteResult(ResultType.Failure, "KartStorageSystem has not been initialized.");
            if (_currentFolder is null)
                _currentFolder = _storageSystem.RootFolder;
            KartStorageFolder listFolder = _currentFolder;
            if (argumentQueue.Count > 0)
            {
                string folderName = argumentQueue.PopArgumentString();
                KartStorageFolder? findFolder = _currentFolder.GetFolder(folderName);
                if (findFolder is null)
                    if (_currentFolder.ContainsFile(folderName))
                        return new CommandExecuteResult(ResultType.Failure, $"\"{folderName}\" is a file, not a folder.");
                    else
                        return new CommandExecuteResult(ResultType.Failure, $"Folder: \"{folderName}\" is not exist in current folder.");
                listFolder = findFolder;
            }
            commandConsole.WriteLine($"\n\tFolder: {listFolder.FullName}\n");
            commandConsole.WriteLine($"  Type      |  Size        |  Name");
            commandConsole.WriteLine($"------------+--------------+------------------");
            foreach (KartStorageFolder folder in listFolder.Folders)
                commandConsole.WriteLine($" [ Folder ]                  {folder.Name}");
            foreach (KartStorageFile file in listFolder.Files)
                commandConsole.WriteLine($" [  File  ]   {UnitUtility.FormatDataSize(file.Size)}   {file.Name}");
            commandConsole.WriteLine("");
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [Command("cd", "Change current work folder.")]
        private CommandExecuteResult commandChangeFolder(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (argumentQueue.Count == 0)
                return new CommandExecuteResult(ResultType.Success, "");
            else
            {
                string folderName = argumentQueue.PopArgumentString();
                KartStorageFolder? changeFolder = _currentFolder.GetFolder(folderName);
                if (changeFolder is null)
                    return new CommandExecuteResult(ResultType.Failure, $"Cannot found folder: \"{folderName}\"");
                _currentFolder = changeFolder;
                return new CommandExecuteResult(ResultType.Success, "");
            }
        }

        [CommandAutoComplete("cd")]
        private string[] commandAutoComplChangeFolder(CommandArgumentQueue argumentQueue)
        {
            List<string> suggestions = new List<string>();
            if (_storageSystem is null || !_storageSystem.IsInitialized)
                return Array.Empty<string>();
            if (_currentFolder is null)
                _currentFolder = _storageSystem.RootFolder;
            if (argumentQueue.Count > 0)
            {
                string findFolderName = argumentQueue.PopArgumentString();
                foreach (KartStorageFolder folder in _currentFolder.Folders)
                    if (folder.Name.StartsWith(findFolderName))
                        suggestions.Add(folder.Name);
            }
            else
            {
                foreach (KartStorageFolder folder in _currentFolder.Folders)
                    suggestions.Add(folder.Name);
            }
            return suggestions.ToArray();
        }

        [Command("pwd", "Print work folder.")]
        private CommandExecuteResult commandPrintWorkFolder(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (_storageSystem is null || !_storageSystem.IsInitialized)
                return new CommandExecuteResult(ResultType.Failure, "KartStorageSystem has not been initialized.");
            if (_currentFolder is null)
                _currentFolder = _storageSystem.RootFolder;
            commandConsole.WriteLine($"\nWork folder: {_currentFolder.FullName}\n");
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [Command("open", "Open specific file")]
        private CommandExecuteResult commandOpen(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (_storageSystem is null || !_storageSystem.IsInitialized)
                return new CommandExecuteResult(ResultType.Failure, "KartStorageSystem has not been initialized.");
            if (_currentFolder is null)
                _currentFolder = _storageSystem.RootFolder;
            string fileName = argumentQueue.PopArgumentString();
            KartStorageFile? file = _currentFolder.GetFile(fileName);
            if (file is null)
                return new CommandExecuteResult(ResultType.Failure, $"Cannot found file: {fileName}.");
            if(!file.HasDataSource)
                return new CommandExecuteResult(ResultType.Failure, $"file: {fileName} has no any data source.");
            string tmpFileName = $"{Path.GetTempFileName()}_{file.Name}";
            using (FileStream outFileStream = new FileStream(tmpFileName, FileMode.Create))
            {
                file?.WriteTo(outFileStream);
            }
            if (OperatingSystem.IsWindows())
            {
                Process openFileProc = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "explorer",
                        Arguments = $"\"{tmpFileName}\""
                    }
                };
                openFileProc.Start();
                _openedTmpFiles.Add(tmpFileName, openFileProc);
            }
            else if(OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                Process openFileProc = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "open",
                        Arguments = $"\"{tmpFileName}\"",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                    }
                };
                openFileProc.Start();
                _openedTmpFiles.Add(tmpFileName, openFileProc);
            }
            else
            {
                return new CommandExecuteResult(ResultType.Failure, $"This command is not available on {Environment.OSVersion.VersionString}");
            }
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [CommandAutoComplete("open")]
        private string[] commandAutoComplOpen(CommandArgumentQueue argumentQueue)
        {
            List<string> suggestions = new List<string>();
            if (_storageSystem is null || !_storageSystem.IsInitialized)
                return Array.Empty<string>();
            if (_currentFolder is null)
                _currentFolder = _storageSystem.RootFolder;
            if (argumentQueue.Count > 0)
            {
                string findFileName = argumentQueue.PopArgumentString();
                foreach (KartStorageFile file in _currentFolder.Files)
                    if (file.Name.StartsWith(findFileName))
                        suggestions.Add(file.Name);
            }
            else
            {
                foreach (KartStorageFile file in _currentFolder.Files)
                    suggestions.Add(file.Name);
            }
            return suggestions.ToArray();
        }

        [Command("try-allRho5", "")]
        private CommandExecuteResult commandTryAllRho5(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (_storageSystem is null)
                return new CommandExecuteResult(ResultType.Failure, "");
            if (!Directory.Exists("try-allRho5"))
                Directory.CreateDirectory("try-allRho5");
            int dataPackNum = 1;
            foreach(KartStorageFolder folder in _storageSystem.RootFolder.Folders)
            {
                Rho5Archive rho5Archive = new Rho5Archive();
                Rho5Folder mountFolder = new Rho5Folder();
                mountFolder.Name = folder.Name;
                rho5Archive.RootFolder.AddFolder(mountFolder);
                Queue<(Rho5Folder, KartStorageFolder)> queue = new Queue<(Rho5Folder, KartStorageFolder)>();
                queue.Enqueue((mountFolder, folder));
                while(queue.Count > 0)
                {
                    (Rho5Folder, KartStorageFolder) curEle = queue.Dequeue();
                    foreach(KartStorageFolder subFolder in curEle.Item2.Folders)
                    {
                        Rho5Folder newSubRho5Folder = new Rho5Folder();
                        newSubRho5Folder.Name = subFolder.Name;
                        curEle.Item1.AddFolder(newSubRho5Folder);
                        queue.Enqueue((newSubRho5Folder, subFolder));
                    }
                    foreach(KartStorageFile file in curEle.Item2.Files)
                    {
                        if (!file.HasDataSource)
                            return new CommandExecuteResult(ResultType.Failure, "");
                        byte[] data = file.GetBytes();
                        Rho5File newRho5File = new Rho5File();
                        newRho5File.Name = file.Name;
                        newRho5File.DataSource = new ByteArrayDataSource(data);
                        curEle.Item1.AddFile(newRho5File);
                    }
                }
                rho5Archive.Save("try-allRho5", $"DataPack{dataPackNum}", _region, SavePattern.AlwaysRegeneration);
                rho5Archive.Dispose();
                rho5Archive = new Rho5Archive();
                rho5Archive.Open("try-allRho5", $"DataPack{dataPackNum}", _region);
                rho5Archive.Dispose();
                dataPackNum++;
            }
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [Command("try-track", "Try to open track model.")]
        private CommandExecuteResult commandTryTrack(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (_storageSystem is null || !_storageSystem.IsInitialized)
                return new CommandExecuteResult(ResultType.Failure, "KartStorageSystem has not been initialized.");
            if (_currentFolder is null)
                _currentFolder = _storageSystem.RootFolder;

            string track = argumentQueue.PopArgumentString();
            KartStorageFolder? trackFolder = _storageSystem.GetFolder("track_");
            if (trackFolder is null)
                return new CommandExecuteResult(ResultType.Failure, "Cannot found track folder.");
            KartStorageFile? file = trackFolder.GetFile($"{track}/track.1s");
            if (file is null)
                return new CommandExecuteResult(ResultType.Failure, $"Cannot found track: {track}.");
            if (!file.HasDataSource)
                return new CommandExecuteResult(ResultType.Failure, $"file: {file.FullName} has no any data source.");
            using (Stream stream = file.CreateStream())
            {
                BinaryReader reader = new BinaryReader(stream);
                Dictionary<short, KartObject> decodedKartObjectMap = new Dictionary<short, KartObject>();
                Dictionary<short, object> decodedFieldMap = new Dictionary<short, object>();

                TrackContainer trackContainer = reader.ReadKartObject<TrackContainer>(decodedKartObjectMap, decodedFieldMap);
                Relement rootRelement = trackContainer.TrackScene;
                
                string relementStr = rootRelement.ToString();

                if (!Directory.Exists("try"))
                    Directory.CreateDirectory("try");
                if (!Directory.Exists("try\\tracks"))
                    Directory.CreateDirectory("try\\tracks");
                if (!Directory.Exists($"try\\tracks\\{track}"))
                    Directory.CreateDirectory($"try\\tracks\\{track}");
                if (!Directory.Exists($"try\\tracks\\{track}\\textures"))
                    Directory.CreateDirectory($"try\\tracks\\{track}\\textures");
                using (FileStream outFileStream = new FileStream($"try\\tracks\\{track}\\{track}.xml", FileMode.Create))
                {
                    byte[] relementStrBin = Encoding.UTF8.GetBytes(relementStr);
                    outFileStream.Write(relementStrBin);
                }

                List<MeshObj> meshObjs = new List<MeshObj>();
                convertRelementToObj(rootRelement, Matrix4x4.CreateLookAt(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1)), meshObjs);
                generateTrackWavefrontObj($"try\\tracks\\{track}", track, meshObjs);
            }
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [CommandAutoComplete("try-track")]
        private string[] commandAutoComplTryTrack(CommandArgumentQueue argumentQueue)
        {
            List<string> suggestions = new List<string>();
            if (_storageSystem is null || !_storageSystem.IsInitialized)
                return Array.Empty<string>();
            KartStorageFolder? trackFolder = _storageSystem.GetFolder("track_");
            if (trackFolder is null)
                return Array.Empty<string>();
            if (argumentQueue.Count > 0)
            {
                string findTrackName= argumentQueue.PopArgumentString();
                foreach (KartStorageFolder folder in trackFolder.Folders)
                    if (folder.Name.StartsWith(findTrackName))
                        suggestions.Add(folder.Name);
            }
            else
            {
                foreach (KartStorageFolder folder in trackFolder.Folders)
                    suggestions.Add(folder.Name);
            }
            
            return suggestions.ToArray();
        }

        [Command("try-kart", "Try to dump kart model.")]
        private CommandExecuteResult commandTryKart(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (_storageSystem is null || !_storageSystem.IsInitialized)
                return new CommandExecuteResult(ResultType.Failure, "KartStorageSystem has not been initialized.");
            if (_currentFolder is null)
                _currentFolder = _storageSystem.RootFolder;

            string kart = argumentQueue.PopArgumentString();
            KartStorageFolder? trackFolder = _storageSystem.GetFolder("kart_");
            if (trackFolder is null)
                return new CommandExecuteResult(ResultType.Failure, "Cannot found track folder.");
            KartStorageFile? file = trackFolder.GetFile($"{kart}/model.1s");
            if (file is null)
                return new CommandExecuteResult(ResultType.Failure, $"Cannot found kart: {kart}.");
            if (!file.HasDataSource)
                return new CommandExecuteResult(ResultType.Failure, $"file: {file.FullName} has no any data source.");
            using (Stream stream = file.CreateStream())
            {
                BinaryReader reader = new BinaryReader(stream);
                Dictionary<short, KartObject> decodedKartObjectMap = new Dictionary<short, KartObject>();
                Dictionary<short, object> decodedFieldMap = new Dictionary<short, object>();

                ReKart reKart = reader.ReadKartObject<ReKart>(decodedKartObjectMap, decodedFieldMap);
                string relementStr = reKart.ToString();

                if (!Directory.Exists("try"))
                    Directory.CreateDirectory("try");
                if (!Directory.Exists("try\\karts"))
                    Directory.CreateDirectory("try\\karts");
                if (!Directory.Exists($"try\\karts\\{kart}"))
                    Directory.CreateDirectory($"try\\karts\\{kart}");
                if (!Directory.Exists($"try\\karts\\{kart}\\textures"))
                    Directory.CreateDirectory($"try\\karts\\{kart}\\textures");
                using (FileStream outFileStream = new FileStream($"try\\karts\\{kart}\\{kart}.xml", FileMode.Create))
                {
                    byte[] relementStrBin = Encoding.UTF8.GetBytes(relementStr);
                    outFileStream.Write(relementStrBin);
                }

                List<MeshObj> meshObjs = new List<MeshObj>();
                convertRelementToObj(reKart, Matrix4x4.Identity, meshObjs);
                generateKartWavefrontObj($"try\\karts\\{kart}", kart, meshObjs);
            }
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [CommandAutoComplete("try-kart")]
        private string[] commandAutoComplTryKart(CommandArgumentQueue argumentQueue)
        {
            List<string> suggestions = new List<string>();
            if (_storageSystem is null || !_storageSystem.IsInitialized)
                return Array.Empty<string>();
            KartStorageFolder? kartFolder = _storageSystem.GetFolder("kart_");
            if (kartFolder is null)
                return Array.Empty<string>();
            if (argumentQueue.Count > 0)
            {
                string findKartName = argumentQueue.PopArgumentString();
                foreach (KartStorageFolder folder in kartFolder.Folders)
                    if (folder.Name.StartsWith(findKartName))
                        suggestions.Add(folder.Name);
            }
            else
            {
                foreach (KartStorageFolder folder in kartFolder.Folders)
                    suggestions.Add(folder.Name);
            }

            return suggestions.ToArray();
        }

        private void convertRelementToObj(Relement relement, Matrix4x4 transform, List<MeshObj> meshObjs)
        {
            transform = Matrix4x4.CreateScale(relement.Scale) * relement.Transform * Matrix4x4.CreateTranslation(relement.Position) * transform;
            if(relement is ReTriList reTriList)
            {
                MeshObj newObj = new MeshObj();
                newObj.Name = relement.Name;
                foreach (Vector3 point in reTriList.Vertex.Vertices)
                    newObj.Vertices.Add(Vector3.Transform(point, transform));
                foreach (Vector2 texCoord in reTriList.Vertex.TextureUVs)
                    newObj.TexCoord.Add(new Vector3(texCoord.X, texCoord.Y, 0));
                for(int i = 0; i < reTriList.Vertex.Indexes.Length; i += 3)
                {
                    MeshFace face = new MeshFace();
                    face.TexCoordIndexes = new int[3];
                    face.VertexIndexes[0] = face.TexCoordIndexes[0] = reTriList.Vertex.Indexes[i];
                    face.VertexIndexes[1] = face.TexCoordIndexes[1] = reTriList.Vertex.Indexes[i + 1];
                    face.VertexIndexes[2] = face.TexCoordIndexes[2] = reTriList.Vertex.Indexes[i + 2];
                    newObj.Faces.Add(face);
                }
                if (reTriList.Tex is not null && reTriList.Tex.u3 is not null)
                    newObj.Merterial.TextureName = reTriList.Tex.u3;
                meshObjs.Add(newObj);
            }
            else if (relement is ReTriStrip reTriStrip)
            {
                MeshObj newObj = new MeshObj();
                newObj.Name = relement.Name;
                foreach (Vector3 point in reTriStrip.Vertex.Vertices)
                    newObj.Vertices.Add(Vector3.Transform(point, transform));
                foreach (Vector2 texCoord in reTriStrip.Vertex.TextureUVs)
                    newObj.TexCoord.Add(new Vector3(texCoord.X, texCoord.Y, 0));
                for (int i = 2; i < reTriStrip.Vertex.Indexes.Length; i ++)
                {
                    MeshFace face = new MeshFace();
                    face.TexCoordIndexes = new int[3];
                    face.VertexIndexes[0] = face.TexCoordIndexes[0] = reTriStrip.Vertex.Indexes[i - 2];
                    face.VertexIndexes[1] = face.TexCoordIndexes[1] = reTriStrip.Vertex.Indexes[i - 1];
                    face.VertexIndexes[2] = face.TexCoordIndexes[2] = reTriStrip.Vertex.Indexes[i];
                    newObj.Faces.Add(face);
                }
                if (reTriStrip.Tex is not null && reTriStrip.Tex.u3 is not null)
                    newObj.Merterial.TextureName = reTriStrip.Tex.u3;
                meshObjs.Add(newObj);
            }
            else if(relement is ReToonRigid reToonRigid)
            {
                MeshObj newObj = new MeshObj();
                newObj.Name = relement.Name;
                foreach(Vector3 vertex in reToonRigid.Vertices)
                    newObj.Vertices.Add(Vector3.Transform(vertex, transform));
                foreach (Vector3 texCoord in reToonRigid.TexCoords)
                    newObj.TexCoord.Add(new Vector3(texCoord.X, texCoord.Y, 0));
                foreach (ReToonRigidMeshFace meshFace in reToonRigid.MeshFaces)
                {
                    MeshFace face = new MeshFace();
                    face.NormalVectorIndexes = new int[3];
                    face.TexCoordIndexes = new int[3];

                    face.VertexIndexes[0] = meshFace.VertexIndex1;
                    face.VertexIndexes[1] = meshFace.VertexIndex2;
                    face.VertexIndexes[2] = meshFace.VertexIndex3;

                    face.NormalVectorIndexes[0] = meshFace.NormalVectorIndex1;
                    face.NormalVectorIndexes[1] = meshFace.NormalVectorIndex2;
                    face.NormalVectorIndexes[2] = meshFace.NormalVectorIndex3;

                    face.TexCoordIndexes[0] = meshFace.TexCoordIndex1;
                    face.TexCoordIndexes[1] = meshFace.TexCoordIndex2;
                    face.TexCoordIndexes[2] = meshFace.TexCoordIndex3;
                    newObj.Faces.Add(face);
                }
                meshObjs.Add(newObj);
            }
            foreach (Relement child in relement)
                convertRelementToObj(child, transform, meshObjs);
        }

        private void generateTrackWavefrontObj(string path, string trackName, List<MeshObj> objs)
        {
            string trackTheme = trackName.Split('_')[0];
            string[] textureSourcePaths = new[] { $"track_/{trackName}" , $"theme_/{trackTheme}/texture", $"theme_/common/texture", "zeta_/kr/ppl/ingame/20221222_facelift/village" };
            StringBuilder objBuilder = new StringBuilder();
            StringBuilder mtlBuilder = new StringBuilder();
            List<KartStorageFolder> textureSourceFolder = new List<KartStorageFolder>();
            foreach(string textureSourcePath in textureSourcePaths)
            {
                KartStorageFolder? folder = _storageSystem.GetFolder(textureSourcePath);
                if (folder is not null)
                    textureSourceFolder.Add(folder);
            }
            objBuilder.AppendLine($"mtllib {trackName}.mtl");
            foreach(MeshObj obj in objs)
            {
                objBuilder.AppendLine($"# Relement: {obj.Name}");
                objBuilder.AppendLine($"o {obj.Name}");
                foreach(Vector3 point in obj.Vertices)
                    objBuilder.AppendLine($"v {point.X:0.0000} {point.Y:0.0000} {point.Z:0.0000}");
                foreach (Vector3 texCoord in obj.TexCoord)
                    objBuilder.AppendLine($"vt {texCoord.X:0.0000} {texCoord.Y:0.0000}");
                if(obj.Merterial.TextureName is not null)
                    objBuilder.AppendLine($"usemtl merterial_{obj.Name}");
                objBuilder.AppendLine($"s 0");
                foreach(MeshFace face in obj.Faces)
                    objBuilder.AppendLine($"f {face.VertexIndexes[0] + 1}/{face.VertexIndexes[0] + 1} {face.VertexIndexes[1] + 1}/{face.VertexIndexes[1] + 1} {face.VertexIndexes[2] + 1}/{face.VertexIndexes[2] + 1}");

                if (obj.Merterial.TextureName is not null)
                {
                    mtlBuilder.AppendLine($"newmtl merterial_{obj.Name}");
                    mtlBuilder.AppendLine($"Ka 1.000 1.000 1.000");
                    mtlBuilder.AppendLine($"Kd 1.000 1.000 1.000");
                    mtlBuilder.AppendLine($"Ks 1.000 1.000 1.000");
                    mtlBuilder.AppendLine($"d 1.0");
                    mtlBuilder.AppendLine($"illum 2");
                    KartStorageFile? texFile = null;
                    foreach(KartStorageFolder folder in textureSourceFolder)
                        foreach(KartStorageFile file in folder.Files)
                            if(file.NameWithoutExt == obj.Merterial.TextureName)
                            {
                                texFile = file;
                                break;
                            }
                    if(texFile is null || !texFile.HasDataSource)
                    {
                        mtlBuilder.AppendLine($"# cannot found: {obj.Merterial.TextureName}");
                    }
                    else
                    {
                        using (FileStream outStream = new FileStream($"{path}\\textures\\{texFile.Name}", FileMode.Create))
                        {
                            texFile.WriteTo(outStream);
                        }
                        mtlBuilder.AppendLine($"map_Ka textures/{texFile.Name}");
                        mtlBuilder.AppendLine($"map_Kd textures/{texFile.Name}");
                        mtlBuilder.AppendLine($"map_Ks textures/{texFile.Name}");
                    }
                }
            }
            string objContent = objBuilder.ToString();
            string mtlContent = mtlBuilder.ToString();
            using(FileStream objStream = new FileStream($"{path}\\{trackName}.obj", FileMode.Create))
            {
                objStream.Write(Encoding.UTF8.GetBytes(objContent));
            }
            using (FileStream mtlStream = new FileStream($"{path}\\{trackName}.mtl", FileMode.Create))
            {
                mtlStream.Write(Encoding.UTF8.GetBytes(mtlContent));
            }
        }

        private void generateKartWavefrontObj(string path, string kartName, List<MeshObj> objs)
        {
            string[] textureSourcePaths = new[] { $"kart_/{kartName}" };
            StringBuilder objBuilder = new StringBuilder();
            StringBuilder mtlBuilder = new StringBuilder();
            List<KartStorageFolder> textureSourceFolder = new List<KartStorageFolder>();
            foreach (string textureSourcePath in textureSourcePaths)
            {
                KartStorageFolder? folder = _storageSystem.GetFolder(textureSourcePath);
                if (folder is not null)
                    textureSourceFolder.Add(folder);
            }
            objBuilder.AppendLine($"mtllib {kartName}.mtl");

            mtlBuilder.AppendLine($"newmtl merterial_{kartName}");
            mtlBuilder.AppendLine($"Ka 1.000 1.000 1.000");
            mtlBuilder.AppendLine($"Kd 1.000 1.000 1.000");
            mtlBuilder.AppendLine($"Ks 1.000 1.000 1.000");
            mtlBuilder.AppendLine($"d 1.0");
            mtlBuilder.AppendLine($"illum 2");
            KartStorageFile? texFile = null;
            foreach (KartStorageFolder folder in textureSourceFolder)
                foreach (KartStorageFile file in folder.Files)
                    if (file.NameWithoutExt == "1")
                    {
                        texFile = file;
                        break;
                    }
            if (texFile is null || !texFile.HasDataSource)
            {
                mtlBuilder.AppendLine($"# cannot found: 0");
            }
            else
            {
                using (FileStream outStream = new FileStream($"{path}\\textures\\{texFile.Name}", FileMode.Create))
                {
                    texFile.WriteTo(outStream);
                }
                mtlBuilder.AppendLine($"map_Ka textures/{texFile.Name}");
                mtlBuilder.AppendLine($"map_Kd textures/{texFile.Name}");
                mtlBuilder.AppendLine($"map_Ks textures/{texFile.Name}");
            }
            foreach (MeshObj obj in objs)
            {
                objBuilder.AppendLine($"# Relement: {obj.Name}");
                objBuilder.AppendLine($"o {obj.Name}");
                foreach (Vector3 point in obj.Vertices)
                    objBuilder.AppendLine($"v {point.X:0.0000} {point.Y:0.0000} {point.Z:0.0000}");
                foreach (Vector3 texCoord in obj.TexCoord)
                    objBuilder.AppendLine($"vt {texCoord.X:0.0000} {texCoord.Y:0.0000} {texCoord.Z:0.0000}");
                objBuilder.AppendLine($"usemtl merterial_{kartName}");
                objBuilder.AppendLine($"s 0");
                foreach (MeshFace face in obj.Faces)
                {
                    objBuilder.Append("f");
                    for(int i = 0; i < 3; i++)
                    {
                        int vertexIndex = face.VertexIndexes[i] + 1;
                        int? normIndex = face.NormalVectorIndexes?[i] + 1;
                        int? texIndex = face.TexCoordIndexes?[i] + 1;
                        objBuilder.Append($" {vertexIndex}");

                        if (texIndex is not null && texIndex > 0)
                            objBuilder.Append($"/{texIndex}");
                        else
                            objBuilder.Append($"/");

                        if (normIndex is not null && normIndex > 0)
                            objBuilder.Append($"/{normIndex}");
                        else
                            objBuilder.Append($"");

                    }
                    objBuilder.AppendLine("");
                }

            }
            string objContent = objBuilder.ToString();
            string mtlContent = mtlBuilder.ToString();
            using (FileStream objStream = new FileStream($"{path}\\{kartName}.obj", FileMode.Create))
            {
                objStream.Write(Encoding.UTF8.GetBytes(objContent));
            }
            using (FileStream mtlStream = new FileStream($"{path}\\{kartName}.mtl", FileMode.Create))
            {
                mtlStream.Write(Encoding.UTF8.GetBytes(mtlContent));
            }
        }

        private class MeshObj
        {
            public string Name = "";
            public List<Vector3> Vertices = new List<Vector3>();
            public List<Vector3> TexCoord = new List<Vector3>();
            public List<MeshFace> Faces = new List<MeshFace>();
            public Merterial Merterial = new Merterial();
        }

        private class MeshFace
        {
            public int[] VertexIndexes = new int[3];
            public int[]? NormalVectorIndexes;
            public int[]? TexCoordIndexes;
        }



        public class Merterial
        {
            public string? TextureName;
        }
    }
}
