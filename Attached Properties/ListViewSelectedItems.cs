using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Product_Manager.Attached_Properties
{
    public static class ListViewSelectedItems
    {
        /// <summary>
        /// SelectedItems Attached Property
        /// -------------------------------
        /// 1. Registering this attached property to dependency property.
        /// 2. Type of property is IList.
        /// 3. Included callback function OnSelectedItemsChanged.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItems",
                typeof(IList),
                typeof(ListViewSelectedItems),
                new PropertyMetadata(null, OnSelectedItemsChanged));

        /// <summary>
        /// GetSelectedItems Function
        /// -------------------------
        /// 1. Gets the value of the attached property.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>
        ///     1. returns value of DependencyObject as IList.
        /// </returns>
        public static IList GetSelectedItems(DependencyObject obj) =>
            (IList)obj.GetValue(SelectedItemsProperty);

        /// <summary>
        /// SetsSelectedItems Function
        /// --------------------------
        /// 1. Sets the value of the attached property from the value parameter.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetSelectedItems(DependencyObject obj, IList value) =>
            obj.SetValue(SelectedItemsProperty, value);

        /// <summary>
        /// OnSelectedItemsChanged Callback Function
        /// ----------------------------------------
        /// 1. Triggers when SelectedItems property changed.
        /// 2. Ensures attached ListView synchronizes its SelectedItems with new value.
        /// 3. Removes any existing SelectionChanged handler from ListView
        /// 4. If new value is an IList, then it attaches the handler and synchronizes the ListView's selected items with new list.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// SynchronizeSelectedItems Function
        /// ---------------------------------
        /// 1. Ensures that the external IList is updated with ListView's selected items.
        /// 2. Removes any existing SelectionChanged handler from ListView.
        /// 3. The ListView.SelectedItems collection reflects the external IList.
        /// 4. Removes all ListView.SelectedItems from external IList.
        /// 5. Adds all external IList items to ListView.SelectedItems.
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="selectedItems"></param>
        private static void SynchronizeSelectedItems(ListView listView, IList selectedItems)
        {
            listView.SelectionChanged -= ListView_SelectionChanged;

            foreach (object item in listView.SelectedItems)
            {
                selectedItems.Remove(item);
            }
            foreach (object item in selectedItems)
            {
                listView.SelectedItems.Add(item);
            }
            listView.SelectionChanged += ListView_SelectionChanged;
        }

        /// <summary>
        /// ListView_SelectionChanged Function
        /// ----------------------------------
        /// 1. Updates the external IList whenever the ListView selection changes.
        /// 2. Items removed from the ListView's selection are removed from the IList.
        /// 3. Items added to the ListView's selection are added to the IList.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView)
            {
                IList selectedItems = GetSelectedItems(listView);

                if (selectedItems != null)
                {
                    foreach (object item in e.RemovedItems)
                    {
                        selectedItems.Remove(item);
                    }
                    foreach (object item in e.AddedItems)
                    {
                        selectedItems.Add(item);
                    }
                }
            }
        }
    }
}
