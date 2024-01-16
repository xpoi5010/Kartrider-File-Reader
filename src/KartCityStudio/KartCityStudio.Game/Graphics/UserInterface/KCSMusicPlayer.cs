using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using KartCityStudio.Game.IO.Stores;
using KartLibrary.Consts;
using KartLibrary.Game.Engine.Track;
using KartLibrary.File;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osuTK;
using osuTK.Audio.OpenAL;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    public partial class KCSMusicPlayer : Container
    {
        private readonly Box background;
        private readonly SpriteText songTitle;
        private readonly SpriteText songFileName;
        private readonly SpriteText trackTime;
        private readonly KCSIconButton playButton;
        private readonly KCSIconButton repeatButton;
        private readonly KCSIconButton unknownButton;
        private readonly Container amplitudeBoxes;

        private KCSSliderBar<double> trackProgressBar;
        private DrawableTrack drawableTrack;
        private int amplitudeBoxesCount = 81;

        private Track track;

        private double prevBoxesUpdateTimestamp = 0;

        private bool unknwonMode = false;

        [Resolved]
        private KartStorageSystem storageSystem { get; set; }

        public KCSMusicPlayer()
        {
            Children = new Drawable[]
            {   background = new Box()
                {
                    RelativeSizeAxes = Axes.Both,
                },
                amplitudeBoxes = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new osuTK.Vector2(0.95f, 1),
                    ChildrenEnumerable = Enumerable
                                                .Range(0, amplitudeBoxesCount)
                                                .Select(x =>
                                                new Container()
                                                {
                                                    Anchor = Anchor.CentreLeft,
                                                    Origin = Anchor.Centre,
                                                    RelativeSizeAxes = Axes.Both,
                                                    RelativePositionAxes = Axes.Both,
                                                    X = (float)x / (amplitudeBoxesCount),
                                                    Width = 0.5f / (amplitudeBoxesCount),
                                                    Height = 0f,
                                                    Masking = true,
                                                    CornerRadius = 3f,
                                                    EdgeEffect = new osu.Framework.Graphics.Effects.EdgeEffectParameters()
                                                    {
                                                        Radius = 3.3f,
                                                        Colour = Colour4.FromHex("205E61"),
                                                        Type = osu.Framework.Graphics.Effects.EdgeEffectType.Shadow,
                                                        Hollow = true,
                                                    },
                                                    Child = new Box()
                                                    {
                                                        RelativeSizeAxes = Axes.Both,
                                                        Colour = Colour4.FromHex("205E61"),
                                                        Width = 1f,
                                                        Height = 1f,
                                                    }
                                                })
                },
                songTitle = new SpriteText()
                {
                    RelativePositionAxes = Axes.None,
                    Position = new osuTK.Vector2(38, 30),
                    Text = "城堡戰鬥舞步",
                    Font = KCSFont.Default.With(size: 55f),
                    Colour = Colour4.White,
                    Alpha = 0f,
                },
                songFileName = new SpriteText()
                {
                    RelativePositionAxes = Axes.None,
                    Position = new osuTK.Vector2(38, 83),
                    Text = "castle_01_re.ogg",
                    Font = KCSFont.Default.With(size: 18f),
                    Colour = Colour4.FromHex("FFFFFF"),
                    Alpha = 0f
                },
                trackTime = new SpriteText()
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    RelativePositionAxes = Axes.X,
                    Position = new osuTK.Vector2(0.05f, -40f),
                    Text = "",
                    Font = KCSFont.Default.With(size : 18f),
                    Colour = Colour4.FromHex("FFFFFF"),
                    Alpha = 0.8f
                },
                trackProgressBar = new KCSSliderBar<double>()
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    RelativePositionAxes = Axes.X,
                    X = 0,
                    Y = -30,
                    Width = 0.9f,
                    Height = 8,
                    MinValue = 0,
                    MaxValue = 100,
                    BackgroundColour = Colour4.FromHex("FFFFFF3A"),
                    ForegroundColour = Colour4.FromHex("FFFFFF"),
                },
                playButton = new KCSIconButton()
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.None,
                    RelativePositionAxes = Axes.None,
                    X = 0, Y = -75,
                    Width = 65,
                    Height = 65,
                    Icon = FontAwesome.Solid.PauseCircle,
                    BackgroundColour = Colour4.Transparent,
                    HoverColour = Colour4.FromHex("3333337A"),
                    Action = onPlayButtonClicked
                },
                repeatButton = new KCSIconButton()
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.None,
                    RelativePositionAxes = Axes.None,
                    X = -55, Y = -75,
                    Width = 35,
                    Height = 35,
                    Icon = FontAwesome.Solid.Redo,
                    BackgroundColour = Colour4.Transparent,
                    HoverColour = Colour4.FromHex("3333337A"),
                    Action = onRepeatButtonClicked
                },
                unknownButton = new KCSIconButton()
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.None,
                    RelativePositionAxes = Axes.None,
                    X = 55, Y = -75,
                    Width = 35,
                    Height = 35,
                    Icon = FontAwesome.Solid.Question,
                    BackgroundColour = Colour4.Transparent,
                    HoverColour = Colour4.FromHex("3333337A"),
                    Action = onUnknownButtonClicked
                }
            };
            background.Colour = Colour4.Transparent;
            //background.Colour = new osu.Framework.Graphics.Colour.ColourInfo()
            //{
            //    TopLeft = Colour4.FromHex("5FBDFF"),
            //    BottomLeft = Colour4.FromHex("4CB9E7"),
            //    TopRight = Colour4.FromHex("6DB9EF"),
            //    BottomRight = Colour4.FromHex("39A7FF"),
            //};
            trackProgressBar.OnUserChangeValue = seekTrack;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audioManager)
        {
            KartStorageResourceStore kartResource = new KartStorageResourceStore(storageSystem);

            ITrackStore trackStore = audioManager.GetTrackStore(kartResource);
            track = trackStore.Get("sound_/bgm/castle2/castle_01_re.ogg");
            drawableTrack = new DrawableTrack(track);
            drawableTrack.Completed += onDrawableTrackCompleted;
            drawableTrack.Start();
            trackProgressBar.MaxValue = track.Length;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            osuTK.Vector2 moveVec = new osuTK.Vector2(50f, 0);
            songTitle
                .MoveToOffset(-moveVec)
                .Delay(100)
                .FadeTo(1.0f, 920, Easing.OutQuint)
                .MoveToOffset(moveVec, 920, Easing.OutQuint);
            songFileName
                .MoveToOffset(-moveVec)
                .Delay(150)
                .FadeTo(1.0f, 1090, Easing.OutQuint)
                .MoveToOffset(moveVec, 1090, Easing.OutQuint);
        }

        protected override void Update()
        {
            base.Update();
            double boxUpdateDuration = Time.Current - prevBoxesUpdateTimestamp;
            double threshold = 15;
            //float boxWidth = 0.3f + Math.Clamp((DrawSize.X / 350));
            for (int i = 0; i < amplitudeBoxesCount; i++)
            {
                var updateBox = amplitudeBoxes[i];
                if(updateBox is Container container)
                {
                    container.CornerRadius = container.DrawSize.X / 2f;
                }
            }
            if (boxUpdateDuration >= threshold && drawableTrack is not null)
            {
                var freqAmps = drawableTrack.CurrentAmplitudes.FrequencyAmplitudes.Span;
                for(int i = 0; i < amplitudeBoxesCount; i++)
                {
                    var updateBox = amplitudeBoxes[i];
                    int freqIndex = i * (freqAmps.Length - 1) / (amplitudeBoxesCount - 1);
                    float freqAmpValue = freqAmps[(freqAmps.Length >> 1) - 1 - Math.Min(freqIndex, (freqAmps.Length - 1 - freqIndex))];
                    if (updateBox.Height < freqAmpValue)
                    {
                        updateBox.ClearTransforms();
                        updateBox.Height = freqAmpValue;
                        updateBox.FadeIn().ResizeHeightTo(0, threshold * 30, Easing.None).FadeTo(0.6f, threshold * 30, Easing.OutCubic);
                    }
                }
                trackProgressBar.Value = track.CurrentTime;
                TimeSpan trackCurrentTime = TimeSpan.FromMilliseconds(track.CurrentTime);
                TimeSpan trackLength = TimeSpan.FromMilliseconds(track.Length);
                trackTime.Text = $"{trackCurrentTime:mm\\:ss} / {trackLength:mm\\:ss}";
                if (unknwonMode)
                {
                    double val = Math.Pow(drawableTrack.CurrentTime / drawableTrack.Length, 3) * 3;
                    drawableTrack.Frequency.Value = Math.Clamp(1 + val, 0.3, 4);
                    amplitudeBoxes.ScaleTo((float)(drawableTrack.Frequency.Value - 1) * 5 + 1, 320, Easing.OutQuint);
                    unknownButton.RotateTo((float)(drawableTrack.Frequency.Value - 1) * 720, 320, Easing.OutQuint);
                }
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            track.Stop();
            track.Dispose();
            base.Dispose(isDisposing);
        }

        private void seekTrack(double time)
        {
            time = Math.Clamp(time, 0, track.Length - 0.3f);
            track.Seek(time);
        }

        private void onPlayButtonClicked()
        {
            if (track is not null)
                if (drawableTrack.IsRunning)
                {
                    drawableTrack.Stop();
                    playButton.Icon = FontAwesome.Solid.PlayCircle;
                }
                else
                {
                    if (drawableTrack.CurrentTime >= drawableTrack.Length)
                        drawableTrack.Seek(0);
                    drawableTrack.Start();
                    playButton.Icon = FontAwesome.Solid.PauseCircle;
                }
        }

        private void onRepeatButtonClicked()
        {
            if(track is not null)
                if(drawableTrack.Looping)
                {
                    repeatButton.BackgroundColour = Colour4.Transparent;
                    drawableTrack.Looping = false;
                }
                else
                {
                    repeatButton.BackgroundColour = Colour4.FromHex("637E76");
                    drawableTrack.Looping = true;
                    if (drawableTrack.CurrentTime >= drawableTrack.Length)
                        playButton.Icon = FontAwesome.Solid.PauseCircle;
                }
        }

        private void onUnknownButtonClicked()
        {
            if (track is not null)
                if (unknwonMode)
                {
                    drawableTrack.Frequency.Value = 1;
                    unknownButton.BackgroundColour = Colour4.Transparent;
                    unknownButton.RotateTo(0, 720, Easing.OutElasticQuarter);
                    amplitudeBoxes.ScaleTo(1, 720, Easing.OutElasticQuarter);
                    unknwonMode = false;
                }
                else
                {
                    unknownButton.BackgroundColour = Colour4.FromHex("637E765F");
                    unknwonMode = true;
                }
        }

        private void onDrawableTrackCompleted()
        {
            playButton.Icon = FontAwesome.Solid.PlayCircle;
            drawableTrack.Stop();
        }
        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (drawableTrack.IsRunning && !unknwonMode)
            {
                if(e.Key == osuTK.Input.Key.P)
                {
                    drawableTrack.Frequency.Value = Math.Clamp(drawableTrack.Frequency.Value * 1.005, 1, 2.86);
                }
                if(e.Key == osuTK.Input.Key.O)
                {
                    drawableTrack.Frequency.Value = Math.Clamp(drawableTrack.Frequency.Value * 0.995, 0.35, 1);
                }
                if (e.Key == osuTK.Input.Key.I)
                {
                    float relMouseX = ((e.MousePosition.X / DrawSize.X) * 2) - 1;
                    float relMouseY = ((e.MousePosition.Y / DrawSize.Y) * 2) - 1;
                    double val = (relMouseX + relMouseY) * 1.86;
                    double scale = val < 0 ? (1 / (1 - val)) : (1 + val);
                    drawableTrack.Frequency.Value = Math.Clamp(scale, 0.35, 2.86);
                    
                }
                songTitle.ScaleTo(new osuTK.Vector2((float)drawableTrack.Frequency.Value, songTitle.Scale.Y), 320, Easing.OutQuint);
            }
            return base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            if (e.Key == osuTK.Input.Key.P || e.Key == osuTK.Input.Key.O || e.Key == osuTK.Input.Key.I)
                if (drawableTrack.IsRunning)
                {
                    drawableTrack.Frequency.Value = 1;
                    songTitle.ScaleTo(1, 720, Easing.OutElasticQuarter);
                }
            base.OnKeyUp(e);
        }
    }
}
