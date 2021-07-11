using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Crowbar
{

    // <XmlRoot("BindigList")> _
    public class BindingListEx<T> : BindingList<T>, IEquatable<BindingListEx<T>>
    {

        #region Create and Destroy

        public BindingListEx() : base()
        {

            // failedToAdd = False
        }

        #endregion

        #region Operators

        public override bool Equals(object otherObject)
        {
            BindingListEx<T> otherList = (BindingListEx<T>)otherObject;
            if (otherList is object)
            {
                return Equals(otherList);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(BindingListEx<T> otherList)
        {
            if (otherList is null)
            {
                return false;
            }

            bool result = false;
            if (Count == otherList.Count)
            {
                result = true;
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    if (!this[i].Equals(otherList[i]))
                    {
                        return false;
                    }
                }
            }

            return result;
        }

        #endregion

        #region Properties

        [XmlIgnore()]
        public object Container
        {
            get
            {
                return theContainer;
            }

            set
            {
                theContainer = value;
            }
        }

        #endregion

        #region Methods

        // Public Function GetItemByName(ByVal name As String) As T
        // Dim sortedProperty As PropertyDescriptor = FindPropertyDescriptor("Name")
        // If sortedProperty IsNot Nothing Then
        // Dim items As List(Of T) = CType(Me.Items, List(Of T))
        // If items IsNot Nothing Then
        // Dim pc As PropertyComparer(Of T) = New PropertyComparer(Of T)(sortedProperty, ListSortDirection.Ascending)
        // Dim itemIndex As Integer = items.BinarySearch(Item, pc)
        // If itemIndex >= 0 Then
        // Return items(itemIndex)
        // End If
        // End If
        // End If
        // Return Nothing
        // End Function

        public void InsertItemSorted(int index, T item, string nProperty)
        {
            _sort = FindPropertyDescriptor(nProperty);
            InsertItemSorted(index, item, _sort);
        }

        public void InsertItemSorted(int index, T item, PropertyDescriptor sortedProperty)
        {
            index = FindInsertionIndex(index, item, sortedProperty);
            base.InsertItem(index, item);
        }

        public void Sort()
        {
            ApplySortCore(_sort, _dir);
        }

        public void Sort(string nProperty)
        {
            _sort = FindPropertyDescriptor(nProperty);
            ApplySortCore(_sort, _dir);
        }

        public void Sort(string nProperty, ListSortDirection direction)
        {
            _sort = FindPropertyDescriptor(nProperty);
            _dir = direction;
            ApplySortCore(_sort, _dir);
        }

        public override void EndNew(int itemIndex)
        {
            if (_sort is object && itemIndex == Count - 1)
            {
                ApplySortCore(_sort, _dir);
            }

            base.EndNew(itemIndex);
        }

        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods

        // Override so that an extra ListChanged event with ListChangedType.ItemDeleted  
        // is raised BEFORE the item is deleted. 
        // NOTE: Can't set NewIndex to negative number, so change OldIndex to something other than -1.
        // NewIndex = item's index that will be deleted.
        // OldIndex = -2 (to distinguish from normal ListChangedType.ItemDeleted event).
        protected override void RemoveItem(int index)
        {
            try
            {
                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index, -2));
            }
            catch
            {
            }

            var removedItem = Items[index];
            // NOTE: The base class RemoveItem raises the expected ListChanged event with 
            // ListChangedType.ItemDeleted and the already-deleted item's index.
            base.RemoveItem(index);
        }

        protected int FindInsertionIndex(int index, T item, PropertyDescriptor sortedProperty)
        {
            var insertionIndex = default(int);
            List<T> items = (List<T>)Items;
            if (items is object & sortedProperty is object)
            {
                var pc = new PropertyComparer<T>(sortedProperty, ListSortDirection.Ascending);
                int itemIndex = items.BinarySearch(item, pc);
                if (itemIndex < 0)
                {
                    insertionIndex = itemIndex ^ -1;
                }
                else
                {
                    // Find last (instead of arbitrary) index with the given value.
                    insertionIndex = itemIndex + 1;
                    while (insertionIndex < items.Count)
                    {
                        itemIndex = items.BinarySearch(insertionIndex, items.Count - insertionIndex, item, pc);
                        if (itemIndex < 0)
                        {
                            break;
                        }
                        // insertionIndex += 1
                        insertionIndex = itemIndex + 1;
                    }
                }
            }

            return insertionIndex;
        }

        protected override bool SupportsSortingCore
        {
            get
            {
                return true;
            }
        }

        protected override bool IsSortedCore
        {
            get
            {
                return _isSorted;
            }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get
            {
                return _dir;
            }
        }

        protected override void ApplySortCore(PropertyDescriptor nProperty, ListSortDirection direction)
        {
            List<T> items = (List<T>)Items;
            if (items is object & nProperty is object)
            {
                var pc = new PropertyComparer<T>(nProperty, direction);
                items.Sort(pc);
                _isSorted = true;
            }
            // NOTE: Although this is convention to raise a "Reset" event, require code to manually call a reset instead.
            // ' Raise the ListChanged Reset event so bound controls refresh their values.
            // Me.OnListChanged(New ListChangedEventArgs(ListChangedType.Reset, -1))
            else
            {
                _isSorted = false;
            }
        }

        protected override void RemoveSortCore()
        {
            _isSorted = false;
        }

        protected PropertyDescriptor FindPropertyDescriptor(string nProperty)
        {
            var pdc = TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor prop = null;
            if (pdc is object)
            {
                prop = pdc.Find(nProperty, true);
            }

            return prop;
        }

        protected override bool SupportsSearchingCore
        {
            get
            {
                return true;
            }
        }

        protected override int FindCore(PropertyDescriptor propertyDesc, object key)
        {
            int i;
            var propInfo = typeof(T).GetProperty(propertyDesc.Name);
            T item;
            if (key is object)
            {
                var loopTo = Count - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    item = Items[i];
                    if (propInfo.GetValue(item, null).Equals(key))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        // ' Override so that an extra ListChanged event with ListChangedType.Reset  
        // ' is raised BEFORE the items are cleared. 
        // Protected Overrides Sub ClearItems()
        // 'Try
        // '	Me.OnListChanged(New ListChangedEventArgs(ListChangedType.Reset, 0, 0))
        // 'Catch
        // 'End Try
        // MyBase.ClearItems()
        // End Sub

        // Protected Overrides Sub SetItem(ByVal index As Integer, ByVal item As T)
        // MyBase.SetItem(index, item)
        // End Sub

        // Protected Overrides Function AddNewCore() As Object
        // Dim index As Integer
        // Dim obj As Object
        // obj = MyBase.AddNewCore()
        // index = Me.Items.Count - 1
        // If Me.RaiseListChangedEvents Then
        // Me.OnListChanged(New ListChangedEventArgs(ListChangedType.ItemAdded, index, -1))
        // End If
        // Return obj
        // End Function

        // Protected Overrides Sub InsertItem(ByVal index As Integer, ByVal item As T)
        // MyBase.InsertItem(index, item)
        // End Sub

        // <OnDeserialized()> _
        // Private Sub OnDeserialized(ByVal context As StreamingContext)
        // Dim items As List(Of t) = New List(Of t)(Me.Items)
        // Dim index As Integer = 0
        // '// call SetItem again on each item  
        // '// to re-establish event hookups
        // For Each item As t In items
        // '// explicitly call the base version 
        // '// in case SetItem is overridden
        // MyBase.SetItem(index, item)
        // index += 1
        // Next
        // End Sub

        #endregion

        #region Data

        private object theContainer;
        // Private failedToAdd As Boolean

        private bool _isSorted;
        private ListSortDirection _dir = ListSortDirection.Ascending;
        private PropertyDescriptor _sort = null;

        #endregion

    }
}