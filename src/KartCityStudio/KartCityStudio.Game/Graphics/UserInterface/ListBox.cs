using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Layout;
using osu.Framework.Localisation;
using osu.Framework.Threading;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    // Refers to osu.Framework.Graphics.Menu
    public abstract partial class ListBox: CompositeDrawable
    {
        protected readonly ScrollContainer<Drawable> ContentContainer;
        protected readonly Drawable Background;
        protected readonly Container MaskingContainer;

        private Colour4 backgroundColour;
        private FillFlowContainer<DrawableListBoxItem> itemsFlow;
        private DrawableListBoxItem? selectedListBoxItem;

        public ListBoxItemList Items { get; } = new ListBoxItemList();

        public Colour4 BackgroundColour
        {
            get => backgroundColour;
            set
            {
                backgroundColour = value;
                Scheduler.AddOnce(UpdateBackgroundColour);
            }
        }

        public ListBoxItem? SelectedItem => selectedListBoxItem?.Item;

        public IEnumerable<LocalisableString> FilterTerms => Items.Select(x => x.Text.Value);

        public new float CornerRadius
        {
            get => MaskingContainer.CornerRadius;
            set => MaskingContainer.CornerRadius = value;
        }

        protected Container<DrawableListBoxItem> ItemsContainer => itemsFlow;

        protected internal IReadOnlyList<DrawableListBoxItem> Children => itemsFlow.Children;

        protected ListBox()
        {
            Items.OnInsert = onItemsInsert;
            Items.OnRemove = onItemsRemove;
            Items.OnClear = onItemsClear;

            InternalChild = MaskingContainer = new Container
            {
                Name = "Our contents",
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                Children = new Drawable[]
                {
                    Background = CreateBackground(),
                    ContentContainer = CreateScrollContainer(Direction.Vertical).With(d =>
                    {
                        d.RelativeSizeAxes = Axes.Both;
                        d.Masking = true;
                        d.Child = itemsFlow = new ItemsFlow() { Direction = FillDirection.Vertical };
                    }),
                }
            };

            itemsFlow.RelativeSizeAxes = Axes.X;
            itemsFlow.AutoSizeAxes = Axes.Y;
        }

        protected virtual Drawable CreateBackground() => new Box() { RelativeSizeAxes = Axes.Both };

        protected abstract DrawableListBoxItem CreateDrawableListBoxItem(ListBoxItem item);

        protected abstract ScrollContainer<Drawable> CreateScrollContainer(Direction direction);

        protected virtual void UpdateBackgroundColour()
        {
            Background.FadeColour(backgroundColour);
        }

        private void onItemsInsert(int index, ListBoxItem item)
        {
            DrawableListBoxItem drawableItem = CreateDrawableListBoxItem(item);
            drawableItem.Clicked = onItemClicked;

            var items = Children.OrderBy(itemsFlow.GetLayoutPosition).ToList();

            for (int i = index; i < items.Count; i++)
                itemsFlow.SetLayoutPosition(items[i], i + 1);

            itemsFlow.Insert(index, drawableItem);
            ((IItemsFlow)itemsFlow).SizeCache.Invalidate();
        }

        private void onItemsRemove(int index)
        {
            var items = Children.OrderBy(itemsFlow.GetLayoutPosition).ToList();
            for (int i = index + 1; i < items.Count; i++)
                itemsFlow.SetLayoutPosition(items[i], i - 1);
            itemsFlow.Remove(items[index], true);
            ((IItemsFlow)itemsFlow).SizeCache.Invalidate();
        }

        private void onItemsClear()
        {
            itemsFlow.Clear();
        }

        private void onItemClicked(DrawableListBoxItem clickedItem)
        {
            if (selectedListBoxItem is not null)
                selectedListBoxItem.State = ListBoxItemState.NotSelected;
            selectedListBoxItem = clickedItem;
            selectedListBoxItem.State = ListBoxItemState.Selected;
        }

        public abstract partial class DrawableListBoxItem: CompositeDrawable, IStateful<ListBoxItemState>
        {
            public readonly ListBoxItem Item;
            protected readonly Drawable Background;
            protected readonly Container Foreground;
            protected readonly Drawable Content;
            internal Action<DrawableListBoxItem> Clicked;

            private ListBoxItemState state;
            private Colour4 backgroundColour;
            private Colour4 backgroundHoverColour;
            private Colour4 backgroundSelectedColour;
            private Colour4 foregroundColour = Colour4.White;
            private Colour4 foregroundHoverColour = Colour4.White;
            private Colour4 foregroundSelectedColour = Colour4.White;

            public event Action<ListBoxItemState> StateChanged;

            public ListBoxItemState State
            {
                get => state;
                set
                {
                    state = value;
                    Scheduler.AddOnce(() =>
                    {
                        UpdateBackgroundColour();
                        UpdateForegroundColour();
                    });
                }
            }

            public bool IsSelected => state == ListBoxItemState.Selected;

            public Colour4 BackgroundColour
            {
                get => backgroundColour;
                set
                {
                    backgroundColour = value;
                    Scheduler.AddOnce(UpdateBackgroundColour);
                }
            }

            public Colour4 BackgroundHoverColour
            {
                get => backgroundHoverColour;
                set
                {
                    backgroundHoverColour = value;
                    Scheduler.AddOnce(UpdateBackgroundColour);
                }
            }

            public Colour4 BackgroundSelectedColour
            {
                get => backgroundSelectedColour;
                set
                {
                    backgroundSelectedColour = value;
                    Scheduler.AddOnce(UpdateBackgroundColour);
                }
            }

            public Colour4 ForegroundColour
            {
                get => backgroundColour;
                set
                {
                    foregroundColour = value;
                    Scheduler.AddOnce(UpdateForegroundColour);
                }
            }

            public Colour4 ForegroundHoverColour
            {
                get => foregroundHoverColour;
                set
                {
                    foregroundHoverColour = value;
                    Scheduler.AddOnce(UpdateForegroundColour);
                }
            }

            public Colour4 ForegroundSelectedColour
            {
                get => foregroundSelectedColour;
                set
                {
                    foregroundSelectedColour = value;
                    Scheduler.AddOnce(UpdateForegroundColour);
                }
            }

            protected DrawableListBoxItem(ListBoxItem item)
            {
                Item = item;
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;

                InternalChildren = new Drawable[]
                {
                    Background = CreateBackground(),
                    Foreground = new Container()
                    {
                        AutoSizeAxes = Axes.Both,
                        Child = Content = CreateContent()
                    }
                };

                if(Content is IHasText textContent)
                {
                    textContent.Text = Item.Text.Value;
                    Item.Text.ValueChanged += e => textContent.Text = e.NewValue;
                }
            }

            protected virtual void UpdateBackgroundColour()
            {
                Background.FadeColour(
                    IsSelected ? backgroundSelectedColour :
                    IsHovered ? backgroundHoverColour :
                    backgroundColour);
            }

            protected virtual void UpdateForegroundColour()
            {
                Foreground.FadeColour(
                    IsSelected ? foregroundSelectedColour :
                    IsHovered ? foregroundHoverColour :
                    foregroundColour);
            }

            protected virtual Drawable CreateBackground() => new Box() { RelativeSizeAxes = Axes.Both };

            protected abstract Drawable CreateContent();

            protected override bool OnHover(HoverEvent e)
            {
                Scheduler.AddOnce(UpdateBackgroundColour);
                Scheduler.AddOnce(UpdateForegroundColour);
                return false;
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                Scheduler.AddOnce(UpdateBackgroundColour);
                Scheduler.AddOnce(UpdateForegroundColour);
            }

            protected override bool OnClick(ClickEvent e)
            {
                Clicked?.Invoke(this);
                Item.ClickAction.Value?.Invoke();
                return true;
            }

            protected override bool OnDoubleClick(DoubleClickEvent e)
            {
                Item.DoubleClickAction.Value?.Invoke();
                return true;
            }
        }

        internal interface IItemsFlow: IFillFlowContainer
        {
            LayoutValue SizeCache { get; }
        }

        internal partial class ItemsFlow: FillFlowContainer<DrawableListBoxItem>, IItemsFlow
        {
            public LayoutValue SizeCache => new LayoutValue(Invalidation.RequiredParentSizeToFit, InvalidationSource.Self);

            public ItemsFlow()
            {
                AddLayout(SizeCache);
            }
        }
    }

    public class ListBoxItemList: IList<ListBoxItem>
    {
        private readonly HashSet<ListBoxItem> itemSearchingCache = new HashSet<ListBoxItem>();
        private readonly List<ListBoxItem> items = new List<ListBoxItem>();

        internal Action<int, ListBoxItem> OnInsert;
        internal Action<int> OnRemove;
        internal Action OnClear;

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public ListBoxItem this[int index]
        {
            get => items[index];
            set
            {
                if(index >= Count)
                    throw new ArgumentOutOfRangeException("index");
                RemoveAt(index);
                Insert(index, value);
            }
        } 

        public void Add(ListBoxItem item)
        {
            Insert(Count, item);
        }

        public void Clear()
        {
            OnClear?.Invoke();
            items.Clear();
        }

        public bool Contains(ListBoxItem item)
        {
            return itemSearchingCache.Contains(item);
        }

        public void CopyTo(ListBoxItem[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public bool Remove(ListBoxItem item)
        {
            if(!Contains(item))
                return false;
            for(int i = 0; i < Count; i++)
                if (items[i] == item)
                {
                    RemoveAt(i);
                }    
            return true;
        }

        public IEnumerator<ListBoxItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public int IndexOf(ListBoxItem item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, ListBoxItem item)
        {
            items.Insert(index, item);
            OnInsert?.Invoke(index, item);
        }

        public void RemoveAt(int index)
        {
            OnRemove?.Invoke(index);
            itemSearchingCache.Remove(items[index]);
            items.RemoveAt(index);
        }
    }

    public enum ListBoxItemState
    {
        NotSelected,
        Selected,
    }
}
