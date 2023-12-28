using System.Linq;
using System.Threading.Tasks;
using KartCityStudio.Game.Graphics.UserInterface;
using KartCityStudio.Game.IO.Stores;
using KartLibrary.File;
using ManagedBass.Fx;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Screens;
using osuTK.Graphics;

namespace KartCityStudio.Game
{
    public partial class MainScreen : Screen
    {
        private KCSMenu mainMenubar;
        private DrawableTrack drawableTrack;
        private SpinningBox spinningBox;
        private Track track;
        private Container lineBoxes;
        private CursorContainer cursorContainer;

        [BackgroundDependencyLoader]
        private void load(AudioManager audioManager)
        {
            InternalChildren = new Drawable[]
            {
                new Container()
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.X,
                    Height = 30,
                    Child = mainMenubar = new KCSMenu()
                    {
                        RelativeSizeAxes = Axes.Both,
                        Items = new[]
                        {
                            new MenuItem("File", () => { })
                            {
                                Items = new[]
                                {
                                    new MenuItem("Open", () =>
                                    {
                                        
                                    }),
                                    new MenuItem("Open Data folder", () => { }),
                                    new MenuItem("Save", () => { }),
                                    new MenuItem("Save As", () => { }),
                                    new MenuItem("Save Workspace", () => { }),
                                    new KCSMenuItemSpacer(),
                                    new MenuItem("Exit", () => { }),
                                }
                            },
                            new MenuItem("Edit")
                            {
                                Items = new[]
                                {
                                    new MenuItem("Add File"),
                                    new MenuItem("Remove All"),
                                }
                            },
                            new MenuItem("Extract")
                            {
                                Items = new[]
                                {
                                    new MenuItem("Extract selected file"),
                                    new MenuItem("Extract current folder"),
                                    new MenuItem("Extract all"),
                                }
                            },
                            new MenuItem("About")
                            {
                                Items = new[]
                                {
                                    new MenuItem("Checks updates"),
                                    new MenuItem("Bug Report"),
                                    new MenuItem("About"),
                                }
                            },
                        }
                    }
                },
                new SpriteText
                {
                    Y = 20,
                    Text = "Main Screen",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40)
                },
                spinningBox = new SpinningBox
                {
                    Anchor = Anchor.Centre,
                }
            };
        }
        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();
            //double dur = Time.Current - prevUpdate;
            //float ddur = 10;
            //if(dur >= ddur)
            //{
            //    for(int i = 0; i < 256; i++)
            //    {
            //        float freq = track?.CurrentAmplitudes.FrequencyAmplitudes.Span[i] ?? 0;
            //        if (lineBoxes[i].Height < freq)
            //        {
            //            lineBoxes[i].ClearTransforms();
            //            lineBoxes[i].Height = freq;

            //            lineBoxes[i].FadeIn().ResizeHeightTo(0, ddur * 30, Easing.None);
            //        }
            //    }
            //    prevUpdate = Time.Current;
            //}
        }
    }
}
