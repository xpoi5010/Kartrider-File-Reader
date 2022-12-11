using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoLoader.Controls
{
    public class NodeInfoContainer
    {
        public NodeInfoContainer(NodeType nodeType, object baseData)
        {
            NodeType = nodeType;
            BaseData = baseData;
        }
        public NodeType NodeType { get; init; }
        public object BaseData { get; init; }
    }

    public enum NodeType
    {
        ImageFile,
        Folder,
        TrackData,
        Relement,
        PlayRecord,
        XML,
        MusicFile,
        Texture,
        Expanding,
        UnknownFile
    }
}
