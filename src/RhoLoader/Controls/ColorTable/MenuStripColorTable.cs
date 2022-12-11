using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoLoader.Controls.ColorTable
{
    public class MenuStripColorTable: ProfessionalColorTable
    {
        public override Color ToolStripBorder
        {
            get { return Color.FromArgb(0, 0, 0); }
        }
        
        public override Color ToolStripDropDownBackground
        {
            get { return Color.FromArgb(64,64,64); }
        }
        
        public override Color ToolStripGradientBegin
        {
            get { return Color.FromArgb(64, 64, 64); }
        }
        public override Color ToolStripGradientEnd
        {
            get { return Color.FromArgb(64, 64, 64); }
        }
        public override Color ToolStripGradientMiddle
        {
            get { return Color.FromArgb(64, 64, 64); }
        }
        public override Color ToolStripContentPanelGradientBegin
        {
            get { return Color.FromArgb(64, 64, 64); }
        }

        public override Color ToolStripContentPanelGradientEnd
        {
            get { return Color.FromArgb(64, 64, 64); }
        }

        public override Color ToolStripPanelGradientBegin
        {
            get { return Color.FromArgb(64, 64, 64); }
        }

        public override Color ToolStripPanelGradientEnd
        {
            get { return Color.FromArgb(64, 64, 64); }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.FromArgb(45, 45, 45); }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.FromArgb(38, 38, 38); }
        }

        public override Color MenuItemBorder
        {
            get { return Color.FromArgb(203,203,213); }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.FromArgb(32,32,32); }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.FromArgb(35,35,35); }
        }

        public override Color SeparatorDark
        {
            get { return Color.FromArgb(35, 35, 35); }
        }

        public override Color SeparatorLight
        {
            get { return Color.FromArgb(35, 35, 35); }
        }
    }
}
