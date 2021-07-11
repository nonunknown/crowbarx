using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Crowbar
{
    class PropertyComparer<TKey> : Comparer<TKey>
    {
        private PropertyDescriptor _property;
        private ListSortDirection _direction;

        public PropertyComparer(PropertyDescriptor nProperty, ListSortDirection direction) : base()
        {
            _property = nProperty;
            _direction = direction;
        }

        public override int Compare(TKey xVal, TKey yVal)
        {
            var xValue = GetPropertyValue(xVal, _property.Name);
            var yValue = GetPropertyValue(yVal, _property.Name);
            if (_direction == ListSortDirection.Ascending)
            {
                return CompareAscending(xValue, yValue);
            }
            else
            {
                return CompareDescending(xValue, yValue);
            }
        }

        public new bool Equals(TKey xVal, TKey yVal)
        {
            return xVal.Equals(yVal);
        }

        public new int GetHashCode(TKey obj)
        {
            return obj.GetHashCode();
        }

        private int CompareAscending(object xValue, object yValue)
        {
            int result;
            // If (xValue IsNot Nothing) Then
            // result = CType(xValue, IComparable).CompareTo(yValue)
            // ElseIf xValue.Equals(yValue) Then
            // result = 0
            // Else
            // result = xValue.ToString.CompareTo(yValue.ToString)
            // End If
            if (xValue is null)
            {
                if (yValue is null)
                {
                    result = 0;
                }
                else
                {
                    result = -1;
                }
            }
            else if (yValue is null)
            {
                result = 1;
            }
            else
            {
                result = ((IComparable)xValue).CompareTo(yValue);
            }

            return result;
        }

        private int CompareDescending(object xValue, object yValue)
        {
            return CompareAscending(xValue, yValue) * -1;
        }

        private object GetPropertyValue(TKey value, string nProperty)
        {
            var propertyInfo = value.GetType().GetProperty(nProperty);
            return propertyInfo.GetValue(value, null);
        }
    }
}