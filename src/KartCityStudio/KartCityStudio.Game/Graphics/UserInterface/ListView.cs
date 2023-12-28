using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using osu.Framework;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Layout;
using osu.Framework.Localisation;
using osu.Framework.Threading;
using Vulkan;

namespace KartCityStudio.Game.Graphics.UserInterface
{
    // Refers to osu.Framework.Graphics.Menu
    public abstract partial class ListView: CompositeDrawable
    {
        protected readonly ScrollContainer<Drawable> ContentContainer;
        protected readonly Drawable Background;
        protected readonly Container MaskingContainer;

        private Colour4 backgroundColour;
        private FillFlowContainer<DrawableListViewItem> itemsFlow;
        private FillFlowContainer<DrawableListViewHeaderItem> headersFlow;
        private Container headerRowContainer;
        private Box headerBackground;
        private DrawableListViewItem? selectedListViewItem;

        public event Action<ListViewItem> ItemClicked;
        public event Action<ListViewItem> ItemDoubleClicked;
        public event Action SelectdItemChanged;

        public ListViewItemList Items { get; } = new ListViewItemList();
        public ListViewHeaderItemList Headers { get; } = new ListViewHeaderItemList();

        public Colour4 BackgroundColour
        {
            get => backgroundColour;
            set
            {
                backgroundColour = value;
                Scheduler.AddOnce(UpdateBackgroundColour);
            }
        }

        public Colour4 HeaderColour
        {
            get => headerBackground.Colour;
            set => headerBackground.Colour = value;
        }

        public ListViewItem? SelectedItem => selectedListViewItem?.Item;

        public new float CornerRadius
        {
            get => MaskingContainer.CornerRadius;
            set => MaskingContainer.CornerRadius = value;
        }

        protected Container<DrawableListViewItem> ItemsContainer => itemsFlow;

        protected Container<DrawableListViewHeaderItem> HeadersContainer => headersFlow;

        protected internal IReadOnlyList<DrawableListViewItem> Children => itemsFlow.Children;

        protected ListView()
        {
            Items.OnInsert = onItemsInsert;
            Items.OnRemove = onItemsRemove;
            Items.OnClear = onItemsClear;

            Headers.OnInsert = onHeadersInsert;
            Headers.OnRemove = onHeadersRemove;
            Headers.OnClear = onHeadersClear;


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
                        d.RelativePositionAxes = Axes.X;
                        d.Masking = true;
                        d.Child = itemsFlow = new ItemsFlow() { Direction = FillDirection.Vertical };
                        d.Padding = new MarginPadding() { Top = 27f };
                    }),
                    headerRowContainer = new Container()
                    {
                        RelativeSizeAxes= Axes.X,
                        Height = 27f,
                        Children = new Drawable[]
                        {
                            headerBackground = new Box()
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Colour4.FromHex("1A1A1A")
                            },
                            headersFlow = new HeadersFlow()
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Direction = FillDirection.Horizontal
                            }
                        },
                    },
                }
            };

            itemsFlow.RelativeSizeAxes = Axes.X;
            itemsFlow.AutoSizeAxes = Axes.Y;
        }

        protected virtual Drawable CreateBackground() => new Box() { RelativeSizeAxes = Axes.Both };

        protected abstract DrawableListViewItem CreateDrawableListViewItem(ListViewItem item);

        protected abstract DrawableListViewHeaderItem CreateDrawableListViewHeaderItem(ListViewHeaderItem header);

        protected abstract ScrollContainer<Drawable> CreateScrollContainer(Direction direction);

        protected virtual void UpdateBackgroundColour()
        {
            Background.FadeColour(backgroundColour);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.IsPressed(osuTK.Input.Key.Down))
            {
                if (selectedListViewItem is null && itemsFlow.Count > 0)
                    selectedListViewItem = itemsFlow[0];
                else
                {
                    float pos = itemsFlow.GetLayoutPosition(selectedListViewItem);
                    if (pos < itemsFlow.Count - 1)
                    {
                        selectedListViewItem.State = ListViewItemState.NotSelected;

                        DrawableListViewItem nextSelectedItem = itemsFlow[(int)pos + 1];
                        selectedListViewItem = nextSelectedItem;
                    }
                }
                selectedListViewItem.State = ListViewItemState.Selected;
                if((selectedListViewItem.Y + selectedListViewItem.DrawHeight + ContentContainer.ScrollContent.Y) > (ContentContainer.ChildSize.Y))
                {
                    float scrollOffset = (selectedListViewItem.Y + selectedListViewItem.DrawHeight + ContentContainer.ScrollContent.Y) - ContentContainer.ChildSize.Y;
                    ContentContainer.ScrollBy(scrollOffset, true);
                }
                else if ((selectedListViewItem.Y + ContentContainer.ScrollContent.Y) < 0)
                {
                    float scrollOffset = selectedListViewItem.Y + ContentContainer.ScrollContent.Y;
                    ContentContainer.ScrollBy(scrollOffset, true);
                }
                return true;
            }
            else if (e.IsPressed(osuTK.Input.Key.Up))
            {
                if (selectedListViewItem is null && itemsFlow.Count > 0)
                    selectedListViewItem = itemsFlow[0];
                else
                {
                    float pos = itemsFlow.GetLayoutPosition(selectedListViewItem);
                    if (pos > 0)
                    {
                        selectedListViewItem.State = ListViewItemState.NotSelected;
                        
                        DrawableListViewItem nextSelectedItem = itemsFlow[(int)pos - 1];
                        selectedListViewItem = nextSelectedItem;
                    }
                }
                selectedListViewItem.State = ListViewItemState.Selected;
                if ((selectedListViewItem.Y + selectedListViewItem.DrawHeight + ContentContainer.ScrollContent.Y) > (ContentContainer.ChildSize.Y))
                {
                    float scrollOffset = (selectedListViewItem.Y + selectedListViewItem.DrawHeight + ContentContainer.ScrollContent.Y) - ContentContainer.ChildSize.Y;
                    ContentContainer.ScrollBy(scrollOffset, true);
                }
                else if ((selectedListViewItem.Y + ContentContainer.ScrollContent.Y) < 0)
                {
                    float scrollOffset = selectedListViewItem.Y + ContentContainer.ScrollContent.Y;
                    ContentContainer.ScrollBy(scrollOffset, true);
                }
                return true;
            }
            return base.OnKeyDown(e);
        }

        protected override void Dispose(bool isDisposing)
        {
            Items.Clear();
            Headers.Clear();
            GC.Collect();
            base.Dispose(isDisposing);
        }

        private void onItemsInsert(int index, ListViewItem item)
        {
            item.ClickAction.Value = onItemClicked;
            item.DoubleClickAction.Value = onItemDoubleClicked;

            DrawableListViewItem drawableItem = CreateDrawableListViewItem(item);
            drawableItem.Clicked = onDrawableItemClicked;
            drawableItem.UpdateListViewHeader(Headers);

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

        private void onItemClicked(ListViewItem clickedItem)
        {
            ItemClicked?.Invoke(clickedItem);
        }

        private void onItemDoubleClicked(ListViewItem clickedItem)
        {
            ItemDoubleClicked?.Invoke(clickedItem);
        }

        private void onDrawableItemClicked(DrawableListViewItem clickedItem)
        {
            if (selectedListViewItem is not null)
                selectedListViewItem.State = ListViewItemState.NotSelected;
            selectedListViewItem = clickedItem;
            selectedListViewItem.State = ListViewItemState.Selected;
            SelectdItemChanged?.Invoke();
        }

        private void onHeadersInsert(int index, ListViewHeaderItem item)
        {
            DrawableListViewHeaderItem drawableHeaderItem = CreateDrawableListViewHeaderItem(item).With(d =>
            {
                
            });
            drawableHeaderItem.Clicked = onHeaderClicked;

            var items = headersFlow.Children.OrderBy(headersFlow.GetLayoutPosition).ToList();

            for (int i = index; i < items.Count; i++)
                headersFlow.SetLayoutPosition(items[i], i + 1);

            headersFlow.Insert(index, drawableHeaderItem);
            ((IHeadersFlow)headersFlow).SizeCache.Invalidate();

            item.FieldWidth.ValueChanged += _ => updateHeaderToItems();
            updateHeaderToItems();
        }

        private void onHeadersRemove(int index)
        {
            var items = headersFlow.Children.OrderBy(headersFlow.GetLayoutPosition).ToList();
            for (int i = index + 1; i < items.Count; i++)
                headersFlow.SetLayoutPosition(items[i], i - 1);
            headersFlow.Remove(items[index], true);
            updateHeaderToItems();
        }

        private void onHeadersClear()
        {
            headersFlow.Clear(true);
            updateHeaderToItems();
        }

        private void onHeaderClicked(DrawableListViewHeaderItem clickedHeader)
        {
            
        }

        private void updateHeaderToItems()
        {
            foreach (DrawableListViewItem drawableItem in itemsFlow)
                drawableItem.UpdateListViewHeader(Headers);
        }

        public abstract partial class DrawableListViewItem: CompositeDrawable, IStateful<ListViewItemState>
        {
            public readonly ListViewItem Item;
            protected readonly Drawable Background;
            protected readonly Container Foreground;
            protected readonly FillFlowContainer<Drawable> ContentsContainer;
            internal Action<DrawableListViewItem> Clicked;

            private ListViewItemState state;
            private Colour4 backgroundColour;
            private Colour4 backgroundHoverColour;
            private Colour4 backgroundSelectedColour;
            private Colour4 foregroundColour = Colour4.White;
            private Colour4 foregroundHoverColour = Colour4.White;
            private Colour4 foregroundSelectedColour = Colour4.White;
            private Dictionary<string, Drawable> colNameToContent = new Dictionary<string, Drawable>();


            public event Action<ListViewItemState> StateChanged;

            public ListViewItemState State
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

            public bool IsSelected => state == ListViewItemState.Selected;

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

            protected DrawableListViewItem(ListViewItem item)
            {
                Item = item;
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;

                InternalChildren = new Drawable[]
                {
                    Background = CreateBackground(),
                    Foreground = new Container()
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Child = ContentsContainer = new FillFlowContainer<Drawable>()
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Masking = true,
                            Direction = FillDirection.Horizontal,
                        }
                    }
                };

                Item.Texts.CollectionChanged += (sender, args) =>
                {
                    switch (args.Action)
                    {
                        case NotifyDictionaryChangedAction.Add:
                            foreach(var newItem in args.NewItems)
                            {
                                if (colNameToContent.ContainsKey(newItem.Key) && colNameToContent[newItem.Key] is IHasText textContent)
                                {
                                    textContent.Text = newItem.Value;
                                }
                            }
                            break;
                        case NotifyDictionaryChangedAction.Remove:
                            foreach (var removedItem in args.OldItems)
                            {
                                if (colNameToContent.ContainsKey(removedItem.Key) && colNameToContent[removedItem.Key] is IHasText textContent)
                                {
                                    textContent.Text = "";
                                }
                            }
                            break;
                        case NotifyDictionaryChangedAction.Replace:
                            foreach (var replacedItem in args.NewItems)
                            {
                                if (colNameToContent.ContainsKey(replacedItem.Key) && colNameToContent[replacedItem.Key] is IHasText textContent)
                                {
                                    textContent.Text = replacedItem.Value;
                                }
                            }
                            break;
                    }
                };
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
                Item.ClickAction.Value?.Invoke(Item);
                return true;
            }

            protected override bool OnDoubleClick(DoubleClickEvent e)
            {
                Item.DoubleClickAction.Value?.Invoke(Item);
                return true;
            }

            internal void UpdateListViewHeader(IList<ListViewHeaderItem> header)
            {
                var items = ContentsContainer.ToList();
                Dictionary<string, Drawable> contentMap = new Dictionary<string, Drawable>();
                foreach(Drawable drawable in items)
                    if(!contentMap.ContainsKey(drawable.Name))
                        contentMap.Add(drawable.Name, drawable);
                for(int i = 0; i < header.Count; i++)
                {
                    ListViewHeaderItem headerItem = header[i];
                    Drawable contentContainer;
                    if (contentMap.ContainsKey(headerItem.Name))
                    {
                        contentContainer = contentMap[headerItem.Name];
                        contentMap.Remove(headerItem.Name);
                        ContentsContainer.SetLayoutPosition(contentContainer, i);
                    }
                    else
                    {
                        Drawable content = CreateContent().With(d =>
                        {
                            d.RelativeSizeAxes = Axes.X;
                        });
                        if(content is IHasText textContent)
                        {
                            textContent.Text = Item.Texts.GetValueOrDefault(headerItem.Name);
                        }
                        ContentsContainer.Insert(i, contentContainer = new Container()
                        {
                            Name = headerItem.Name,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Masking = true,
                            Child = content,
                        });
                        colNameToContent.Add(headerItem.Name, content);
                    }
                    contentContainer.Width = headerItem.FieldWidth.Value;
                }
                foreach (KeyValuePair<string, Drawable> keyValuePair in contentMap)
                {
                    colNameToContent.Remove(keyValuePair.Key);
                    ContentsContainer.Remove(keyValuePair.Value, true);
                }
            }
        }

        public abstract partial class DrawableListViewHeaderItem : CompositeDrawable
        {
            public readonly ListViewHeaderItem Item;
            protected readonly Drawable Background;
            protected readonly FillFlowContainer<Drawable> Foreground;
            protected readonly Drawable Content;
            internal Action<DrawableListViewHeaderItem> Clicked;


            private Colour4 backgroundColour;
            private Colour4 backgroundHoverColour;
            private Colour4 foregroundColour = Colour4.White;
            private Colour4 foregroundHoverColour = Colour4.White;

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

            protected DrawableListViewHeaderItem(ListViewHeaderItem item)
            {
                Item = item;
                Width = item.FieldWidth.Value;
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;

                InternalChildren = new Drawable[]
                {
                    Background = CreateBackground(),
                    Foreground = new FillFlowContainer<Drawable>()
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Horizontal,
                        Child = Content = CreateContent(),
                    }
                };

                if(Content is IHasText textContent)
                {
                    Item.Text.ValueChanged += e => textContent.Text = e.NewValue;
                    textContent.Text = Item.Text.Value;
                }

                item.FieldWidth.ValueChanged += e => this.ResizeWidthTo(e.NewValue);
            }

            protected virtual void UpdateBackgroundColour()
            {
                Background.FadeColour(
                    IsHovered ? backgroundHoverColour :
                    backgroundColour);
            }

            protected virtual void UpdateForegroundColour()
            {
                Foreground.FadeColour(
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
                return true;
            }

            protected override bool OnDoubleClick(DoubleClickEvent e)
            {
                return true;
            }
        }

        internal interface IItemsFlow: IFillFlowContainer
        {
            LayoutValue SizeCache { get; }
        }

        internal partial class ItemsFlow: FillFlowContainer<DrawableListViewItem>, IItemsFlow
        {
            public LayoutValue SizeCache => new LayoutValue(Invalidation.RequiredParentSizeToFit, InvalidationSource.Self);

            public ItemsFlow()
            {
                AddLayout(SizeCache);
            }
        }

        internal interface IHeadersFlow : IFillFlowContainer
        {
            LayoutValue SizeCache { get; }
        }

        internal partial class HeadersFlow : FillFlowContainer<DrawableListViewHeaderItem>, IHeadersFlow
        {
            public LayoutValue SizeCache => new LayoutValue(Invalidation.RequiredParentSizeToFit, InvalidationSource.Self);

            public HeadersFlow()
            {
                AddLayout(SizeCache);
            }
        }


    }

    public class ListViewItemList: IList<ListViewItem>
    {
        private readonly HashSet<ListViewItem> itemSearchingCache = new HashSet<ListViewItem>();
        private readonly List<ListViewItem> items = new List<ListViewItem>();

        internal Action<int, ListViewItem> OnInsert;
        internal Action<int> OnRemove;
        internal Action OnClear;

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public ListViewItem this[int index]
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

        public void Add(ListViewItem item)
        {
            Insert(Count, item);
        }

        public void Clear()
        {
            OnClear?.Invoke();
            items.Clear();
        }

        public bool Contains(ListViewItem item)
        {
            return itemSearchingCache.Contains(item);
        }

        public void CopyTo(ListViewItem[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public bool Remove(ListViewItem item)
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

        public IEnumerator<ListViewItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public int IndexOf(ListViewItem item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, ListViewItem item)
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

    public class ListViewHeaderItemList : IList<ListViewHeaderItem>
    {
        private readonly HashSet<string> usedHeaderNames = new HashSet<string>();
        private readonly List<ListViewHeaderItem> items = new List<ListViewHeaderItem>();

        internal Action<int, ListViewHeaderItem> OnInsert;
        internal Action<int> OnRemove;
        internal Action OnClear;

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public ListViewHeaderItem this[int index]
        {
            get => items[index];
            set
            {
                if (index >= Count)
                    throw new ArgumentOutOfRangeException("index");
                RemoveAt(index);
                Insert(index, value);
            }
        }

        public void Add(ListViewHeaderItem item)
        {
            Insert(Count, item);
        }

        public void Clear()
        {
            OnClear?.Invoke();
            items.Clear();
        }

        public bool Contains(ListViewHeaderItem item)
        {
            return usedHeaderNames.Contains(item.Name);
        }

        public void CopyTo(ListViewHeaderItem[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public bool Remove(ListViewHeaderItem item)
        {
            bool removed = false;
            for (int i = 0; i < Count; i++)
                if (items[i] == item)
                {
                    RemoveAt(i);
                    removed = true;
                }
            return removed;
        }

        public IEnumerator<ListViewHeaderItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public int IndexOf(ListViewHeaderItem item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, ListViewHeaderItem item)
        {
            if (usedHeaderNames.Contains(item.Name))
                throw new Exception();
            items.Insert(index, item);
            usedHeaderNames.Add(item.Name);
            OnInsert?.Invoke(index, item);
        }

        public void RemoveAt(int index)
        {
            usedHeaderNames.Remove(items[index].Name);
            items.RemoveAt(index);
            OnRemove?.Invoke(index);
        }
    }

    public enum ListViewItemState
    {
        NotSelected,
        Selected,
    }
}
