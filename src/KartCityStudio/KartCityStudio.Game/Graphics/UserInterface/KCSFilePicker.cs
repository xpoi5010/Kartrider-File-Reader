using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KartCityStudio.Game.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSFilePicker: CompositeDrawable
    {
        private readonly Container contentContainer;
        private readonly Box background;
        private readonly KCSSplittableContainer splittableContainer;

        private readonly KCSToolboxGroup volumesGroup;
        private readonly KCSListBox volumesListBox;
        private readonly KCSToolboxGroup systemGroup;
        private readonly KCSListBox systemListBox;

        private readonly Container rightPanelContainer;
        private readonly FillFlowContainer fileListBtnContainer;
        private readonly KCSTextbox currentPathTextbox;
        private readonly KCSTextbox searchingTextbox;
        private readonly KCSButton backBtn;
        private readonly KCSButton nextBtn;
        private readonly KCSButton backToParentBtn;

        private readonly KCSListView fileListView;

        private string currentPath;

        private System.ComponentModel.BackgroundWorker initializeRightSideWorker;
        private System.ComponentModel.BackgroundWorker getDirectoryItemsWorker;

        

        public KCSFilePicker()
        {
            currentPath = Environment.CurrentDirectory;

            InternalChild = contentContainer = new Container()
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    background = new Box()
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.Transparent
                    },
                    splittableContainer = new KCSSplittableContainer(Direction.Horizontal)
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                }
            };
            splittableContainer.FirstContainer.Add(new FillFlowContainer()
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding() { Vertical = 7, Horizontal = 7 },
                Spacing = new osuTK.Vector2(3),
                Children = new Drawable[]
                {
                    volumesGroup = new KCSToolboxGroup()
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        GroupText = "Volumes",
                        Masking = true,
                        CornerRadius = 5,
                        Child = volumesListBox = new KCSListBox()
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 80,
                            BackgroundColour = Colour4.FromHex("2A2A2A"),
                            CornerRadius = 5,
                        }
                    },
                    systemGroup = new KCSToolboxGroup()
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        GroupText = "System",
                        Masking = true,
                        CornerRadius = 5,
                        Child = systemListBox = new KCSListBox()
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 80,
                            BackgroundColour = Colour4.FromHex("2A2A2A"),
                            CornerRadius = 5,
                        }
                    }
                }
            });
            splittableContainer.SecondContainer.Add(rightPanelContainer = new Container()
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    fileListBtnContainer = new FillFlowContainer()
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 40f,
                        Direction = FillDirection.Horizontal,
                        Spacing = new osuTK.Vector2(5),
                        Children = new Drawable[]
                        {
                            new KCSButtonGroup(FillDirection.Horizontal)
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                RelativeSizeAxes = Axes.None,
                                Height = 27f,
                                Masking = true,
                                CornerRadius = 5,
                                Children = new KCSButton[]
                                {
                                    backBtn = new KCSIconButton()
                                    {
                                        RelativeSizeAxes = Axes.Y,
                                        Anchor = Anchor.TopLeft,
                                        Origin = Anchor.TopLeft,
                                        Width = 27f,
                                        IconRelativeSize = new osuTK.Vector2(0.4f),
                                        Icon = FontAwesome.Solid.ChevronLeft,
                                        BackgroundColour = Colour4.FromHex("2A2A2A"),
                                        HoverColour = Colour4.FromHex("3A3A3A3A"),
                                    },
                                    nextBtn = new KCSIconButton()
                                    {
                                        RelativeSizeAxes = Axes.Y,
                                        Anchor = Anchor.TopLeft,
                                        Origin = Anchor.TopLeft,
                                        Width = 27f,
                                        IconRelativeSize = new osuTK.Vector2(0.4f),
                                        Icon = FontAwesome.Solid.ChevronRight,
                                        BackgroundColour = Colour4.FromHex("2A2A2A"),
                                        HoverColour = Colour4.FromHex("3A3A3A3A"),
                                        Masking = true,
                                        CornerRadius = 0,
                                    },
                                    backToParentBtn = new KCSIconButton()
                                    {
                                        RelativeSizeAxes = Axes.Y,
                                        Anchor = Anchor.TopLeft,
                                        Origin = Anchor.TopLeft,
                                        Width = 27f,
                                        IconRelativeSize = new osuTK.Vector2(0.4f),
                                        Icon = FontAwesome.Solid.UndoAlt,
                                        BackgroundColour = Colour4.FromHex("2A2A2A"),
                                        HoverColour = Colour4.FromHex("3A3A3A3A"),
                                        Masking = true,
                                        CornerRadius = 0,
                                    }
                                }
                            },
                            currentPathTextbox = new KCSTextbox()
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                RelativeSizeAxes = Axes.X,
                                Width = 0.7f,
                                Height = 27f,
                            },
                            searchingTextbox = new KCSTextbox()
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                RelativeSizeAxes = Axes.X,
                                Width = 0.2f,
                                Height = 27f,
                            }
                        }
                    },
                    fileListView = new KCSListView()
                    {
                        RelativeSizeAxes = Axes.Both,
                        BackgroundColour = Colour4.FromHex("101010"),
                        HeaderColour = Colour4.FromHex("252525"),
                        Margin = new MarginPadding() { Top = 40f }
                    }
                }
            });

            fileListView.Headers.Add(new ListViewHeaderItem("Name", "Name", 0.50f));
            fileListView.Headers.Add(new ListViewHeaderItem("DateModified", "Date modified", 0.20f));
            fileListView.Headers.Add(new ListViewHeaderItem("Type", "Type", 0.15f));
            fileListView.Headers.Add(new ListViewHeaderItem("Size", "Size", 0.15f));

            initializeRightSideWorker = new System.ComponentModel.BackgroundWorker();
            initializeRightSideWorker.DoWork += initializeRightSide;
            initializeRightSideWorker.RunWorkerCompleted += initializeRightSideWorker_RunWorkerCompleted; ;
            initializeRightSideWorker.RunWorkerAsync();

            getDirectoryItemsWorker = new System.ComponentModel.BackgroundWorker();
            getDirectoryItemsWorker.DoWork += getDirectoryItems;
            getDirectoryItemsWorker.RunWorkerCompleted += getDirectoryItemsWorker_RunWorkerCompleted;

            getDirectoryItemsWorker.RunWorkerAsync();
        }

        private void initializeRightSide(object? sender, System.ComponentModel.DoWorkEventArgs e)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            e.Result = new Tuple<DriveInfo[]>(drives);

        }

        private void initializeRightSideWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error is not null)
            {

            }
            else
            {
                if(e.Result is Tuple <DriveInfo[]> result)
                {
                    Scheduler.Add(updateRightSide, result);
                }
            }
        }

        private void updateRightSide(Tuple<DriveInfo[]> result)
        {
            foreach(var drive in result.Item1)
            {
                string driveStr = $"";
                volumesListBox.Items.Add(new ListBoxItem($"{(drive.VolumeLabel == "" ? "Local Disk" : drive.VolumeLabel)}({drive.RootDirectory})"));
            }
        }

        private void getDirectoryItems(object? sender, System.ComponentModel.DoWorkEventArgs e)
        {
            DirectoryInfo currentDirectoryInfo = new DirectoryInfo(currentPath);
            if (!currentDirectoryInfo.Exists)
            {
                throw new DirectoryNotFoundException(currentPath);
            }
            else
            {
                e.Result = new Tuple<DirectoryInfo[], FileInfo[]>(currentDirectoryInfo.GetDirectories(), currentDirectoryInfo.GetFiles());
            }
        }

        private void getDirectoryItemsWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if(e.Error is not null)
            {

            }
            else
            {
                if(e.Result is Tuple<DirectoryInfo[], FileInfo[]> result)
                {
                    Scheduler.Add(updatesDirectoryList, result);
                }
            }
        }

        private void updatesDirectoryList(Tuple<DirectoryInfo[], FileInfo[]> result)
        {
            fileListView.Items.Clear();
                    foreach(DirectoryInfo subDir in result.Item1)
                    {
                        fileListView.Items.Add(
                            new ListViewItem(new (string key, osu.Framework.Localisation.LocalisableString value)[]
                            {
                                ("Name",  subDir.Name),
                                ("DateModified",  subDir.LastWriteTime.ToString()),
                                ("Type",  "Folder"),
                                ("Size",  ""),
                            }));
                    }
                    foreach (FileInfo file in result.Item2)
                    {
                        fileListView.Items.Add(
                            new ListViewItem(new (string key, osu.Framework.Localisation.LocalisableString value)[]
                            {
                                ("Name",  file.Name),
                                ("DateModified",  file.LastWriteTime.ToString()),
                                ("Type",  "File"),
                                ("Size",  Utilities.UnitUtility.FormatDataSize(file.Length)),
                            }));
                    }
        }

        private void showLoadingScreen()
        {

        }
    }
}
