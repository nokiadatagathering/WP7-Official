using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Interactivity;
using System.Collections.ObjectModel;

public struct InnerIndexes
{
    public int MainIndex { get; set; }

    public int InnerIndex { get; set; }
}

namespace NDG.Helpers.Behaviors
{
    public static class ItemsControlExtensions
    {
        public static void ScrollToVerticalOffset(this ItemsControl control, double offset, double actualHeight)
        {
            var scrollViewer = VisualTreeHelper.GetChild((DependencyObject)control, 0) as ScrollViewer;
            double scrollHeight = (offset * scrollViewer.ScrollableHeight) / actualHeight;
            if (double.IsNaN(scrollHeight))
            {
                scrollHeight = 0;
            }

            scrollViewer.ScrollToVerticalOffset(scrollHeight);
        }

        public static double GetOffset(this ItemsControl control, int index)
        {
            double result = 0;
            for (int i = 0; i < index; i++)
            {
                var item = control.ItemContainerGenerator.ContainerFromIndex(i) as ContentPresenter;
                if (item != null)
                {
                    result += item.ActualHeight;
                }
            }

            return result;
        }
    }

    public class ScrollItemsControlBehavior : Behavior<ItemsControl>
    {
        private const double AVARAGE_ITEM_HEIGHT = 130;

        private bool isNeedScroll = false;

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(InnerIndexes), typeof(ScrollItemsControlBehavior), new PropertyMetadata(OnSelectedIndexesChanged));

        public static readonly DependencyProperty AllItemsCountProperty = DependencyProperty.Register("AllItemsCount", typeof(int), typeof(ScrollItemsControlBehavior), new PropertyMetadata(null));

        public int AllItemsCount
        {
            get { return (int)this.GetValue(AllItemsCountProperty); }
            set { this.SetValue(AllItemsCountProperty, value); }
        }

        public InnerIndexes SelectedItem
        {
            get { return (InnerIndexes)this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }

        private static void OnSelectedIndexesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (ScrollItemsControlBehavior)sender;
            behavior.isNeedScroll = true;
            if (behavior.AssociatedObject != null)
            {
                TryScroll(behavior);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += this.OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TryScroll(this);
        }

        private static void TryScroll(ScrollItemsControlBehavior behavior)
        {
            if (behavior.isNeedScroll)
            {
                var offset = behavior.AssociatedObject.GetOffset(behavior.SelectedItem.MainIndex);
                if (behavior.SelectedItem.InnerIndex != 0)
                {
                    offset += behavior.SelectedItem.InnerIndex * AVARAGE_ITEM_HEIGHT;
                }

                double actualHeight = AVARAGE_ITEM_HEIGHT * behavior.AllItemsCount;
                behavior.AssociatedObject.ScrollToVerticalOffset(offset, actualHeight);
                behavior.isNeedScroll = false;
            }
        }
    }
}
