using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Product_Manager.Attached_Properties
{
    public static class ListViewSelectedItems
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItems",
                typeof(IList),
                typeof(ListViewSelectedItems),
                new PropertyMetadata(null, OnSelectedItemsChanged));

        public static IList GetSelectedItems(DependencyObject obj) =>
            (IList)obj.GetValue(SelectedItemsProperty);

        public static void SetSelectedItems(DependencyObject obj, IList value) =>
            obj.SetValue(SelectedItemsProperty, value);

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListView listView)
            {
                listView.SelectionChanged -= ListView_SelectionChanged;

                if (e.NewValue is IList newSelectedItems)
                {
                    listView.SelectionChanged += ListView_SelectionChanged;

                    SynchronizeSelectedItems(listView, newSelectedItems);
                }
            }
        }

        private static void SynchronizeSelectedItems(ListView listView, IList selectedItems)
        {
            listView.SelectionChanged -= ListView_SelectionChanged;

            foreach (var item in listView.SelectedItems)
            {
               selectedItems.Remove(item);
            }
            foreach (var item in selectedItems)
            {
                listView.SelectedItems.Add(item);
            }
            listView.SelectionChanged += ListView_SelectionChanged;
        }

        private static void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is ListView listView)
            {
                var selectedItems = GetSelectedItems(listView);

                if(selectedItems != null)
                {
                    foreach( var item in e.RemovedItems)
                    {
                        selectedItems.Remove(item);
                    }
                    foreach (var item in e.AddedItems)
                    {
                        selectedItems.Add(item);
                    }
                }
            }
        }
    }
}
