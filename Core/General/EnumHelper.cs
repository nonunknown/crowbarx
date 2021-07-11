using System;
using System.Collections;
using System.Collections.Generic;

namespace Crowbar
{
    public sealed class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("value");
            }

            string description = value.ToString();
            var fieldInfo = value.GetType().GetField(description);
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (attributes is object && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }

            return description;
        }

        public static IList ToList(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException("type");
            }
            // Dim list As ArrayList = New ArrayList()
            var list = new List<KeyValuePair<Enum, string>>();
            var enumValues = Enum.GetValues(type);
            foreach (Enum value in enumValues)
                list.Add(new KeyValuePair<Enum, string>(value, GetDescription(value)));
            return list;
        }

        public static void InsertIntoList(int index, Enum value, ref IList list)
        {
            list.Insert(index, new KeyValuePair<Enum, string>(value, GetDescription(value)));
        }

        public static void RemoveFromList(Enum value, ref IList list)
        {
            list.Remove(new KeyValuePair<Enum, string>(value, GetDescription(value)));
        }

        public static bool Contains(Enum value, IList list)
        {
            return list.Contains(new KeyValuePair<Enum, string>(value, GetDescription(value)));
        }

        public static Enum FindKeyFromDescription(string description, IList list)
        {
            Enum key = null;
            foreach (KeyValuePair<Enum, string> pair in list)
            {
                if ((pair.Value ?? "") == (description ?? ""))
                {
                    key = pair.Key;
                    break;
                }
            }

            return key;
        }

        public static int IndexOf(Enum key, IList list)
        {
            return list.IndexOf(new KeyValuePair<Enum, string>(key, GetDescription(key)));
        }

        public static int IndexOfKeyAsString(string keyText, IList list)
        {
            int index = -1;
            for (int pairIndex = 0, loopTo = list.Count - 1; pairIndex <= loopTo; pairIndex++)
            {
                KeyValuePair<Enum, string> pair = (KeyValuePair<Enum, string>)list[pairIndex];
                if ((pair.Key.ToString() ?? "") == (keyText ?? ""))
                {
                    index = pairIndex;
                    break;
                }
            }

            return index;
        }

        public static int IndexOfKeyAsCaseInsensitiveString(string keyText, IList list)
        {
            int index = -1;
            for (int pairIndex = 0, loopTo = list.Count - 1; pairIndex <= loopTo; pairIndex++)
            {
                KeyValuePair<Enum, string> pair = (KeyValuePair<Enum, string>)list[pairIndex];
                if ((pair.Key.ToString().ToLower() ?? "") == (keyText.ToLower() ?? ""))
                {
                    index = pairIndex;
                    break;
                }
            }

            return index;
        }

        public static int IndexOf(string description, IList list)
        {
            int index = -1;
            Enum key = null;
            key = FindKeyFromDescription(description, list);
            if (key is object)
            {
                index = list.IndexOf(new KeyValuePair<Enum, string>(key, GetDescription(key)));
            }

            return index;
        }

        public static Enum Key(int index, IList list)
        {
            KeyValuePair<Enum, string> pair = (KeyValuePair<Enum, string>)list[index];
            return pair.Key;
        }

        public static string Value(int index, IList list)
        {
            KeyValuePair<Enum, string> pair = (KeyValuePair<Enum, string>)list[index];
            return pair.Value;
        }
    }
}