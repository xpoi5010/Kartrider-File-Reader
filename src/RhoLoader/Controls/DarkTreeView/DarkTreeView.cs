using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RhoLoader.Controls
{
    public class DarkTreeView : TreeView
    {
        public new TreeViewDrawMode DrawMode { get => base.DrawMode; set { } }
        public DarkTreeView() : base()
        {
            base.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            DrawNode += DarkTreeView_DrawNode;
            BeforeExpand += DarkTreeView_BeforeExpand;
            BeforeCollapse += DarkTreeView_BeforeCollapse;

            ShowPlusMinus = false;
            ItemHeight = 20;
            FullRowSelect = true;
        }
        //Reference from https://stackoverflow.com/questions/10362988/treeview-flickering
        //To solve treeview flickering issue.
        [DllImport("User32.dll")]
        private static extern nint SendMessage(nint hWnd, int msg, nint wp, nint lp);
        private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        private const int TVS_EX_DOUBLEBUFFER = 0x0004;
        protected override void OnHandleCreated(EventArgs e)
        {
            SendMessage(Handle, TVM_SETEXTENDEDSTYLE, TVS_EX_DOUBLEBUFFER, TVS_EX_DOUBLEBUFFER);
            base.OnHandleCreated(e);
        }

        private void DarkTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (!expandByCustom)
                e.Cancel = true;
        }

        private void DarkTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (!expandByCustom)
                e.Cancel = true;
        }

        private void DarkTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            TreeView baseTreeView = (TreeView)sender;
            Graphics baseGraph = e.Graphics;
            Rectangle baseRectangle = e.Bounds;
            int baseX = e.Node.Level * 18 + baseRectangle.X;
            Rectangle baseIconRange = GetExpandIconRange(e.Node, baseRectangle.X);
            //string[] debug__ = new string[] { "file_image.png", "file_ksv.png", "file_music.png", "file_xml.png", "folder_close.png", "folder_open.png", "object_unknown.png", "relement_base.png", "relement_recamera.png"};

            Brush strBrush = Brushes.Black;
            bool whiteIcon = false;

            if (e.Node.Parent != null && e.Node.Bounds.Y <= e.Node.Parent.Bounds.Y)
            {
                baseRectangle = new Rectangle(0, e.Node.Parent.Bounds.Y + (e.Node.Index + 1) * 18, e.Bounds.Width, e.Bounds.Height);
            }
            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                if (Focused)
                {
                    //2B4865
                    baseGraph.FillRectangle(new SolidBrush(Color.FromArgb(36, 36, 36)), baseRectangle.X, baseRectangle.Y, baseRectangle.Width, baseRectangle.Height);// Border
                    baseGraph.FillRectangle(new SolidBrush(Color.FromArgb(0x2B, 0x48, 0x65)), baseRectangle.X + 1, baseRectangle.Y + 1, baseRectangle.Width - 2, baseRectangle.Height - 2);// Background
                    strBrush = Brushes.White;
                    whiteIcon = true;
                }
                else
                {
                    baseGraph.FillRectangle(new SolidBrush(Color.FromArgb(0x2B, 0x48, 0x65)), baseRectangle.X, baseRectangle.Y, baseRectangle.Width, baseRectangle.Height);// Background
                    strBrush = Brushes.White;
                    whiteIcon = true;
                }
            }
            else
            {
                baseGraph.FillRectangle(new SolidBrush(BackColor), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }
            if (e.Node.Tag is NodeInfoContainer infoContainer)
                switch (infoContainer.NodeType)
                {
                    case NodeType.Folder:
                        DrawNodeIcon(baseGraph, baseX + 38, baseRectangle.Y, e.Node.IsExpanded ? "folder_open.png" : "folder_close.png");
                        break;
                    case NodeType.MusicFile:
                        DrawNodeIcon(baseGraph, baseX + 38, baseRectangle.Y, "file_music.png");
                        break;
                    case NodeType.ImageFile:
                        DrawNodeIcon(baseGraph, baseX + 38, baseRectangle.Y, "file_image.png");
                        break;
                    case NodeType.XML:
                        DrawNodeIcon(baseGraph, baseX + 38, baseRectangle.Y, "file_xml.png");
                        break;
                    case NodeType.PlayRecord:
                        DrawNodeIcon(baseGraph, baseX + 38, baseRectangle.Y, "file_ksv.png");
                        break;
                    case NodeType.Texture:
                        DrawNodeIcon(baseGraph, baseX + 38, baseRectangle.Y, "file_image.png");
                        break;
                    default:
                        DrawNodeIcon(baseGraph, baseX + 38, baseRectangle.Y, "object_unknown.png");
                        break;
                }
            else
                DrawNodeIcon(baseGraph, baseX + 38, baseRectangle.Y, "object_unknown.png");
            baseGraph.DrawString(e.Node.Text, baseTreeView.Font, strBrush, new Point(baseX + 60, baseRectangle.Y + 1));
            if (e.Node.Nodes.Count > 0)
                if (e.Node.IsExpanded)
                    DrawExpand(baseGraph, baseIconRange, whiteIcon);
                else
                    DrawCollapsed(baseGraph, baseIconRange, whiteIcon);
        }

        private bool expandByCustom = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            TreeNode clickNode = GetNodeAt(e.X, e.Y);
            if (clickNode is not null)
            {
                if (clickNode.Nodes.Count > 0)
                {
                    Rectangle baseRange = GetExpandIconRange(clickNode, Bounds.X);
                    if (baseRange.Contains(e.X, e.Y))
                    {
                        expandByCustom = true;
                        if (clickNode.IsExpanded)
                            clickNode.Collapse();
                        else
                            clickNode.Expand();
                        expandByCustom = false;
                    }
                    else
                        SelectedNode = clickNode;
                }
                else
                    SelectedNode = clickNode;
            }
            else
                SelectedNode = clickNode;
            base.OnMouseDown(e);
        }



        private Stream LoadResource(string resourcePath)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream(resourcePath);
        }

        private void DrawExpand(Graphics graphics, Rectangle range, bool white = false)
        {
            using (Stream stream = LoadResource($"RhoLoader.Controls.DarkTreeView.Icons.expanded{(white ? "_white" : "")}.png"))
            {
                Bitmap bmp = new Bitmap(stream);
                graphics.DrawImage(bmp, range);
                bmp.Dispose();
            }
        }

        private void DrawCollapsed(Graphics graphics, Rectangle range, bool white = false)
        {
            using (Stream stream = LoadResource($"RhoLoader.Controls.DarkTreeView.Icons.collapsed{(white ? "_white" : "")}.png"))
            {
                Bitmap bmp = new Bitmap(stream);
                graphics.DrawImage(bmp, range);
                bmp.Dispose();
            }
        }

        private void DrawNodeIcon(Graphics graphics, int baseX, int baseY, string IconName)
        {
            using (Stream stream = LoadResource($"RhoLoader.Controls.DarkTreeView.Icons.{IconName}"))
            {
                Bitmap bmp = new Bitmap(stream);
                Rectangle range = new Rectangle(baseX + (18 - bmp.Width >> 1), baseY + (ItemHeight - bmp.Height >> 1), bmp.Width, 14);
                graphics.DrawImage(bmp, range);
                bmp.Dispose();
            }
        }

        private Rectangle GetExpandIconRange(TreeNode node, int offsetX)
        {
            int baseX = node.Level * 18 + node.Bounds.X - (node.Level * 19 + 22);
            return new Rectangle(baseX + 20, node.Bounds.Y + (ItemHeight - 8 >> 1), 8, 8);
        }
    }
}
