using System.ComponentModel;

namespace Crowbar
{
    public class BindingListExAutoSort<T> : BindingListEx<T>
    {
        public BindingListExAutoSort(string nProperty)
        {
            theSortedPropertyName = nProperty;
            theSortedProperty = FindPropertyDescriptor(nProperty);
        }

        protected override void InsertItem(int index, T item)
        {
            InsertItemSorted(index, item, theSortedProperty);
        }

        // Public Overloads Sub ResetItem(ByVal index As Integer)
        // MyBase.ResetItem(index)
        // End Sub

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            // If e.ListChangedType = ListChangedType.ItemChanged AndAlso e.PropertyDescriptor IsNot Nothing AndAlso e.PropertyDescriptor.Name = Me.theSortedPropertyName Then
            // Dim obj As Object = Me.Items(e.NewIndex)
            // MyBase.ApplySortCore(Me.theSortedProperty, ListSortDirection.Ascending)
            // Dim aEventArgs As New ListChangedEventArgs(ListChangedType.ItemMoved, Me.IndexOf(CType(obj, T)), e.NewIndex)
            // MyBase.OnListChanged(aEventArgs)
            // Else
            // MyBase.OnListChanged(e)
            // End If
            // ======
            // NOTE: Raise an extra new event, ItemMoved, so that widgets can know when an item moved because of auto-sorting.
            if (e.ListChangedType == ListChangedType.ItemChanged && e.PropertyDescriptor is object && (e.PropertyDescriptor.Name ?? "") == (theSortedPropertyName ?? ""))
            {
                object obj = Items[e.NewIndex];
                Items.RemoveAt(e.NewIndex);
                int insertionIndex;
                insertionIndex = FindInsertionIndex(0, (T)obj, theSortedProperty);
                Items.Insert(insertionIndex, (T)obj);
                var aEventArgs = new ListChangedEventArgs(ListChangedType.ItemMoved, insertionIndex, e.NewIndex);
                base.OnListChanged(aEventArgs);
                // Else
                // MyBase.OnListChanged(e)
            }

            base.OnListChanged(e);
        }

        private string theSortedPropertyName;
        private PropertyDescriptor theSortedProperty;
    }
}